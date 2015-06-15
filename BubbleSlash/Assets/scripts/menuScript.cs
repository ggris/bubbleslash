using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class menuScript : MonoBehaviour {
	//canvas
	public GameObject main_canvas;
	public GameObject settings_canvas;
	public GameObject maps_canvas;
	public GameObject playerFactoryMenuPrefab;
	public GameObject playerFactoryPrefab;
	private List<GameObject> playerFactoryMenus;

	public Sprite[] map_previews;
	private int map_preview_index = 0;
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
		maps_canvas.SetActive (false);
		settings_canvas.SetActive (true);

		addPlayer ();
		Debug.Log ("go to settings");
	}

	void goToMaps(){
		settings_canvas.SetActive (false);
		main_canvas.SetActive (false);
		maps_canvas.SetActive (true);
	}
	
	void goToMain(){
		maps_canvas.SetActive (false);
		settings_canvas.SetActive (false);
		main_canvas.SetActive (true);

	}

	void startGame(){
		online_game.level_ = sceneName (map_preview_index);
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

	void mapRight(){
		map_preview_index = (++map_preview_index) % map_previews.Length;
		GameObject.Find ("MapPreview").GetComponent<Image> ().sprite = map_previews [map_preview_index];
	}
	void mapLeft(){
		map_preview_index = (--map_preview_index);
		if (map_preview_index < 0)
			map_preview_index = map_previews.Length - 1;
		GameObject.Find ("MapPreview").GetComponent<Image> ().sprite = map_previews [map_preview_index];
	}
	string sceneName(int map_index){
		switch (map_index) {
		case 0:
			return ("level0");
		case 1 :
			return ("level1");
		case 2 :
			return ("TestLevel03");
		default :
			return("");

		}
	}

}
