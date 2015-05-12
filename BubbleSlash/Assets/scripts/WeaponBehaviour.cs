using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour {
	public Vector2 ennemyDirection;
	public GameObject ennemy;
	public GameObject player;
	// Use this for initialization
	void Start () {
		player = gameObject.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void getHit(GameObject newEnnemy) {
		ennemy = newEnnemy;
		GetComponent<Animator> ().SetTrigger ("hit");
	}
}
