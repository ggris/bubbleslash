using UnityEngine;
using System.Collections;

public class AttackSound : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<AudioSource> ().pitch = Random.Range (0.4f, 1.6f);
		animator.gameObject.GetComponent<AudioSource> ().Play ();
	}


}
