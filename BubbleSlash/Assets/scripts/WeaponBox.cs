using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour {
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = this.transform.parent.parent.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter2D (Collider2D other){
		if (player.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("attack")) {
			GameObject obj = other.gameObject;
			if (obj.name == "body") {
				obj = obj.transform.parent.parent.gameObject;
				if (obj.GetComponent<PlayerPhysics> ().playerNumber != player.GetComponent<PlayerPhysics> ().playerNumber) {
					Animator e_anim = obj.GetComponent<Animator> ();
					if (e_anim.GetCurrentAnimatorStateInfo (0).IsName ("attack") || e_anim.GetCurrentAnimatorStateInfo (0).IsName ("delayAttack")) {
						Vector2 direction = (obj.transform.position - player.transform.position).normalized;
						obj.GetComponent<PlayerPhysics> ().isParried (direction);
						player.GetComponent<PlayerPhysics> ().isParried (-direction);
					} else {
						obj.GetComponent<PlayerPhysics> ().isHit ();
						Debug.Log (obj.name + " has been killed");
					}
				}
			}
			if (obj.tag == "weapon") {
				/*
			obj = obj.transform.parent.parent.parent.gameObject;
			Vector2 direction = (obj.transform.position-player.transform.position).normalized;
			obj.GetComponent<PlayerPhysics>().isParried(direction);
			player.GetComponent<PlayerPhysics>().isParried(-direction);
			*/
			}
		}
	}
}
