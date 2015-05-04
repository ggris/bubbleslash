using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {

	public PlayerPhysics physics;
	
	void feetStay(){
		physics.jumps_left = physics.max_jumps;
		physics.is_grounded = true;
	}

	void feetEnter(){
	}

	void feetExit(){
		if (physics.is_grounded) {
			physics.jumps_left--;
		}
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
