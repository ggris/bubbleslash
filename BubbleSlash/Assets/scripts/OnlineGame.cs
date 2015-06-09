using UnityEngine;
using System.Collections;

public class OnlineGame : MonoBehaviour
{
	public PlayerFactory player1_factory_;
	public PlayerFactory player2_factory_;
	public GameObject player_manager_prefab_;
	public GameObject player_prefab_;
	public const string typeName_ = "GGVLBubbleSlashDEV";
	public string gameName_ = "kaboom";
	public string level_ = "OnlineTest";
	private GameObject player_manager_;
	private HostData[] hostList;

	NetworkView network_view_;
	
	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Use this for initialization
	void Start ()
	{
		hostList = new HostData[0];
		network_view_ = GetComponent<NetworkView> ();
		Debug.Log (network_view_);
		
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
		loadLevel ();
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
		loadLevel ();
		Debug.Log ("Server Joined2 : " + gameName_);
	}

	void loadLevel ()
	{
		InitPlayers ();
		Application.LoadLevel (level_);
		player_manager_.GetComponent<PlayerManager> ().activatePlayers ();
	}

	void InitPlayers ()
	{
		if (Network.isClient || Network.isServer) {
			player1_factory_.createNetworkPlayer ();
		} else {
			player_manager_ = GameObject.Instantiate (player_manager_prefab_, new Vector3 (), new Quaternion ()) as GameObject;
			player1_factory_.createPlayer ();
			player2_factory_.createPlayer ();
		}
	}

}
