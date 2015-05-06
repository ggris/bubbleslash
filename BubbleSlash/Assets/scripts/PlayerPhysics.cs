using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {


	public int playerNumber;

	//physics settings
	public float ground_acc;
	public float air_acc;

	public float jump_speed;
	public float push_air_speed;

	public float ground_horizontal_drag;
	public float air_horizontal_drag;
	public float air_vertical_drag;

	public float max_falling_speed;
	public float max_horizontal_speed;

	//ables
	public int max_jumps;
	public int jumps_left;
	public bool is_grounded;
	public bool able_to_move;
	public bool able_to_jump;
	public bool able_to_attack;

	//privates
	private Rigidbody2D body;
	private Animator animator;
	private Vector2 direction;
	private Vector2 direction_input;
	private Vector2 direction_action;
	private float horizontal_direction;
	private GameObject weapon;
	private PlayerManager manager;

	//quick under tea
	public string key_jump;
	public string key_weapon;
	public string key_hat;
	public float dash_speed;
	
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		weapon = transform.Find("weapon").gameObject;
		manager = GameObject.Find ("playerManager").GetComponent<PlayerManager> ();

		jumps_left = max_jumps - 1;
		is_grounded = false;
		horizontal_direction = 1;		
		able_to_move = true;
		able_to_jump = true;

	}

	void logState(){
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
		{
			Debug.Log ("idle");
		}
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("walking"))
		{
			Debug.Log ("walking");
		}
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("falling"))
		{
			Debug.Log ("falling");
		}
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("jumping"))
		{
			Debug.Log ("jumping");
		}

	}

	void FixedUpdate(){
	}

	// Update is called once per frame
	void Update () {

		able_to_jump = isAbleToJump();
		able_to_move = isAbleToMove();
		able_to_attack = isAbleToAttack ();

		//set animation values
		animator.SetFloat ("inputX", direction_input.x);
		animator.SetFloat ("inputY", direction_input.y);
		animator.SetFloat ("speedX", body.velocity.x);
		animator.SetBool ("isOnFeet", is_grounded);

		if (Input.GetKeyDown(key_jump) && able_to_jump)
			animator.SetTrigger ("triggerJump");
		if (Input.GetKeyDown (key_weapon) && able_to_attack)
			animator.SetTrigger ("triggerAttack");

		//updates direction
		direction_input = directionFromInput ();
		direction = realDirection (direction_input);
		//controls
		checkAttack ();

		checkMove ();

		checkJump ();


		//max speeds
		checkMaxSpeeds ();



	}

	public Vector2 directionFromInput (){
		//return direction from input + update horizontalDirection
		Vector2 ans = new Vector2 (playerInputAxis("Horizontal"), playerInputAxis("Vertical"));
		ans.Normalize ();
		if(ans.x!=0)
			horizontal_direction=ans.x;

		return ans;
	}
	public Vector2 realDirection(Vector2 direction_input){
		if (direction_input.x==0 && direction_input.y==0)
			return new Vector2(horizontal_direction, 0);
		else
			return direction_input;
	}

	private float playerInputAxis(string inputName) {
		return Input.GetAxis("P" + playerNumber + " " + inputName);
	}

	public bool isAbleToJump(){
		bool ans = animator.GetCurrentAnimatorStateInfo(0).IsName("idle")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("walking")
				|| animator.GetCurrentAnimatorStateInfo(0).IsName("falling");
		return ans && (jumps_left > 0);
	}

	public bool isAbleToMove(){
		return !animator.GetCurrentAnimatorStateInfo (0).IsTag ("weapon");
	}

	public bool isAbleToAttack(){
		return true;
	}

	public void checkMove(){
		if (direction_input.x < 0 && able_to_move) {
			if (is_grounded){
				body.AddForce (-Vector2.right * ground_acc * Time.deltaTime);
			}
			else {
				body.AddForce (-Vector2.right * air_acc * Time.deltaTime);
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * air_horizontal_drag * Time.deltaTime);
				body.AddForce (new Vector2 (0f, -body.velocity.y) * air_vertical_drag * Time.deltaTime);
			}
		}
		if (direction_input.x > 0 && able_to_move) {
			if (is_grounded){
				body.AddForce (Vector2.right * ground_acc * Time.deltaTime);
			}
			else {
				body.AddForce (Vector2.right * air_acc * Time.deltaTime);
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * air_horizontal_drag * Time.deltaTime);
				body.AddForce (new Vector2 (0f, -body.velocity.y) * air_vertical_drag * Time.deltaTime);
			}
		}
	}

	public void checkJump (){
		if (Input.GetKeyDown(key_jump) && able_to_jump) {
			
			if (jumps_left > 0) {
				if (!is_grounded) {
					if (playerInputAxis("Horizontal") <= 0 && body.velocity.x > 0)
						body.velocity = new Vector2 (-push_air_speed, jump_speed);
					else if (playerInputAxis("Horizontal") >= 0 && body.velocity.x < 0)
						body.velocity = new Vector2 (push_air_speed, jump_speed);
					else 
						body.velocity = new Vector2 (body.velocity.x, jump_speed);
					
				} else
					body.velocity = new Vector2 (body.velocity.x, jump_speed);
			}
		}
	}

	public void checkMaxSpeeds(){
		if (body.velocity.x < -max_horizontal_speed) {
			body.velocity=new Vector2(-max_horizontal_speed,body.velocity.y);
		}
		if (body.velocity.x > max_horizontal_speed) {
			body.velocity=new Vector2(max_horizontal_speed,body.velocity.y);
		}
		if (body.velocity.y < -max_falling_speed) {
			body.velocity=new Vector2(body.velocity.x,-max_falling_speed);
		}
	}

	public void checkAttack(){
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("startAttack")) {

			direction_action=direction;
			weapon.SetActive(true);
			body.gravityScale=0;
			weapon.transform.localEulerAngles=new Vector3(0,0,getAngle(direction_action,new Vector2(1,0)));
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack")){
			body.velocity= direction_action * dash_speed;
		}

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("endAttack")){
			weapon.SetActive(false);
			body.gravityScale=5;
		}
	}

	public float getAngle (Vector2 a, Vector2 b){
		return Vector2.Angle (a, b) * -1 * Mathf.Sign (Vector3.Cross (new Vector3 (a.x, a.y, 0), new Vector3 (b.x, b.y, 0)).z);
	}

	public void isHit(){
		GameObject.Find ("playerManager").GetComponent<PlayerManager> ().dealWithDeath (playerNumber-1);
	}

}
