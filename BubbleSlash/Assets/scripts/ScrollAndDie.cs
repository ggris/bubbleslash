using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollAndDie : MonoBehaviour {

	float t_;
	public Color color_ = new Color (1, 0, 1, 1);

	// Use this for initialization
	void Start () {
		t_ = Time.time;
		color_ = GetComponent<Text> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		float x = Time.time - t_;
		if (x <= 0.2)
			GetComponent<RectTransform> ().localScale *= 1.1f;
		else
			GetComponent<RectTransform> ().localScale += new Vector3(1, 1, 1)*0.01f;
		color_.a *= 0.95f;
		GetComponent<Text> () .color = color_;
	
	}
}
