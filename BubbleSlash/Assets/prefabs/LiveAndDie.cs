using UnityEngine;
using System.Collections;

public class LiveAndDie : MonoBehaviour {

	public float lifetime_ = 1.0f;

	private float time_;

	// Use this for initialization
	void Start () {
		time_ = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - time_ > lifetime_)
			Destroy (this.gameObject);
	
	}
}
