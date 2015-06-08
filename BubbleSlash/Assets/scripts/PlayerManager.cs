using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
	public HashSet<GameObject> players_ = new HashSet<GameObject> ();
	
	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	[RPC]
	public void addPlayer(GameObject player)
	{
		players_.Add (player);
	}


	public HashSet<GameObject> getPlayers ()
	{
		return players_;
	}

	public void activatePlayers() {
		foreach (GameObject player in players_) 
			player.SetActive (true);
	}

}
