using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public PlayerSettings[] settings;
	public GameObject player_prefab;
	public Transform[] spawn_points;
	public GameObject [] players;
	public float min_distance_spawn;
	public float death_altitude;
	private int [] score;
	private CameraTracking camera_tracking;

	void Start () {
		score = new int[players.Length];
		camera_tracking = (CameraTracking)FindObjectOfType (typeof(CameraTracking));
	}

	void Update () {
	}

	private int indice;
	private Vector3 position_death;

	public void dealWithDeath(int i){
		score [i] -= 1;
		position_death = players [i].transform.position;
		indice = i; //needed for passing arguments to invoked method
		Destroy(players [i]);
		Invoke("spawn",0.5f);
	}

	public void spawn(){
		int i = indice;
		Transform t = findRandomSpawnPoint (i, position_death);
		players [i] = GameObject.Instantiate (player_prefab, t.position, new Quaternion (0, 0, 0, 0)) as GameObject;
		players [i].GetComponent<PlayerPhysics> ().playerNumber = i + 1;
		players [i].gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		SetColor (i);
		camera_tracking.resetPlayers ();
	}

	Transform findRandomSpawnPoint(int i, Vector3 pos){
		List<Transform> far_enough_spawn_points = new List<Transform>();
		foreach (Transform t in spawn_points) {
			if ((t.position-pos).magnitude > min_distance_spawn)
				far_enough_spawn_points.Add(t);
		}
		if (far_enough_spawn_points.Count > 0) 
			return far_enough_spawn_points [Random.Range (0, far_enough_spawn_points.Count - 1)];
		else 
			return spawn_points[Random.Range(0,spawn_points.Length-1)];
	}
	void SetColor(int i){
		string color;
		switch (i) {
		case 1:
			color = "red";
			break;
		default :
			color = "blue";
			break;
		}
		Object [] sprites = Resources.LoadAll<Sprite>("sprites/playerSpriteSheet_"+color);

		players [i].transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [0];
		players [i].transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites[1];
		players [i].transform.Find ("animation").Find ("weapon_trans").Find ("weapon").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites[2];

	}

	void OnGUI(){
		for (int i=0; i <players.Length; i++) {
			string message = "P" + i + " : " + score[i];
			GUI.Box (new Rect (50*i, 10, 50, 25), message);
		}
	}
}
