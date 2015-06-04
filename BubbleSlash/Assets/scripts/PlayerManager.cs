using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

	public GameObject[] hat_prefabs;
	public GameObject player_prefab;
	public PlayerSettings.Hat[] hatChoices;
	public Transform[] spawn_points;
	public GameObject[] players_;
	public float min_distance_spawn;
	public float death_altitude;
	private int[] score;
	private CameraTracking camera_tracking;

	void Start ()
	{
		players_ = new GameObject[2];
		score = new int[players_.Length];
		camera_tracking = (CameraTracking)FindObjectOfType (typeof(CameraTracking));
		if (Network.isServer)
			StartOnlineServer ();
		else if (Network.isClient)
			StartOnlineClient ();
		else
			StartOffline ();
	}

	void StartOffline ()
	{
		spawn (0, spawn_points [0]);
		spawn (1, spawn_points [1]);
	}

	void StartOnlineServer ()
	{
		Debug.Log ("StartOnlineServer");
		spawn (0, spawn_points [0]);
	}
	
	void StartOnlineClient ()
	{
		Debug.Log ("StartOnlineClient");
		spawn (1, spawn_points [1]);
	}

	void Update ()
	{
	}

	public void dealWithDeath (int i)
	{
		score [i] -= 1;
		Destroy (players_ [i]);
		StartCoroutine (spawnLater (i, players_ [i].transform.position, 0.5f));

	}

	public IEnumerator spawnLater (int i, Vector3 position_death, float delayTime)
	{
		yield return new WaitForSeconds (delayTime);
		Transform t = findRandomSpawnPoint (i, position_death);
		spawn (i, t);
	}

	public void spawn (int i, Transform t)
	{
		if (!Network.isClient && !Network.isServer) 
			spawnOffline (i, t);
		else
			spawnOnline (i, t, 0);
	}
	
	public void spawnOffline (int i, Transform t)
	{
		players_ [i] = GameObject.Instantiate (player_prefab, t.position, new Quaternion (0, 0, 0, 0)) as GameObject;
		players_ [i].GetComponent<PlayerPhysics> ().playerNumber = i + 1;
		players_ [i].gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		SetSprites (i);
		SetColor (i);
		SetHatRendering (i);
		GameObject hat_i = GameObject.Instantiate (hat_prefabs [(int)hatChoices [i]], new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		hat_i.transform.parent = players_ [i].transform;
		players_ [i].GetComponent<PlayerPhysics> ().hat = hat_i;
		camera_tracking.resetPlayers ();
	}

	public void spawnOnline (int i, Transform t, int group)
	{
		players_ [i] = Network.Instantiate (player_prefab, t.position, new Quaternion (0, 0, 0, 0), group) as GameObject;
		players_ [i].GetComponent<PlayerPhysics> ().playerNumber = i + 1;
		players_ [i].gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		SetSprites (i);
		SetColor (i);
		SetHatRendering (i);
		GameObject hat_i = Network.Instantiate (hat_prefabs [(int)hatChoices [i]], new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0), group) as GameObject;
		hat_i.transform.parent = players_ [i].transform;
		players_ [i].GetComponent<PlayerPhysics> ().hat = hat_i;
		camera_tracking.resetPlayers ();
		Debug.Log ("Spawn online");
	}

	Transform findRandomSpawnPoint (int i, Vector3 pos)
	{
		List<Transform> far_enough_spawn_points = new List<Transform> ();
		foreach (Transform t in spawn_points) {
			if ((t.position - pos).magnitude > min_distance_spawn)
				far_enough_spawn_points.Add (t);
		}
		if (far_enough_spawn_points.Count > 0) 
			return far_enough_spawn_points [Random.Range (0, far_enough_spawn_points.Count - 1)];
		else 
			return spawn_points [Random.Range (0, spawn_points.Length - 1)];
	}

	void SetSprites (int i)
	{
		Object [] sprites = Resources.LoadAll<Sprite> ("sprites/playerSpriteSheet_" + "blue");
		Object weapon = Resources.Load<Sprite> ("sprites/playerSpriteSheet_sword");
		players_ [i].transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [0];
		players_ [i].transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [1];
		players_ [i].transform.Find ("animation").Find ("weapon_trans").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [2];
		players_ [i].transform.Find ("animation").Find ("weapon_trans").Find ("weapon").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [3];
	}

	public Color[] colors;

	void SetColor (int i)
	{
		players_ [i].transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().color = colors [i];
		players_ [i].transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().color = colors [i];
		players_ [i].transform.Find ("animation").Find ("weapon_trans").gameObject.GetComponent<SpriteRenderer> ().color = colors [i];
	}

	void SetHatRendering (int i)
	{
		PlayerSettings.Hat h = hatChoices [i];
		GameObject anim = players_ [i].transform.Find ("animation").gameObject;
		if (h == PlayerSettings.Hat.dashHat) {
			anim.transform.FindChild ("dodgeHat").gameObject.SetActive (false);
			anim.transform.FindChild ("dashHat").gameObject.SetActive (true);
		}
		if (h == PlayerSettings.Hat.dodgeHat) {
			anim.transform.FindChild ("dashHat").gameObject.SetActive (false);
			anim.transform.FindChild ("dodgeHat").gameObject.SetActive (true);
		}
	}

	void OnGUI ()
	{
		for (int i=0; i <players_.Length; i++) {
			string message = "P" + i + " : " + score [i];
			GUI.Box (new Rect (50 * i, 10, 50, 25), message);
		}
	}
}
