using UnityEngine;
using System.Collections;

public class WeaponBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D (Collider2D other){
		GameObject obj=other.gameObject;
		if (obj.name=="body") {
			obj = obj.transform.parent.parent.gameObject;
			obj.GetComponent<PlayerPhysics>().isHit();
			Debug.Log (obj.name + " has been killed");
		}
	}
}
