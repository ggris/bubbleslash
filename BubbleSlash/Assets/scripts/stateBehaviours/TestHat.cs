﻿using UnityEngine;
using System.Collections;

public class TestHat : StateMachineBehaviour {

	 //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
		animator.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(0,0);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 5;
		animator.gameObject.transform.parent.gameObject.GetComponent<Animator> ().SetTrigger ("stopHat");
	}


}
