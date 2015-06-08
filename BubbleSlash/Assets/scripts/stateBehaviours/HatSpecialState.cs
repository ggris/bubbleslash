using UnityEngine;
using System.Collections;

public class HatSpecialState : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<PlayerPhysics> ().hat_GO.gameObject.GetComponent<Animator> ().SetTrigger ("input");
		//animator.gameObject.transform.Find (animator.gameObject.GetComponent<PlayerPhysics>().hat_name).GetComponent<Animator> ().SetTrigger ("input");
	}


}
