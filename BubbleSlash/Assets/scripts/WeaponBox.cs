using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour {
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = this.transform.parent.parent.parent.gameObject;

	}

	void OnTriggerEnter2D (Collider2D other){

		GameObject objectHit = other.gameObject;
		if (objectHit.name == "body") {
			GameObject ennemyHit = objectHit.transform.parent.parent.gameObject;
			if (ennemyHit.GetComponent<PlayerPhysics>().playerNumber != player.GetComponent<PlayerPhysics>().playerNumber
			    && ennemyHit.GetComponent<PlayerPhysics>().is_hitable){
				ennemyHit.transform.Find("weapon").GetComponent<WeaponBehaviour>().getHit(player);
			}
		}
	}
}
