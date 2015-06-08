using UnityEngine;
using System.Collections;

public class OnlineGame : MonoBehaviour
{
	
	public GameObject player_prefab;
	
	public const string typeName_ = "GGVLBubbleSlash";
	public string gameName_ = "kaboom";
	private HostData[] hostList;
	public string level_ = "OnlineTest";
	
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
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
		Application.LoadLevel(level_);
		SpawnPlayer();
	}
	
	void SpawnPlayer() {
		GameObject player = Network.Instantiate (player_prefab, new Vector3 (), new Quaternion (), 0) as GameObject;
		player.GetComponent<PlayerPhysics> ().playerNumber = 2;
	}
	
	void ConnectRandomServer ()
	{
		RefreshHostList ();
		if (hostList.Length > 0) {
			JoinServer(hostList[0]);
		}
	}
	
	private void RefreshHostList ()
	{
		MasterServer.RequestHostList (typeName_);
	}
	
	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
	}
	
	private void JoinServer (HostData hostData)
	{
		Network.Connect (hostData);
		gameName_ = hostData.gameName;
		Debug.Log ("Server Joined1 : " + gameName_);
	}
	
	void OnConnectedToServer ()
	{
		Application.LoadLevel(level_);
		SpawnPlayer();
		Debug.Log ("Server Joined2 : " + gameName_);
	}

}
