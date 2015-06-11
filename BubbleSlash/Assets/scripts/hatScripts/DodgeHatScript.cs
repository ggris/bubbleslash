using UnityEngine;
using System.Collections;

public class DodgeHatScript : HatAbstractClass {
	public float dodge_duration = 0.3f;
	public float speed = 5;
	
	
	override public void onSpecialStateEnter(){
		timer = 0f;
		setDodge ();
		Invoke ("setFall", dodge_duration);



	}
	void setDodge(){
		Vector2 direction = player.GetComponent<PlayerPhysics> ().getInputDirection ();
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;
		player.GetComponent<Rigidbody2D> ().velocity = direction*speed;
		player.GetComponent<PlayerPhysics> ().is_hitable = false;
	}

	void setFall(){
		player.GetComponent<Rigidbody2D> ().gravityScale = 2;
		player.GetComponent<Rigidbody2D> ().velocity *= 0.5f;
		player.GetComponent<PlayerPhysics> ().is_hitable = true;
	}


	override public void onSpecialStateExit(){
		player.GetComponent<Rigidbody2D> ().gravityScale = 5;
		player.GetComponent<Animator> ().SetTrigger ("stopHat");
		//player.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
		//player.GetComponent<PlayerPhysics> ().is_hitable = true;
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