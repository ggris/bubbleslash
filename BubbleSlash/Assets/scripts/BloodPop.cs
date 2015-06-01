using UnityEngine;
using System.Collections;

public class BloodPop : MonoBehaviour {

	public GameObject blood_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("o")) {
			//For test purposes, to work on blood rendering easily
			displayBlood (transform.position, new Vector2(3,0));
		}
	
	}
	public void displayBlood(Vector3 pos, Vector2 speed){
		GameObject blood = Instantiate (blood_, pos + new Vector3(0, 0, -6), new Quaternion (0, 0, 0, 0)) as GameObject;
		blood.GetComponent<FluidSim> ().initial_velocity_ = speed;
		blood.GetComponent<FluidSim> ().Invoke ("source", 0);
	}
}
