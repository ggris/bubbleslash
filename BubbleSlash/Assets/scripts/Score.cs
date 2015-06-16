using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		text.color = HSVToRGB (value, 1, 1);
		GameObject.Instantiate (message_).transform.SetParent (scorePanel_.transform, false);
	}

	public static Color HSVToRGB(float H, float S, float V)
	{
		if (S == 0f)
			return new Color(V,V,V);
		else if (V == 0f)
			return Color.black;
		else
		{
			Color col = Color.black;
			float Hval = H * 6f;
			int sel = Mathf.FloorToInt(Hval);
			float mod = Hval - sel;
			float v1 = V * (1f - S);
			float v2 = V * (1f - S * mod);
			float v3 = V * (1f - S * (1f - mod));
			switch (sel + 1)
			{
			case 0:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 1:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			case 2:
				col.r = v2;
				col.g = V;
				col.b = v1;
				break;
			case 3:
				col.r = v1;
				col.g = V;
				col.b = v3;
				break;
			case 4:
				col.r = v1;
				col.g = v2;
				col.b = V;
				break;
			case 5:
				col.r = v3;
				col.g = v1;
				col.b = V;
				break;
			case 6:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 7:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			}
			col.r = Mathf.Clamp(col.r, 0f, 1f);
			col.g = Mathf.Clamp(col.g, 0f, 1f);
			col.b = Mathf.Clamp(col.b, 0f, 1f);
			return col;
		}
	}

}
