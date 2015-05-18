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
			GameObject blood = Instantiate (blood_, transform.position, transform.rotation) as GameObject;
			blood.GetComponent<FluidSim>().speed=new Vector2(6*Random.value-3,6*Random.value-3);
			blood.GetComponent<FluidSim>().Invoke("source",0);
		}
	
	}
}
