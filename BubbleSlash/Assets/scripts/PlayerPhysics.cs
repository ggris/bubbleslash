using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {


	public int playerNumber;

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

	public bool is_grounded;
	public bool is_touching_left;
	public bool is_touching_right;
	public bool able_to_move;
	public bool able_to_jump;
	public bool able_to_attack;
	public bool is_hitable;

	//privates
	private Rigidbody2D body;
	private Animator animator;

	private Vector2 direction_input;
	public Vector2 direction_action;
	public Vector2 direction_parry;
	public float horizontal_direction;
	private GameObject weapon;
	private Animator weapon_state;
	private PlayerManager manager;
	public GameObject hat;


	//"state"

	private bool is_wounded;


	void OnDestroy(){
		CameraTracking cam = (CameraTracking)FindObjectOfType (typeof(CameraTracking));
		if (cam!=null)
			cam.resetPlayers ();
	}
	void Awake(){
		body = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		weapon = transform.Find("weapon").gameObject;
		weapon_state = weapon.GetComponent<Animator> ();
		manager = GameObject.Find ("playerManager").GetComponent<PlayerManager> ();
	}
	void Start () {
		jumps_left = max_jumps - 1;
		is_grounded = false;
		horizontal_direction = 1;		
		able_to_move = true;
		able_to_jump = true;
		attack_start = Time.time;
		is_touching_left = false;
		is_touching_right = false;
		is_hitable = true;
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
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("sliding"))
		{
			Debug.Log ("sliding");
		}
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("walljumping"))
		{
			Debug.Log ("walljumping");
		}
		if( animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
		{
			Debug.Log ("attack");
		}

	}


	// Update is called once per frame
	void Update () {


		//updates direction
		if (isAttacking ()) {

			if (direction_action.x < 0) 
				horizontal_direction = -1;
			if (direction_action.x > 0) 
				horizontal_direction = 1;
		}
		else {
			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("hatSpecialState")){
				direction_input = directionFromInput ();
				//change horizontal direction if horizontal input is not equal to 0
				if (direction_input.x < 0) 
					horizontal_direction = -1;
				if (direction_input.x > 0) 
					horizontal_direction = 1;
				checkSlide (); //only changes "horizontal_direction" if sliding
			}
		}

		//set animator parameters values
		animator.SetFloat ("inputX", direction_input.x);
		animator.SetFloat ("inputY", direction_input.y);
		animator.SetFloat ("speedX", body.velocity.x);
		animator.SetBool ("isOnFeet", is_grounded);
		animator.SetBool ("isOnHand", is_touching_left || is_touching_right);
		animator.SetBool ("inputJump", playerInputButton ("Jump"));
		
		
		//animation, small blood and smoke
		//quick under tee
		Transform tr_animation = transform.Find ("animation");

		if (playerInputButtonDown ("Jump") && isAbleToJump ()) {
			animator.SetTrigger ("triggerJump");
			Transform tr_smoke = tr_animation.Find ("smoke");
			tr_smoke.gameObject.GetComponent<ParticleSystem>().Play();
			tr_smoke.eulerAngles = new Vector3 (tr_smoke.eulerAngles.x, 0, tr_smoke.eulerAngles.z);
		}

		if (playerInputButtonDown("Hat") && isInputFree() && hat.GetComponent<HatAbstractClass>().hasSpecialState() && hat.GetComponent<HatAbstractClass>().isNotInCd())
			animator.SetTrigger("inputHat");

		if (playerInputButtonDown ("Weapon") && isAbleToAttack()) {
			animator.SetTrigger ("triggerAttack");
			weapon_state.SetTrigger("input");
			direction_action=realDirection(directionFromInput());
			weapon.transform.localEulerAngles=new Vector3(0,0,getAngle(direction_action,new Vector2(1,0)));
		}

		//controls

		checkMove ();

		checkJump ();

		//max speeds
		checkMaxSpeeds ();

		if (isAttacking ()) {
			float a = getAngle (direction_action, horizontal_direction * Vector2.right);
			tr_animation.eulerAngles = new Vector3 (0,0, a);
			tr_animation.localScale = new Vector3(horizontal_direction, tr_animation.localScale.y,tr_animation.localScale.z);
		} 
		else {
			tr_animation.eulerAngles = new Vector3 (0, 0, 0);

			tr_animation.localScale = new Vector3(horizontal_direction, tr_animation.localScale.y,tr_animation.localScale.z);
			Transform tr_blood = tr_animation.Find ("small blood");
			tr_blood.eulerAngles = new Vector3 (tr_blood.eulerAngles.x, -horizontal_direction*90, tr_blood.eulerAngles.z);
		}

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("walking")) {
			Transform tr_smoke = tr_animation.Find ("smoke");
			tr_smoke.gameObject.GetComponent<ParticleSystem>().Play();
			tr_smoke.eulerAngles = new Vector3 (tr_smoke.eulerAngles.x, -horizontal_direction*90, tr_smoke.eulerAngles.z);
		} else {
			//tr_animation.Find("smoke").gameObject.GetComponent<ParticleSystem>().Clear();
			//tr_animation.Find("smoke").gameObject.GetComponent<ParticleSystem>().loop = false;
		}
		
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("sliding")){
			Transform tr_smoke = tr_animation.Find ("smoke");
			tr_smoke.gameObject.GetComponent<ParticleSystem>().Play();
			tr_smoke.eulerAngles = new Vector3 (tr_smoke.eulerAngles.x, horizontal_direction*120, tr_smoke.eulerAngles.z);

		}

		//death on fall
		if (transform.position.y < manager.death_altitude) {
			manager.dealWithDeath(playerNumber -1);
		}

	}

	public Vector2 directionFromInput (){
		//return direction from input + update horizontalDirection
		Vector2 ans = new Vector2 (playerInputAxis("Horizontal"), playerInputAxis("Vertical"));

		if (ans.x > 0)
			ans.x = 1;
		if (ans.x < 0)
			ans.x = -1;
		if (ans.y > 0)
			ans.y = 1;
		if (ans.y < 0)
			ans.y = -1;

		ans.Normalize ();



		return ans;
	}

	public Vector2 realDirection(Vector2 direction_input){
		if (direction_input.x==0 && direction_input.y==0)
			return new Vector2(horizontal_direction, 0);
		else
			return direction_input;
	}

	private float playerInputAxis(string inputName) {
		return Input.GetAxisRaw("P" + playerNumber + " " + inputName);
	}
	private bool playerInputButtonDown(string inputName){
		return Input.GetButtonDown("P" + playerNumber + " " + inputName);
	}
	private bool playerInputButton(string inputName){
		return Input.GetButton("P" + playerNumber + " " + inputName);
	}

	public bool isAbleToJump(){
		bool ans = animator.GetCurrentAnimatorStateInfo(0).IsName("idle")
				|| animator.GetCurrentAnimatorStateInfo(0).IsName("walking")
				|| animator.GetCurrentAnimatorStateInfo(0).IsName("falling");
		return (ans && (jumps_left > 0)) || animator.GetCurrentAnimatorStateInfo(0).IsName("sliding");
	}
	bool isAttacking (){
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("attack");
	}
	bool isOnHat(){
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("hatSpecialState");
	}
	bool canAttackOnHat(){
		return hat.GetComponent<HatAbstractClass>().canAttack();
	}
	bool isInputFree(){
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("idle")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("walking")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("falling")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("jumping")
			|| animator.GetCurrentAnimatorStateInfo(0).IsName("walljumping");
	}

	public bool isAbleToAttack(){
		return 
			(Time.time - attack_start >= attack_cd) 
			&& (isInputFree () 
			    || (canAttackOnHat () && isOnHat ()) 
			    || animator.GetCurrentAnimatorStateInfo (0).IsName ("sliding"));
	}

	public void checkMove(){
		if (direction_input.x < 0 && isInputFree()) {
			if (is_grounded){
				body.AddForce (-Vector2.right * ground_acc * Time.deltaTime);
			}
			else {
				body.AddForce (-Vector2.right * air_acc * Time.deltaTime);
			}
		}
		if (direction_input.x > 0 && isInputFree()) {
			if (is_grounded){
				body.AddForce (Vector2.right * ground_acc * Time.deltaTime);
			}
			else {
				body.AddForce (Vector2.right * air_acc * Time.deltaTime);
			}
		}
		if (isInputFree()) {
			if (is_grounded){
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * ground_horizontal_drag * Time.deltaTime);
			}
			else {
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * air_horizontal_drag * Time.deltaTime);
				body.AddForce (new Vector2 (0f, -body.velocity.y) * air_vertical_drag * Time.deltaTime);
			}
		}
	}
	public void checkSlide(){
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("sliding")){
			if (is_touching_left)
				horizontal_direction=1;
			else {
				if (is_touching_right)
					horizontal_direction=-1;
			}
		}
	}

	public void checkJump (){
		if (playerInputButtonDown("Jump") && isAbleToJump()) {
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("sliding")){
				body.velocity = new Vector2 (horizontal_direction * push_wall_speed, wall_jump_speed);
			}
			else {
				if (jumps_left > 0) {
					
					if (!is_grounded) {
						if (playerInputAxis("Horizontal") < -0.5 && body.velocity.x > 0)
							body.velocity = new Vector2 (-push_air_speed, jump_speed);
						else if (playerInputAxis("Horizontal") > 0.5 && body.velocity.x < 0)
							body.velocity = new Vector2 (push_air_speed, jump_speed);
						else 
							body.velocity = new Vector2 (body.velocity.x, jump_speed);
					
					} else
					body.velocity = new Vector2 (body.velocity.x, jump_speed);
				}
			}
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("jumping")) {

			if (body.velocity.y <carry_jump_speed){
				body.velocity = new Vector2(body.velocity.x, carry_jump_speed);
			}
		}
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("walljumping")) {
			
			if (body.velocity.y <carry_jump_speed){
				body.velocity = new Vector2(body.velocity.x, carry_jump_speed);
			}
		}

	}

	public void checkMaxSpeeds(){
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("falling")) {
			float a=directionFromInput ().normalized.y;
			if (a <0) {
				if (body.velocity.y<5 && body.velocity.y >=-10){
					body.velocity = new Vector2 (body.velocity.x, -10);
				}

				if (body.velocity.y<0 ){
					body.velocity = new Vector2 (body.velocity.x, body.velocity.y - a * fall_sprint_acc*Time.deltaTime*body.velocity.y);
				}

				if (body.velocity.y < -max_falling_speed_sprint)
					body.velocity = new Vector2 (body.velocity.x, -max_falling_speed_sprint);
			} 
			else {
				if (body.velocity.y < -max_falling_speed) 
					body.velocity = new Vector2 (body.velocity.x, -max_falling_speed);
			}
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("sliding"))
			if (body.velocity.y < -max_sliding_speed) 
				body.velocity=new Vector2(body.velocity.x,-max_sliding_speed);
		if (! animator.GetCurrentAnimatorStateInfo (0).IsName ("attack") && !animator.GetCurrentAnimatorStateInfo (0).IsName ("hatSpecialState")) {
			if (body.velocity.x < -max_horizontal_speed) {
				body.velocity = new Vector2 (-max_horizontal_speed, body.velocity.y);
			}
			if (body.velocity.x > max_horizontal_speed) {
				body.velocity = new Vector2 (max_horizontal_speed, body.velocity.y);
			}
		}
	}



	public float getAngle (Vector2 a, Vector2 b){
		return Vector2.Angle (a, b) * -1 * Mathf.Sign (Vector3.Cross (new Vector3 (a.x, a.y, 0), new Vector3 (b.x, b.y, 0)).z);
	}


	public void isHurt(){
		if (is_wounded) {
			manager.dealWithDeath (playerNumber-1);
			CancelInvoke("stopWound");
		}
		else {
			is_wounded = true;
			Invoke("startWound",0f);
			Invoke("stopWound",wound_length);
		}
	}
	public void isParried(Vector2 dir_parry){
		direction_parry = dir_parry;
		animator.SetTrigger ("parried");
	}

	public void startWound(){

		gameObject.transform.Find ("animation").Find ("small blood").gameObject.GetComponent<ParticleSystem> ().Play ();
	}
	public void stopWound(){
		gameObject.transform.Find ("animation").Find("small blood").gameObject.GetComponent<ParticleSystem> ().Stop ();
		is_wounded = false;
	}
	
	public void startAttack(){
		attack_start=Time.time;
	}
}
