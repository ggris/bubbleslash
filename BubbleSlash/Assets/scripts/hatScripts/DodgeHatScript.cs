using UnityEngine;
using System.Collections;

public class DodgeHatScript : HatAbstractClass {
	

	
	
	override public void onSpecialStateEnter(){
	
		timer = 0f;
		player.GetComponent<Rigidbody2D> ().gravityScale = 0.3f;
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		player.GetComponent<PlayerPhysics> ().is_hitable = false;

	}
	
	override public void onSpecialStateExit(){
		player.GetComponent<Rigidbody2D> ().gravityScale = 5;
		player.GetComponent<Animator> ().SetTrigger ("stopHat");
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		player.GetComponent<PlayerPhysics> ().is_hitable = true;
	}
	override public bool hasSpecialState(){
		return true;
	}

	override public void updateHat(){
	}
	override public void startHat(){
		length = 1f;
	}
}