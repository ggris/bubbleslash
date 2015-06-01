using UnityEngine;
using System.Collections;

public class TestHat : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<HatAbstractClass>().onSpecialStateEnter();
	}


	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<HatAbstractClass>().onSpecialStateExit();
	}


}
