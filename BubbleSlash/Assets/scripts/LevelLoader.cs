using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	string level_;

	// Use this for initialization
	void Start () {
		level_ = "TestLevel03";
	
	}
	
	// Update is called once per frame
	void Update () {
			Application.LoadLevel(level_);
	
	}

}
