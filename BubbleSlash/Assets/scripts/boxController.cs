using UnityEngine;
using System.Collections;

public class boxController : MonoBehaviour {

	public playerPhysics physics;
	
	void feetstay(){
		Debug.Log ("feet stay");
		physics.jumpsLeft = physics.maxJumps;
		physics.isGrounded = true;
	}

	void feetenter(){
	}

	void feetexit(){
		Debug.Log ("feet exit");
		if (physics.isGrounded) {
			physics.jumpsLeft--;
		}
		physics.isGrounded=false;
	}

	void leftstay(){
	}
	void leftenter(){
	}
	void leftexit(){
	}
	void rightstay(){
	}
	void rightenter(){
	}
	void rightexit(){
	}


}
