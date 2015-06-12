using UnityEngine;
using System.Collections;

public class killOnOutOfStage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D (Collider2D other){
		if (other.gameObject.name == "collision boxes") {
			Debug.Log("fall!");
			other.transform.parent.gameObject.GetComponent<PlayerPhysics>().StartCoroutine("die");
		}
	}
}
