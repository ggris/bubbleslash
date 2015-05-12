using UnityEngine;
using System.Collections;

public class PlayerAttack : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
		animator.gameObject.transform.Find ("weapon/translation").gameObject.SetActive (true);

	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<Rigidbody2D> ().velocity = animator.gameObject.GetComponent<PlayerPhysics> ().direction_action * animator.gameObject.GetComponent<PlayerPhysics> ().dash_speed;
	}
	

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 5;
		animator.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		animator.gameObject.transform.Find ("weapon/translation").gameObject.SetActive (false);
	}
}
