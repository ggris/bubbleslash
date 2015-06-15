using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class menuScript : MonoBehaviour {
	//canvas
	public GameObject main_canvas;
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
		main_canvas.SetActive (false);
		settings_canvas.SetActive (true);

		addPlayer ();
		Debug.Log ("go to settings");
	}
	/*
	void goToOptions(){
		settings_canvas.SetActive (false);
		main_canvas.SetActive (false);

	}*/
	
	void goToMain(){
		settings_canvas.SetActive (false);
		main_canvas.SetActive (true);

	}

	void startGame(){
		online_game.loadLevel ();
		//online_game.SendMessage ("loadLevel");
	}

	void addPlayer(){
		if (playerFactoryMenus.Count < 2) {
			GameObject new_pfm = GameObject.Instantiate (playerFactoryMenuPrefab) as GameObject;
			RectTransform rect_tr = new_pfm.GetComponent<RectTransform> ();
			rect_tr.SetParent (GameObject.Find ("PlayerFactoryMenus").GetComponent<RectTransform> ());
			new_pfm.transform.localScale = new Vector3 (1, 1, 1);
			new_pfm.GetComponent<PlayerFactoryMenuScript> ().setInputNumber (playerFactoryMenus.Count + 1);
			playerFactoryMenus.Add (new_pfm);
			GameObject.Find ("PlayerFactoryMenus").GetComponent<RectTransform> ().offsetMin = new Vector2 (0, 80 + 90 * (4 - playerFactoryMenus.Count));
		}
		//GameObject player_factory = GameObject.Instantiate(playerFactoryPrefab) as GameObject;
		//online_game.addFactory (player_factory.GetComponent<PlayerFactory> ());
	}
	void removePlayer(){
		if (playerFactoryMenus.Count > 0) {
			GameObject pfm = playerFactoryMenus [playerFactoryMenus.Count - 1];
			playerFactoryMenus.Remove (pfm);
			GameObject.Destroy (pfm);
			GameObject.Find ("PlayerFactoryMenus").GetComponent<RectTransform> ().offsetMin = new Vector2 (0, 80 + 90 * (4 - playerFactoryMenus.Count));
		}

	}


}
