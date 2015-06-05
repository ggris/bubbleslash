using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Box : MonoBehaviour {

	public BoxController controller;
	private List<Collider2D> colls;
	void Start () {
		colls = new List<Collider2D> ();
	}
	void Update () {

	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "floor"){
			if (colls.Count==0){
				controller.SendMessage (this.name + "Enter");
			}
			colls.Add (other);
		}
	}
	void OnTriggerStay2D(Collider2D other){
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "floor") {
			colls.Remove(other);
			if(colls.Count==0)
				controller.SendMessage (this.name + "Exit");
		}
	}
}
