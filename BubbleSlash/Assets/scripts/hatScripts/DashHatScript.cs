using UnityEngine;
using System.Collections;

public class DashHatScript : HatAbstractClass {
	public float speed = 50f;
	private Vector2 direction;
	Transform tr_animation;


	override public void onSpecialStateEnter(){
		timer = 0f;
		direction = player.GetComponent<PlayerPhysics> ().getInputDirection ();
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;
		player.GetComponent<Rigidbody2D> ().velocity = speed * (direction.normalized);
		player.GetComponent<PlayerSounds> ().dash ();
		//float a = PlayerPhysics.getAngle (direction, player.GetComponent<PlayerPhysics>().horizontal_direction * Vector2.right);
		//tr_animation.eulerAngles = new Vector3 (0,0, a);
		//tr_animation.localScale = new Vector3(player.GetComponent<PlayerPhysics>().horizontal_direction, tr_animation.localScale.y,tr_animation.localScale.z);
	
	}

	override public void onSpecialStateExit(){
		player.GetComponent<Rigidbody2D> ().gravityScale = 5;
		player.GetComponent<Animator> ().SetTrigger ("stopHat");
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);

		tr_animation.eulerAngles = new Vector3 (0,0,0);
		//tr_animation.localScale = new Vector3(player.GetComponent<PlayerPhysics>().horizontal_direction, tr_animation.localScale.y,tr_animation.localScale.z);
	}
	override public bool hasSpecialState(){
		return true;
	}
	override public void updateHat(){
		//player.GetComponent<Rigidbody2D> ().velocity = speed * direction;
	}
	override public void startHat(){
		tr_animation = transform.parent.Find ("animation");
	}
}
