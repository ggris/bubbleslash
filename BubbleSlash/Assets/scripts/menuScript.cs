using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class menuScript : MonoBehaviour {
	//canvas
	public GameObject main_canvas;
	public GameObject options_canvas;
	public GameObject settings_canvas;
	public GameObject playerFactoryMenuPrefab;
	public GameObject playerFactoryPrefab;
	private List<GameObject> playerFactoryMenus;
	/*
	enum menu {main,options,settings};
	private menu current_menu;
	*/
	public OnlineGame online_game;

	void Awake(){
		//current_menu = menu.main;
		playerFactoryMenus = new List<GameObject> ();
	}

	void Start () {

	}

	void Update () {
	
	}
	void goToSettings(){
		settings_canvas.SetActive (true);
		main_canvas.SetActive (false);
		options_canvas.SetActive (false);
		Debug.Log ("go to settings");
	}
	
	void goToOptions(){
		settings_canvas.SetActive (false);
		main_canvas.SetActive (false);
		options_canvas.SetActive (true);
	}
	
	void goToMain(){
		settings_canvas.SetActive (false);
		main_canvas.SetActive (true);
		options_canvas.SetActive (false);
	}

	void startGame(){

		online_game.SendMessage ("loadLevel");
	}

	void addPlayer(){
		GameObject new_pfm = GameObject.Instantiate(playerFactoryMenuPrefab) as GameObject;
		RectTransform rect_tr = new_pfm.GetComponent<RectTransform> ();
		rect_tr.SetParent (GameObject.Find ("Settings").GetComponent<RectTransform> ());
		new_pfm.transform.localScale = new Vector3 (1, 1, 1);
		new_pfm.GetComponent<RectTransform> ().position = new Vector3 (0, 100 - 80*playerFactoryMenus.Count, 0);
		new_pfm.GetComponent<PlayerFactoryMenuScript> ().setInputNumber (playerFactoryMenus.Count + 1);
		playerFactoryMenus.Add (new_pfm);

		//GameObject player_factory = GameObject.Instantiate(playerFactoryPrefab) as GameObject;
		//online_game.addFactory (player_factory.GetComponent<PlayerFactory> ());
	}
	void removePlayer(){
		GameObject pfm = playerFactoryMenus [playerFactoryMenus.Count - 1];
		playerFactoryMenus.Remove (pfm);
		GameObject.Destroy (pfm);

	}


}
