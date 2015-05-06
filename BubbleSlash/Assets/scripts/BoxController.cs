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
	}
	void leftEnter(){
	}
	void leftExit(){
	}
	void rightStay(){
	}
	void rightEnter(){
	}
	void rightExit(){
	}


}
