using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public Text value_;
	static float score_ = 0;
	static float power_ = 1;

	// Use this for initialization
	void Start () {
		score_ = 0;
	}
	
	// Update is called once per frame
	void Update () {
		value_.text = Mathf.Floor(score_).ToString();
	}

	public static void kill() {
		score_ += 42 * power_;
		power_ *= 1.01f;
	}

	public static void hurt() {
		score_ += 13 * power_;
		power_ += 0.4f;
	}

	public static void hit() {
		score_ -= 17;
		if (score_<0) score_ = 0;
		power_ /= 2;
	}

	public static void die() {
		score_ *= 0.92f;
		power_ = 1;
	}

}
