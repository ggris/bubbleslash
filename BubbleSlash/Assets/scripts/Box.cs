using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : MonoBehaviour {

	public BoxController controller;
	//private List<Collider2D> colls;
	void Start () {
		//colls = new List<Collider2D> ();
	}
	void Update () {

	}
	void OnTriggerEnter2D(Collider2D other){
		//if (!controller.isIgnored (other)) {
		if (other.gameObject.tag == "obstacle" || other.gameObject.tag == "semiobstacle") {

					controller.enter (GetComponent<Collider2D>(), other);

			}
		//}
	}
	void OnTriggerStay2D(Collider2D other){
		//if (!controller.isIgnored (other)) {
		if (other.gameObject.tag == "obstacle" || other.gameObject.tag == "semiobstacle"){
				controller.stay (GetComponent<Collider2D>(), other);
			}
		//}
	}
	void OnTriggerExit2D(Collider2D other){
		//if (!controller.isIgnored (other)) {
		if (other.gameObject.tag == "obstacle" || other.gameObject.tag == "semiobstacle") {

				controller.exit (GetComponent<Collider2D>(), other);

			}
		//}
	}
}
