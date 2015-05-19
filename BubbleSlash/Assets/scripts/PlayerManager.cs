using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

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

	public void dealWithDeath(int i){
		score [i] -= 1;
		Destroy(players [i]);
		indice = i; //needed for passing arguments to invoked method
		Invoke("spawn",0.5f);
	}

	public void spawn(){
		int i = indice;
		int j =Random.Range(0,spawn_points.Length-1);


		players [i] = GameObject.Instantiate (player_prefab, spawn_points [j].position, new Quaternion (0, 0, 0, 0)) as GameObject;
		players [i].GetComponent<PlayerPhysics> ().playerNumber = i + 1;
		players [i].gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);

		camera_tracking.resetPlayers ();
	}

	void OnGUI(){
		for (int i=0; i <players.Length; i++) {
			string message = "P" + i + " : " + score[i];
			GUI.Box (new Rect (50*i, 10, 50, 25), message);
		}
	}
}
