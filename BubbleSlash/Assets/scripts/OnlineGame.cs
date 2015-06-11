using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OnlineGame : MonoBehaviour
{

	//private List<PlayerFactory> player_factories;

	//public PlayerFactory player1_factory_;
	//public PlayerFactory player2_factory_;
	public GameObject player_manager_prefab_;
	public GameObject player_prefab_;
	public const string typeName_ = "GGVLBubbleSlashDEV";
	public string gameName_ = "kaboom";
	public string level_ = "OnlineTest";
	private GameObject player_manager_;
	private HostData[] hostList;
	public menuScript menu;
	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Use this for initialization
	void Start ()
	{
		hostList = new HostData[0];
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void LaunchServer ()
	{
		//Network.incomingPassword = "BubbleSlash";
		bool useNat = !Network.HavePublicAddress ();
		Network.InitializeServer (32, 25000, useNat);
		MasterServer.RegisterHost (typeName_, gameName_);
	}
	
	void OnServerInitialized ()
	{
		Debug.Log ("Server Initializied");
		player_manager_ = Network.Instantiate (player_manager_prefab_, new Vector3 (), new Quaternion (), 0) as GameObject;
		menu.SendMessage ("goToSettings");
	}
	
	void ConnectRandomServer ()
	{
		RefreshHostList ();
	}
	
	private void RefreshHostList ()
	{
		MasterServer.RequestHostList (typeName_);
	}
	
	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
		else
			Debug.Log (msEvent);
		if (hostList.Length > 0) {
			JoinServer (hostList [0]);
		} else {
			Debug.Log("No host");
		}
	}
	
	private void JoinServer (HostData hostData)
	{
		Debug.Log ("Joining : " + gameName_);
		Network.Connect (hostData);
		gameName_ = hostData.gameName;
	}
	
	void OnConnectedToServer ()
	{
		player_manager_ = GameObject.FindGameObjectWithTag ("player manager");
		Debug.Log (player_manager_);
		Debug.Log ("Server Joined : " + gameName_);
		menu.SendMessage ("goToSettings");
	}
	
	void loadLevel ()
	{
		InitPlayers ();
		Application.LoadLevel (level_);
//		player_manager_.GetComponent<PlayerManager> ().activatePlayers ();
	}

	void InitPlayers ()
	{

		PlayerFactory [] pfs = GameObject.FindObjectsOfType(typeof(PlayerFactory)) as PlayerFactory[];
		Debug.Log (pfs.Length + " players created");

		if (Network.isClient || Network.isServer) {
			foreach (PlayerFactory pf in pfs) {
				pf.createNetworkPlayer();
			}

		} else {
			foreach (PlayerFactory pf in pfs) {
				pf.createPlayer();
			}
		}
	}

}
