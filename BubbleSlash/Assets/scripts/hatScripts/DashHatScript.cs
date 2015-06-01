using UnityEngine;
using System.Collections;

public class DashHatScript : HatAbstractClass {
	public float speed = 50f;
	private Vector2 direction;

	override public void onSpecialStateEnter(){
		timer = 0f;
		direction = player.GetComponent<PlayerPhysics> ().directionFromInput ();
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;

		player.GetComponent<Rigidbody2D> ().velocity = speed * direction;
	}

	override public void onSpecialStateExit(){
		player.GetComponent<Rigidbody2D> ().gravityScale = 5;
		player.GetComponent<Animator> ().SetTrigger ("stopHat");
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
	}
	override public bool hasSpecialState(){
		return true;
	}
	override public void updateHat(){
		//player.GetComponent<Rigidbody2D> ().velocity = speed * direction;
	}
}
