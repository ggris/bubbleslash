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

	enum menu {main,options,settings};
	private menu current_menu;

	public GameObject online_game;

	void Awake(){
		current_menu = menu.main;
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
		new_pfm.transform.SetParent(GameObject.Find("Settings").transform);
		new_pfm.transform.position = new Vector3 (0, 100-playerFactoryMenus.Count * 80);
		playerFactoryMenus.Add (new_pfm);
		//GameObject.Instantiate(playerFactoryPrefab) as GameObject;
	}
	void removePlayer(){
		GameObject pfm = playerFactoryMenus [playerFactoryMenus.Count - 1];
		playerFactoryMenus.Remove (pfm);
		GameObject.Destroy (pfm);

	}


}
