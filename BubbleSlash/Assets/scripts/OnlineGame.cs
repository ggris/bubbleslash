using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlineGame : MonoBehaviour
{

	enum GameStatus {
		MainMenu,
		Settings,
		InGame
	}

	public static OnlineGame unique_game_;

	public GameObject player_manager_prefab_;
	public GameObject player_prefab_;
	public const string typeName_ = "GGVLBubbleSlashDEV";
	public string gameName_ = "kaboom";
	public string level_ = "OnlineTest";
	//private GameObject player_manager_;
	private HostData[] hostList;
	public menuScript menu;
	public GameObject esc_menu_;
	GameStatus game_status_;
	public NetworkView nview_;

	void Awake ()
	{      
		if (!unique_game_) {
			unique_game_ = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (unique_game_.gameObject);
			unique_game_ = this;
			DontDestroyOnLoad (gameObject);
		}
		game_status_ = GameStatus.MainMenu;
		
		DontDestroyOnLoad (esc_menu_.gameObject);
	}
	
	// Use this for initialization
	void Start ()
	{
		hostList = new HostData[0];
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKey (KeyCode.Escape) && game_status_ == GameStatus.InGame)
			esc_menu_.SetActive (true);
	}

	public static void disconnect()
	{
		foreach(GameObject gobj in GameObject.FindGameObjectsWithTag ("Player")) {
			Destroy(gobj);
		}
		Application.LoadLevel ("Launcher");
	}
	
	void LaunchServer ()
	{
		//Network.incomingPassword = "BubbleSlash";
		bool useNat = !Network.HavePublicAddress ();
		Network.InitializeServer (32, 25000, useNat);
		MasterServer.RegisterHost (typeName_, gameName_);
	}

	public void PlayOnline() {
		RefreshHostList ();
	}
	
	void OnServerInitialized ()
	{
		Debug.Log ("Server Initializied");
		//player_manager_ = Network.Instantiate (player_manager_prefab_, new Vector3 (), new Quaternion (), 0) as GameObject;
		menu.goToSettings ();
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
		if (game_status_ == GameStatus.MainMenu) {
			if (hostList.Length > 0) {
				game_status_ = GameStatus.Settings;
				JoinServer (hostList [0]);
			} else {
				game_status_ = GameStatus.Settings;
				LaunchServer ();
			}
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
		//player_manager_ = GameObject.FindGameObjectWithTag ("player manager");
		//Debug.Log (player_manager_);
		Debug.Log ("Server Joined : " + gameName_);
		menu.goToSettings();
	}

	void OnPlayerConnected (NetworkPlayer player)
	{
		if (game_status_ == GameStatus.InGame)
			nview_.RPC ("setLevelRPC", player, level_);
	}

	[RPC]
	void setLevelRPC(string level) {
		level_ = level;
		menu.enableStartButtonRPC();
	}
	
	public void loadLevel ()
	{
		if (Network.isServer) {
			nview_.RPC("setLevelRPC", RPCMode.OthersBuffered, level_);
		}
		InitPlayers ();
		Application.LoadLevel (level_);
		game_status_ = GameStatus.InGame;
		//		player_manager_.GetComponent<PlayerManager> ().activatePlayers ();
	}

	void InitPlayers ()
	{

		PlayerFactory [] pfs = GameObject.FindObjectsOfType (typeof(PlayerFactory)) as PlayerFactory[];
		Debug.Log (pfs.Length + " players created");

		if (Network.isClient || Network.isServer) {
			foreach (PlayerFactory pf in pfs) {
				pf.createNetworkPlayer ();
			}


		} else {
			foreach (PlayerFactory pf in pfs) {
				pf.createPlayer ();
			}
		}
	}

}
