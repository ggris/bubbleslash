﻿using UnityEngine;
using System.Collections;

public class PlayerFactory : MonoBehaviour {

	//player setting
	public Vector2 pos = new Vector2 (0,0);

	public int player_number=1;
	PlayerSettings.Hat hat = PlayerSettings.Hat.dashHat;
	//PlayerSettings.Weapon weapon=PlayerSettings.Weapon.sword;
	Color color= Color.white;


	//prefabs
	public GameObject[] hat_prefabs;
	public GameObject player_prefab;

	private PlayerManager player_manager = null;
	
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	public void createPlayer() {
		configurePlayer( GameObject.Instantiate (player_prefab, pos, new Quaternion (0, 0, 0, 0)) as GameObject);
	}

	public void createNetworkPlayer() {
		configurePlayer( Network.Instantiate (player_prefab, pos, new Quaternion (0, 0, 0, 0), 0) as GameObject);
	}

	void configurePlayer(GameObject player) {
		getPlayerManager().addPlayer (player);
		player.SetActive (false);
		player.GetComponent<PlayerPhysics> ().playerNumber = input_number;
		player.GetComponent<PlayerPhysics> ().setHatChoice (hat);
		setSprites (player);
		setColor (player, color);
		setHatRendering (player);
		createHat (player);
		//notify playermanager
	}

	void setSprites (GameObject player)
	{
		Object [] sprites = Resources.LoadAll<Sprite> ("sprites/playerSpriteSheet_" + "blue");
		player.transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [0];
		player.transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [1];
		player.transform.Find ("animation").Find ("weapon_trans").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [2];
		player.transform.Find ("animation").Find ("weapon_trans").Find ("weapon").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [3];
	}

	PlayerManager getPlayerManager() {
		if (player_manager == null)
			player_manager = FindObjectOfType (typeof(PlayerManager)) as PlayerManager;

		return player_manager;
	}
	
	void setColor (GameObject player, Color c)
	{
		player.transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().color = c;
		player.transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().color = c;
		player.transform.Find ("animation").Find ("weapon_trans").gameObject.GetComponent<SpriteRenderer> ().color = c;
	}

	void setHatRendering (GameObject player)
	{
		GameObject anim = player.transform.Find ("animation").gameObject;
		if (hat == PlayerSettings.Hat.dashHat) {
			anim.transform.FindChild ("dodgeHat").gameObject.SetActive (false);
			anim.transform.FindChild ("dashHat").gameObject.SetActive (true);
		}
		if (hat == PlayerSettings.Hat.dodgeHat) {
			anim.transform.FindChild ("dashHat").gameObject.SetActive (false);
			anim.transform.FindChild ("dodgeHat").gameObject.SetActive (true);
		}
	}

	void createHat(GameObject player){
		GameObject hat_ = GameObject.Instantiate (hat_prefabs [(int)hat], new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		hat_.transform.parent = player.transform;
		player.GetComponent<PlayerPhysics> ().hat_GO = hat_;
	}
}
