using UnityEngine;
using System.Collections;

public class OnlineGame : MonoBehaviour
{

	public const string typeName_ = "GGVLBubbleSlash";
	public string gameName_ = "kaboom";
	private HostData[] hostList;

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
		Network.incomingPassword = "BubbleSlash";
		bool useNat = !Network.HavePublicAddress ();
		Network.InitializeServer (32, 25000, useNat);
		MasterServer.RegisterHost (typeName_, gameName_);
	}

	void OnServerInitialized ()
	{
		Debug.Log ("Server Initializied");
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
		Debug.Log ("Server Joined2 : " + gameName_);
	}

}
