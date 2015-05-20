using UnityEngine;
using System.Collections;

public class Parry : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		GameObject ennemy = animator.gameObject.GetComponent<WeaponBehaviour> ().ennemy;
		GameObject player = animator.gameObject.GetComponent<WeaponBehaviour> ().player;

		Vector2 playerToEnnemy = ennemy.transform.position 
								- player.transform.position;
		playerToEnnemy.Normalize ();

		GameObject.FindObjectOfType<ShockWave> ().pop ((player.transform.position+ennemy.transform.position)/2);
		Vector2 playerProj = - playerToEnnemy.normalized * ennemy.GetComponent<PlayerPhysics> ().parry_speed;
		Vector2 ennemyProj = playerToEnnemy.normalized * player.GetComponent<PlayerPhysics> ().parry_speed;
		//Debug.Log (ennemy.name + " got parried by " + player.name + " at speed : " + playerProj);
		animator.gameObject.GetComponent<WeaponBehaviour> ().ennemy.GetComponent<PlayerPhysics> ().isParried (ennemyProj);
		animator.gameObject.GetComponent<WeaponBehaviour> ().player.GetComponent<PlayerPhysics> ().isParried (playerProj);
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
