using UnityEngine;
using System.Collections;

public class OnlineGame : MonoBehaviour
{

	public const string typeName_ = "GGVLBubbleSlash";
	public const string gameName_ = "kaboom";
	private HostData[] hostList;

	// Use this for initialization
	void Start ()
	{
	
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

	private void RefreshHostList ()
	{
		MasterServer.RequestHostList (typeName_);
	}
	
	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
		Debug.Log (hostList);
	}

}
