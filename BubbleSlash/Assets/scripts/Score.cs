using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class Score : MonoBehaviour
{

	public Text value_;
	public GameObject message_prefab_;
	static GameObject message_;
	static GameObject scorePanel_;
	static float score_ = 0;
	static float power_ = 1;
	static float score_target_ = 0;
	static float score_top_ = 0;
	static int combo_ = 0;
	static float lastKill_;

	// Use this for initialization
	void Start ()
	{
		score_ = 0;
		score_target_ = 0;
		message_ = message_prefab_;
		scorePanel_ = gameObject;
		lastKill_ = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		value_.text = Mathf.Floor (score_).ToString () + " : " + Mathf.Floor (score_top_).ToString ();
		if (score_target_ >= 0.1f)
			score_target_ -= 0.1f;
		score_ *= 0.9f;
		score_ += score_target_ * 0.1f;
		score_top_ = Mathf.Max (score_top_, score_);
	}

	public static void kill ()
	{
		score_target_ += 42 * power_;
		power_ += 1.0f;
		float value = 1 - 0.6f / (combo_ + 1.0f);
		if (Time.time - lastKill_ < 0.2f) {
			score_target_ += 42 * power_;
			announce ("MULTI KILL", value);
		}
		else
			announce ("kill" + ++combo_, value);
		lastKill_ = Time.time;
	}

	public static void hurt ()
	{
		score_target_ += 13 * power_;
		power_ += 0.3f;
		announce ("hurt", 0.4f);
	}

	public static void hit ()
	{
		score_target_ -= 17;
		if (score_target_ < 0)
			score_target_ = 0;
		power_ *= 2;
		announce ("hit", 0.2f);
	}

	public static void die ()
	{
		score_target_ *= 0.5f;
		power_ = 1;
		announce ("you die");
	}

	public static void announce (string message = "yey", float value = 0)
	{
		Text text = message_.GetComponent<Text> ();
		text.text = message;
		text.color = EditorGUIUtility.HSVToRGB (value, 1, 1);
		GameObject.Instantiate (message_).transform.SetParent (scorePanel_.transform, false);
	}

}
