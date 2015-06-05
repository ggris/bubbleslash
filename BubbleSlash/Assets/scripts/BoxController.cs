using UnityEngine;
using System.Collections;


public class BoxController : MonoBehaviour {

	public PlayerPhysics physics;
	
	void feetStay(){

	}

	void feetEnter(){
		physics.is_grounded = true;
	}

	void feetExit(){
		physics.is_grounded=false;
	}

	void leftStay(){

	}
	void leftEnter(){
		physics.is_touching_left = true;
	}
	void leftExit(){
		physics.is_touching_left = false;
	}
	void rightStay(){

	}
	void rightEnter(){
		physics.is_touching_right = true;
	}

	void rightExit(){
		physics.is_touching_right = false;
	}


}
