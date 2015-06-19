using UnityEngine;
using System.Collections;

public class EscMenu : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void quit ()
	{
		Debug.Log ("Quit");
		Application.Quit ();
		mainMenu ();
	}

	public static void mainMenu ()
	{
		if (Network.isServer) {
			Network.Disconnect ();
			MasterServer.UnregisterHost ();
		}
		if (Network.isClient) {
			Network.Disconnect ();
		}
		gameObject.SetActive (false);
		OnlineGame.disconnect ();
	}
}
