﻿using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {

	public BoxController controller;

	void Start () {
	}
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "floor")
			controller.SendMessage (this.name + "Enter");
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "floor")
			controller.SendMessage (this.name + "Stay");
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "floor")
			controller.SendMessage (this.name + "Exit");
	}
}
