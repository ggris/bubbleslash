using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
	private GameObject[] players_;
	
	// Use this for initialization
	void Start ()
	{
		refreshPlayers ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void refreshPlayers ()
	{
		players_ = GameObject.FindGameObjectsWithTag ("Player");
	}

	GameObject[] getPlayers ()
	{
		return players_;
	}

}
