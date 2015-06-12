﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerFactoryMenuScript : MonoBehaviour {
	private GameObject player_factory;
	public GameObject pf_prefab;
	public Color[] colors;

	//settings
	private int color_indice = 0;
	private Color color;
	private PlayerSettings.Hat hat = PlayerSettings.Hat.dashHat;
	//private int input_number;

	//rendering on buttons
	UnityEngine.UI.Text text_hat;
	UnityEngine.UI.Image button_image;	
	void Awake(){
		color_indice = 0;
		player_factory = Instantiate (pf_prefab);
	}

	void OnDestroy(){
		GameObject.Destroy (player_factory);
	}
	void Start () {
		button_image = gameObject.transform.Find ("color").Find("image").gameObject.GetComponent<UnityEngine.UI.Image> ();
		text_hat = gameObject.transform.Find ("hat").Find ("Text").GetComponent<UnityEngine.UI.Text> ();
		player_factory.GetComponent<PlayerFactory> ().color = Color.white;
		player_factory.GetComponent<PlayerFactory> ().hat = hat;
		text_hat.text = PlayerSettings.ToString (hat);
	}
	
	// Update is called once per frame
	void Update () {
		 
	}
	void changeHat(){
		hat = PlayerSettings.nextHat (hat);
		player_factory.GetComponent<PlayerFactory> ().hat = hat;
		text_hat.text = PlayerSettings.ToString (hat);
	}
	void changeColor(){
		color_indice = (++color_indice) % colors.Length;
		color = colors [color_indice];
		player_factory.GetComponent<PlayerFactory> ().color = color;
		color *= new Color (0.69f, 0.75f, 0.85f);
		button_image.color = color;
	}


	public void setInputNumber(int i){
		player_factory.GetComponent<PlayerFactory> ().input_number = i;
	}

}
