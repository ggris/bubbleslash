using UnityEngine;
using System.Collections;

public class playerPhysics : MonoBehaviour {

	public int playerNumber;

	//physics settings
	public float groundAcc;
	public float airAcc;
	public float jumpSpeed;
	public float pushAirSpeed;
	public float airHorizontalDrag;
	public float airVerticalDrag;

	//ables
	public int maxJumps;
	public int jumpsLeft;
	public bool isGrounded;
	public bool ableToMove;
	public bool ableToJump;
	
	//privates
	private Rigidbody2D body;
	private Vector2 direction;
	private Vector2 directionInput;
	private float horizontalDirection;

	
	
	
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		jumpsLeft = maxJumps - 1;
		isGrounded = false;
		horizontalDirection = 1;		
		
		ableToMove = true;
		ableToJump = true;
	}
	
	// Update is called once per frame
	void Update () {

		//updates direction
		directionInput = directionFromInput ();
		direction = realDirection (directionInput);
		//controls
		if (directionInput.x < 0 && ableToMove) {
			if (isGrounded)
				body.AddForce (-Vector2.right * groundAcc * Time.deltaTime);
			else {
				body.AddForce (-Vector2.right * airAcc * Time.deltaTime);
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * airHorizontalDrag * Time.deltaTime);
				body.AddForce (new Vector2 (0f, -body.velocity.y) * airVerticalDrag * Time.deltaTime);
			}
		}
		if (directionInput.x > 0 && ableToMove) {
			if (isGrounded)
				body.AddForce (Vector2.right * groundAcc * Time.deltaTime);
			else {
				body.AddForce (Vector2.right * airAcc * Time.deltaTime);
				body.AddForce (new Vector2 (-body.velocity.x, 0f) * airHorizontalDrag * Time.deltaTime);
				body.AddForce (new Vector2 (0f, -body.velocity.y) * airVerticalDrag * Time.deltaTime);
			}
		}
		
		if (Input.GetKeyDown ("space") && ableToJump) {
			

			if (jumpsLeft > 0) {
				if (!isGrounded) {
					if (Input.GetKey ("left") && body.velocity.x > 0)
						body.velocity = new Vector2 (-pushAirSpeed, jumpSpeed);
					else if (Input.GetKey ("right") && body.velocity.x < 0)
						body.velocity = new Vector2 (pushAirSpeed, jumpSpeed);
					else 
						body.velocity = new Vector2 (body.velocity.x, jumpSpeed);
							
					jumpsLeft--;
				} else
					body.velocity = new Vector2 (body.velocity.x, jumpSpeed);
			}
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
			horizontalDirection=ansX;
		
		return new Vector2 (ansX, ansY);
	}
	
	public Vector2 realDirection(Vector2 directionInput){
		if (directionInput.x==0 && directionInput.y==0)
			return new Vector2(horizontalDirection, 0);
		else
			return directionInput;
	}

}
