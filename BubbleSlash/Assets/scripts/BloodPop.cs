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
			displayBlood (transform.position, new Vector2(6*Random.value-3,6*Random.value-3));
		}
	
	}
	public void displayBlood(Vector3 pos, Vector2 speed){
		GameObject blood = Instantiate (blood_, pos, new Quaternion (0, 0, 0, 0)) as GameObject;
		blood.GetComponent<FluidSim> ().speed = speed;
		blood.GetComponent<FluidSim> ().Invoke ("source", 0);
	}
}
