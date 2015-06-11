using UnityEngine;
using System.Collections;

public class Hurt : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameObject ennemy = animator.gameObject.GetComponent<WeaponBehaviour> ().ennemy;
		GameObject player = animator.gameObject.GetComponent<WeaponBehaviour> ().player;
		
		Vector2 bloodspeed = ennemy.GetComponent<PlayerPhysics> ().direction_action_*2;
	

		//GameObject.Find("bloodManager").GetComponent<BloodPop>().displayBlood(player.transform.position,bloodspeed);
		animator.gameObject.GetComponent<WeaponBehaviour> ().player.GetComponent<PlayerPhysics> ().isHurt (ennemy);

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
