using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {

	public PlayerPhysics physics;
	
	void feetStay(){
		physics.is_grounded = true;
	}

	void feetEnter(){
	}

	void feetExit(){
		physics.is_grounded=false;
	}

	void leftStay(){
		physics.is_touching_left = true;
	}
	void leftEnter(){
	}
	void leftExit(){
		physics.is_touching_left = false;
	}
	void rightStay(){
		physics.is_touching_right = true;
	}
	void rightEnter(){
	}
	void rightExit(){
		physics.is_touching_right = false;
	}


}
