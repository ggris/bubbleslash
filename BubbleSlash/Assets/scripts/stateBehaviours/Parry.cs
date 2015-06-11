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
		Vector2 playerProj = - playerToEnnemy.normalized * ennemy.GetComponent<PlayerPhysics> ().parry_speed;
		Vector2 ennemyProj = playerToEnnemy.normalized * player.GetComponent<PlayerPhysics> ().parry_speed;
		float scalar = Vector2.Dot (ennemy.GetComponent<PlayerPhysics> ().direction_action_, player.GetComponent<PlayerPhysics> ().direction_action_);
		if (scalar < 0) {
			GameObject.FindObjectOfType<ShockWave> ().pop ((player.transform.position+ennemy.transform.position)/2);
			animator.gameObject.GetComponent<WeaponBehaviour> ().ennemy.GetComponent<PlayerPhysics> ().isParried (ennemyProj);
			animator.gameObject.GetComponent<WeaponBehaviour> ().player.GetComponent<PlayerPhysics> ().isParried (playerProj);
		} else {
			animator.SetTrigger("hurt");
		}
	}
}
