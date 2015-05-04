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
	
	//privates
	private Rigidbody2D body;
	private Vector2 direction;
	private Vector2 direction_input;
	private float horizontal_direction;

	
	
	
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		jumps_left = max_jumps - 1;
		is_grounded = false;
		horizontal_direction = 1;		
		
		able_to_move = true;
		able_to_jump = true;
	}
	
	// Update is called once per frame
	void Update () {

		//updates direction
		direction_input = directionFromInput ();
		direction = realDirection (direction_input);
		//controls
		if (direction_input.x < 0 && able_to_move) {
			if (is_grounded){
				body.AddForce (-Vector2.right * ground_acc * Time.deltaTime);
				//body.AddForce (new Vector2 (body.velocity.x*body.velocity.x, 0f) * groundHorizontalDrag * Time.deltaTime);
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
				//body.AddForce (new Vector2 (-body.velocity.x*body.velocity.x, 0f) * groundHorizontalDrag * Time.deltaTime);
			}
			else {
				body.AddForce (Vector2.right * air_acc * Time.deltaTime);
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * air_horizontal_drag * Time.deltaTime);
				body.AddForce (new Vector2 (0f, -body.velocity.y) * air_vertical_drag * Time.deltaTime);
			}
		}
		
		if (Input.GetKeyDown ("space") && able_to_jump) {

			if (jumps_left > 0) {
				if (!is_grounded) {
					if (Input.GetKey ("left") && body.velocity.x > 0)
						body.velocity = new Vector2 (-push_air_speed, jump_speed);
					else if (Input.GetKey ("right") && body.velocity.x < 0)
						body.velocity = new Vector2 (push_air_speed, jump_speed);
					else 
						body.velocity = new Vector2 (body.velocity.x, jump_speed);
							
					jumps_left--;
				} else
					body.velocity = new Vector2 (body.velocity.x, jump_speed);
			}
		}

		//max speeds

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

	public Vector2 directionFromInput (){
		//return direction from input + update horizontalDirection
		
		float ansX = 0f;
		float ansY = 0f;
		if (Input.GetKey("left"))
			ansX -= 1f;
		if (Input.GetKey("right"))
			ansX += 1f;
		if (Input.GetKey("up"))
			ansY += 1f;
		if (Input.GetKey("down"))
			ansY -= 1f;
		
		if(ansX!=0)
			horizontal_direction=ansX;
		
		return new Vector2 (ansX, ansY);
	}
	
	public Vector2 realDirection(Vector2 direction_input){
		if (direction_input.x==0 && direction_input.y==0)
			return new Vector2(horizontal_direction, 0);
		else
			return direction_input;
	}

}
