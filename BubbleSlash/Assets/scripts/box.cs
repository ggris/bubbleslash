using UnityEngine;
using System.Collections;

public class box : MonoBehaviour {

	public boxController controller;

	void Start () {
	}
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "floor")
			controller.SendMessage (this.name + "enter");
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "floor")
			controller.SendMessage (this.name + "stay");
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "floor")
			controller.SendMessage (this.name + "exit");
	}
}
