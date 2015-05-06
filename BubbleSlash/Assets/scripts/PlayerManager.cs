using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public GameObject player_prefab;
	public Transform[] spawn_points;
	public GameObject [] players;
	public float min_distance_spawn;

	private int [] score;

	// Use this for initialization
	void Start () {
		score = new int[players.Length];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void dealWithDeath(int i){
		score [i] -= 1;
		spawn (i);
	}

	public void spawn(int i){
		int j =Random.Range(0,spawn_points.Length-1);
		players [i].transform.position = spawn_points [j].position;
	}

	void OnGUI(){

	}
}
