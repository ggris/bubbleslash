using UnityEngine;
using System.Collections;

public class TestHatScript : HatAbstractClass {

	override public void onSpecialStateEnter(){
		timer = 0;
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
	}
	override public void onSpecialStateExit(){
		player.GetComponent<Rigidbody2D> ().gravityScale = 5;
		player.GetComponent<Animator> ().SetTrigger ("stopHat");
	}
	override public bool hasSpecialState(){
		return true;
	}

}
