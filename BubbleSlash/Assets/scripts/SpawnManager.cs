using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
	public float min_spawn_distance_ = 1.0f;

	private Vector2[] respawns_;

	// Use this for initialization
	void Start ()
	{
		GameObject[] respawns = GameObject.FindGameObjectsWithTag ("Respawn");
		respawns_ = new Vector2[respawns.Length];
		for (int i = 0; i< respawns_.Length; ++i)
			respawns_[i] = respawns[i].transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public Vector2 getRespawnPoint (Vector2 player_pos)
	{
		List<Vector2> far_enough_spawn_points = new List<Vector2> ();
		float sqr_max_mag = min_spawn_distance_ * min_spawn_distance_;
		foreach (Vector2 pos in respawns_) {
			if ((pos - player_pos).sqrMagnitude > sqr_max_mag)
				far_enough_spawn_points.Add (pos);
		}
		if (far_enough_spawn_points.Count > 0) 
			return far_enough_spawn_points [Random.Range (0, far_enough_spawn_points.Count - 1)];
		else 
			return respawns_ [Random.Range (0, respawns_.Length - 1)];
	}

}
