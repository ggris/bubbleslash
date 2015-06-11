using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{


	public int playerNumber;

	public GameObject[] hat_prefabs;

	//physics settings
	public float ground_acc;
	public float air_acc;
	public float jump_speed;
	public float carry_jump_speed;
	public float push_air_speed;
	public float wall_jump_speed;
	public float push_wall_speed;
	public float ground_horizontal_drag;
	public float air_horizontal_drag;
	public float air_vertical_drag;
	public float max_falling_speed;
	public float max_falling_speed_sprint;
	public float fall_sprint_acc;
	public float max_horizontal_speed;
	public float max_sliding_speed;
	public float attack_cd;
	private float attack_start;
	public float parry_speed;
	public float dash_speed;
	public float wound_length;

	//ables
	public int max_jumps;
	public int jumps_left;
	public int max_attacks;
	public int attacks_left;

	public bool is_grounded_;
	public bool is_touching_left_;
	public bool is_touching_right_;

	public bool able_to_move;
	public bool able_to_jump;
	public bool able_to_attack;
	public bool is_hitable;

	//privates
	private Rigidbody2D body;
	private Animator animator;
	private Vector3 input_direction_;
	private bool input_jump_;
	private bool input_weapon_;
	private bool input_hat_;
	public Vector2 direction_action_;
	public Vector2 direction_parry;
	public float horizontal_direction;
	private GameObject weapon;
	private Animator weapon_state;
	public GameObject hat_GO;
	private PlayerSettings.Hat hat_choice;
	private NetworkView nview;

	//"state"

	private bool is_wounded_;
	private bool is_network_;

	void OnDestroy ()
	{
		CameraTracking cam = (CameraTracking)FindObjectOfType (typeof(CameraTracking));
		if (cam != null)
			cam.resetPlayers ();
	}

	void Awake ()
	{
		body = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		weapon = transform.Find ("weapon").gameObject;
		weapon_state = weapon.GetComponent<Animator> ();
		nview = GetComponent<NetworkView> ();
		DontDestroyOnLoad (transform.gameObject);
		is_network_ = Network.isServer || Network.isClient;
	}

	void Start ()
	{
		jumps_left = max_jumps - 1;
		is_grounded_ = false;
		horizontal_direction = 1;		
		able_to_move = true;
		able_to_jump = true;
		attack_start = Time.time;
		is_touching_left_ = false;
		is_touching_right_ = false;
		is_hitable = true;
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");
		Application.LoadLevel ("Launcher");
		Destroy(GameObject.Find("playerManager"));
		        Destroy(this.gameObject);
	}

	void logState ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			Debug.Log ("idle");
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("walking")) {
			Debug.Log ("walking");
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("falling")) {
			Debug.Log ("falling");
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("jumping")) {
			Debug.Log ("jumping");
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding")) {
			Debug.Log ("sliding");
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("walljumping")) {
			Debug.Log ("walljumping");
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("attack")) {
			Debug.Log ("attack");
		}

	}
	
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Vector3 syncDirection = Vector2.zero;

		if (stream.isWriting) {
			syncPosition = body.transform.position;
			syncVelocity = body.velocity;
			syncDirection = input_direction_;

			stream.Serialize (ref syncPosition);
			stream.Serialize (ref syncVelocity);
			stream.Serialize (ref syncDirection);

		} else {
			stream.Serialize (ref syncPosition);
			stream.Serialize (ref syncVelocity);
			stream.Serialize (ref syncDirection);
			body.transform.position = syncPosition;
			body.velocity = syncVelocity;
			input_direction_ = syncDirection;

		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (playerNumber != 0)
			UpdateLocal ();

		UpdateGlobal ();
	}

	void UpdateLocal ()
	{
		setDirectionFromInput ();
		
		//controls
		
		checkMove ();

		input_jump_ = playerInputButton ("Jump");
		input_hat_ = playerInputButtonDown ("Hat");
		input_weapon_ = playerInputButtonDown ("Weapon") & isAbleToAttack ();
		if (input_weapon_)
			triggerAttack ();

		checkJump ();

	}

	void UpdateGlobal ()
	{

		//updates direction
		if (isAttacking ()) {

			if (direction_action_.x < 0) 
				horizontal_direction = -1;
			if (direction_action_.x > 0) 
				horizontal_direction = 1;
		} else {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("hatSpecialState")) {
				//change horizontal direction if horizontal input is not equal to 0
				if (input_direction_.x < 0) 
					horizontal_direction = -1;
				if (input_direction_.x > 0) 
					horizontal_direction = 1;
				checkSlide (); //only changes "horizontal_direction" if sliding
			}
		}

		//set animator parameters values
		animator.SetFloat ("inputX", input_direction_.x);
		animator.SetFloat ("inputY", input_direction_.y);
		animator.SetFloat ("speedX", body.velocity.x);
		animator.SetBool ("isOnFeet", is_grounded_);
		animator.SetBool ("isOnHand", is_touching_left_ || is_touching_right_);
		animator.SetBool ("inputJump", input_jump_);
		animator.SetFloat ("hat", (float)hat_choice);
		
		//animation, small blood and smoke
		//quick under tee
		
		if (input_hat_ && isInputFree () && hat_GO.GetComponent<HatAbstractClass> ().hasSpecialState () && hat_GO.GetComponent<HatAbstractClass> ().isNotInCd ())
			animator.SetTrigger ("inputHat");	

		//max speeds
		checkMaxSpeeds ();
		Transform tr_animation = transform.Find ("animation");

		if (isAttacking ()) {
			float a = getAngle (direction_action_, horizontal_direction * Vector2.right);
			tr_animation.eulerAngles = new Vector3 (0, 0, a);
			tr_animation.localScale = new Vector3 (horizontal_direction, tr_animation.localScale.y, tr_animation.localScale.z);
		} else {
			tr_animation.eulerAngles = new Vector3 (0, 0, 0);

			tr_animation.localScale = new Vector3 (horizontal_direction, tr_animation.localScale.y, tr_animation.localScale.z);
			Transform tr_blood = tr_animation.Find ("small blood");
			tr_blood.eulerAngles = new Vector3 (tr_blood.eulerAngles.x, -horizontal_direction * 90, tr_blood.eulerAngles.z);
		}

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("walking")) {
			Transform tr_smoke = tr_animation.Find ("smoke");
			tr_smoke.gameObject.GetComponent<ParticleSystem> ().Play ();
			tr_smoke.eulerAngles = new Vector3 (tr_smoke.eulerAngles.x, -horizontal_direction * 90, tr_smoke.eulerAngles.z);
		}

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding")) {
			Transform tr_smoke = tr_animation.Find ("smoke");
			tr_smoke.gameObject.GetComponent<ParticleSystem> ().Play ();
			tr_smoke.eulerAngles = new Vector3 (tr_smoke.eulerAngles.x, horizontal_direction * 120, tr_smoke.eulerAngles.z);
		}

		//death on fall
		if (isLiving ()) {

			if (transform.position.y < -10) {
				Debug.Log ("fall !");
				StartCoroutine (die ());
			}
		}

	}

	public bool isInputJump()
	{
		return input_jump_;
	}

	void updateJump()
	{
		bool input_jump = playerInputButton ("Jump");
		if (is_network_) {
			if (input_jump != input_jump_)
				nview.RPC ("jumpRPC", RPCMode.All, input_jump);
		}
		else
			input_jump_ = input_jump;
	}

	[RPC]
	void jumpRPC (bool input_jump)
	{
		input_jump_ = input_jump;
	}

	public Vector2 getInputDirection ()
	{
		return input_direction_;
	}

	void setDirectionFromInput ()
	{
		//return direction from input + update horizontalDirection
		Vector2 ans = new Vector2 (playerInputAxis ("Horizontal"), playerInputAxis ("Vertical"));

		if (ans.x > 0)
			ans.x = 1;
		if (ans.x < 0)
			ans.x = -1;
		if (ans.y > 0)
			ans.y = 1;
		if (ans.y < 0)
			ans.y = -1;

		ans.Normalize ();

		input_direction_ = ans;
	}

	public Vector2 realDirection (Vector2 direction_input)
	{
		if (input_direction_.x == 0 && input_direction_.y == 0)
			return new Vector2 (horizontal_direction, 0);
		else
			return input_direction_;
	}

	float playerInputAxis (string inputName)
	{
		return Input.GetAxisRaw ("P" + playerNumber + " " + inputName);
	}

	bool playerInputButtonDown (string inputName)
	{
		return Input.GetButtonDown ("P" + playerNumber + " " + inputName);
	}

	bool playerInputButton (string inputName)
	{
		return Input.GetButton ("P" + playerNumber + " " + inputName);
	}

	public bool isAbleToJump ()
	{
		bool ans = animator.GetCurrentAnimatorStateInfo (0).IsName ("idle")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("walking")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("falling");
		return (ans && (jumps_left > 0)) || animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding");
	}

	bool isAttacking ()
	{
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("attack");
	}

	bool isOnHat ()
	{
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("hatSpecialState");
	}

	bool canAttackOnHat ()
	{
		return hat_GO.GetComponent<HatAbstractClass> ().canAttack ();
	}

	bool isInputFree ()
	{
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("idle")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("walking")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("falling")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("jumping")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("walljumping");
	}

	bool isLiving ()
	{
		return !(animator.GetCurrentAnimatorStateInfo (0).IsName ("dead")
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("die"));
	}

	public bool isAbleToAttack ()
	{
		return 
			(Time.time - attack_start >= attack_cd) 
			&& (isInputFree () 
			|| (canAttackOnHat () && isOnHat ()) 
			|| animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding"));
	}

	void triggerAttack()
	{
		if (is_network_)
			nview.RPC ("triggerAttackRPC", RPCMode.All, input_direction_);
		else
			triggerAttackRPC (input_direction_);
	}

	[RPC]
	void triggerAttackRPC (Vector3 input_direction)
	{
		input_direction_ = input_direction;
		animator.SetTrigger ("triggerAttack");
		weapon_state.SetTrigger ("input");
		direction_action_ = realDirection (input_direction_);
		weapon.transform.localEulerAngles = new Vector3 (0, 0, getAngle (direction_action_, new Vector2 (1, 0)));
	}

	public void checkMove ()
	{
		if (input_direction_.x < 0 && isInputFree ()) {
			if (is_grounded_) {
				addForce (-Vector2.right * ground_acc * Time.deltaTime);
			} else {
				addForce (-Vector2.right * air_acc * Time.deltaTime);
			}
		}
		if (input_direction_.x > 0 && isInputFree ()) {
			if (is_grounded_) {
				addForce (Vector2.right * ground_acc * Time.deltaTime);
			} else {
				addForce (Vector2.right * air_acc * Time.deltaTime);
			}
		}
		if (isInputFree ()) {
			if (is_grounded_) {
				addForce (new Vector2 (-body.velocity.x, 0f) * ground_horizontal_drag * Time.deltaTime);
			} else {
				addForce (new Vector2 (-body.velocity.x, 0f) * air_horizontal_drag * Time.deltaTime);
				addForce (new Vector2 (0f, -body.velocity.y) * air_vertical_drag * Time.deltaTime);
			}
		}
	}

	void addForce (Vector3 force)
	{
		if (is_network_)
			nview.RPC ("addForceRPC", RPCMode.All, force);
		else
			addForceRPC (force);
	}
	
	[RPC]
	void addForceRPC (Vector3 force)
	{
		body.AddForce (force);
	}

	public void checkSlide ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding")) {
			if (is_touching_left_)
				horizontal_direction = 1;
			else {
				if (is_touching_right_)
					horizontal_direction = -1;
			}
		}
	}

	public void checkJump ()
	{
		if (playerInputButtonDown ("Jump") && isAbleToJump ()) {

			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding")) {
				setBodyVelocity(new Vector2 (horizontal_direction * push_wall_speed, wall_jump_speed));

			} else {
				if (jumps_left > 0) {
					Vector3 velocity;

					if (!is_grounded_) {
						if (playerInputAxis ("Horizontal") < -0.5 && body.velocity.x > 0)
							velocity = new Vector2 (-push_air_speed, jump_speed);
						else if (playerInputAxis ("Horizontal") > 0.5 && body.velocity.x < 0)
							velocity = new Vector2 (push_air_speed, jump_speed);
						else 
							velocity = new Vector2 (body.velocity.x, jump_speed);
					
					} else {
						velocity = new Vector2 (body.velocity.x, jump_speed);
					}
					setBodyVelocity( velocity);


				}
			}
			animateJump();
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("jumping")) {

			if (body.velocity.y < carry_jump_speed) {
				setBodyVelocity( new Vector3 (body.velocity.x, carry_jump_speed));

			}
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("walljumping")) {
			
			if (body.velocity.y < carry_jump_speed) {
				setBodyVelocity( new Vector3 (body.velocity.x, carry_jump_speed));

			}
		}
	}

	void animateJump()
	{
		if (is_network_)
			nview.RPC ("animateJumpRPC", RPCMode.All, null);
		else
			animateJumpRPC();
	}

	[RPC]
	void animateJumpRPC ()
	{
		animator.SetTrigger ("triggerJump");
		Transform tr_animation = transform.Find ("animation");
		Transform tr_smoke = tr_animation.Find ("smoke");
		tr_smoke.gameObject.GetComponent<ParticleSystem> ().Play ();
		tr_smoke.eulerAngles = new Vector3 (tr_smoke.eulerAngles.x, 0, tr_smoke.eulerAngles.z);
		/*
			if (is_grounded){
				GetComponent<AudioSource>().pitch=2;
			}
			else{
				GetComponent<AudioSource>().pitch=2.5f;
			}
			GetComponent<AudioSource>().Play ();*/
	}

	void setBodyVelocity (Vector3 velocity)
	{
		if (is_network_)
			nview.RPC ("setBodyVelocityRPC", RPCMode.All, input_direction_, velocity);
		else
			body.velocity = velocity;
	}

	[RPC]
	void setBodyVelocityRPC (Vector3 direction, Vector3 velocity)
	{
		input_direction_ = direction;

		body.velocity = velocity;
	}

	public void checkMaxSpeeds ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("falling")) {
			float a = input_direction_.normalized.y;
			if (a < 0) {
				if (body.velocity.y < 5 && body.velocity.y >= -10) {
					body.velocity = new Vector2 (body.velocity.x, -10);
				}

				if (body.velocity.y < 0) {
					body.velocity = new Vector2 (body.velocity.x, body.velocity.y - a * fall_sprint_acc * Time.deltaTime * body.velocity.y);
				}

				if (body.velocity.y < -max_falling_speed_sprint)
					body.velocity = new Vector2 (body.velocity.x, -max_falling_speed_sprint);
			} else {
				if (body.velocity.y < -max_falling_speed) 
					body.velocity = new Vector2 (body.velocity.x, -max_falling_speed);
			}
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding"))
		if (body.velocity.y < -max_sliding_speed) 
			body.velocity = new Vector2 (body.velocity.x, -max_sliding_speed);
		if (! animator.GetCurrentAnimatorStateInfo (0).IsName ("attack") && !animator.GetCurrentAnimatorStateInfo (0).IsName ("hatSpecialState")) {
			if (body.velocity.x < -max_horizontal_speed) {
				body.velocity = new Vector2 (-max_horizontal_speed, body.velocity.y);
			}
			if (body.velocity.x > max_horizontal_speed) {
				body.velocity = new Vector2 (max_horizontal_speed, body.velocity.y);
			}
		}
	}

	public static float getAngle (Vector2 a, Vector2 b)
	{
		return Vector2.Angle (a, b) * -1 * Mathf.Sign (Vector3.Cross (new Vector3 (a.x, a.y, 0), new Vector3 (b.x, b.y, 0)).z);
	}

	public void isHurt ()
	{
		if (is_network_) {
			if (nview.isMine)
				nview.RPC ("isHurtRPC", RPCMode.All, is_wounded_);
		}
		else
			isHurtRPC (is_wounded_);
	}

	[RPC]
	void isHurtRPC(bool is_wounded)
	{
		if (is_wounded) {
			
			StartCoroutine (die ());
			stopWound ();
			CancelInvoke ("stopWound");
		} else {
			is_wounded_ = true;
			Invoke ("startWound", 0f);
			Invoke ("stopWound", wound_length);
		}
	}

	public void isParried (Vector2 dir_parry)
	{
		direction_parry = dir_parry;
		animator.SetTrigger ("parried");
	}

	public void startWound ()
	{
		gameObject.transform.Find ("animation").Find ("small blood").gameObject.GetComponent<ParticleSystem> ().Play ();
	}

	public void stopWound ()
	{
		gameObject.transform.Find ("animation").Find ("small blood").gameObject.GetComponent<ParticleSystem> ().Stop ();
		is_wounded_ = false;
	}
	
	public void startAttack ()
	{
		attack_start = Time.time;
	}

	public IEnumerator die ()
	{
		Debug.Log ("dead !");
		animator.SetTrigger ("die");
		is_hitable = false;
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		GetComponent<Rigidbody2D> ().gravityScale = 0;
		transform.Find ("animation").gameObject.SetActive (false);
		yield return new WaitForSeconds (1);
		respawn (new Vector2 (0, 0));


	}

	public void respawn (Vector2 pos)
	{
		animator.SetTrigger ("respawn");
		is_hitable = true;
		transform.Find ("animation").gameObject.SetActive (true);
		gameObject.transform.position = new Vector3 (pos.x, pos.y, 0);
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		GetComponent<Rigidbody2D> ().gravityScale = 5;

	}

	public void setHatChoice (PlayerSettings.Hat hat)
	{
		if (is_network_) {
			nview.RPC ("setHatChoiceRPC", RPCMode.AllBuffered, (int)hat);
		} else
			setHatChoiceRPC ((int) hat);
	}

	[RPC]
	void setHatChoiceRPC (int hat)
	{
		hat_choice = (PlayerSettings.Hat)hat;
		setHatRendering ();
		createHat ();
	}

	public void setColor (Color c)
	{
		Vector3 color = new Vector3 (c.r, c.g, c.b);
		if (is_network_) {
			nview.RPC ("setColorRPC", RPCMode.AllBuffered, color);
		} else
			setColorRPC(color);
	}

	[RPC]
	void setColorRPC (Vector3 color)
	{
		Color c = new Color(color.x, color.y, color.z);
		transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().color = c;
		transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().color = c;
		transform.Find ("animation").Find ("weapon_trans").gameObject.GetComponent<SpriteRenderer> ().color = c;
	}

	void setHatRendering ()
	{
		GameObject anim = transform.Find ("animation").gameObject;
		if (hat_choice == PlayerSettings.Hat.dashHat) {
			anim.transform.FindChild ("dodgeHat").gameObject.SetActive (false);
			anim.transform.FindChild ("dashHat").gameObject.SetActive (true);
		}
		if (hat_choice == PlayerSettings.Hat.dodgeHat) {
			anim.transform.FindChild ("dashHat").gameObject.SetActive (false);
			anim.transform.FindChild ("dodgeHat").gameObject.SetActive (true);
		}
	}

	void createHat(){
		hat_GO = GameObject.Instantiate (hat_prefabs [(int)hat_choice], new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		hat_GO.transform.parent = transform;
	}

}
