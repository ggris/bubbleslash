using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnlinePlayerManager : MonoBehaviour
{
	public float min_spawn_distance_ = 1.0f;

	private HashSet<GameObject> players_ = new HashSet<GameObject>();
	private Vector2[] spawns_;

	// Use this for initialization
	void Start ()
	{
		Transform[] spawns_transforms = GetComponentsInChildren<Transform> ();
		spawns_ = new Vector2[spawns_transforms.Length];
		for (int i = 0; i< spawns_.Length; ++i)
			spawns_[i] = spawns_transforms[i].position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void addPlayer (GameObject player)
	{
		players_.Add (player);
	}

	void removePlayer (GameObject player)
	{
		players_.Remove (player);
	}

	Vector2 getSpawnPoint (GameObject player)
	{
		List<Vector2> far_enough_spawn_points = new List<Vector2> ();
		float sqr_max_mag = min_spawn_distance_ * min_spawn_distance_;
		Vector2 player_pos = player.transform.position;
		foreach (Vector2 pos in spawns_) {
			if ((pos - player_pos).sqrMagnitude > sqr_max_mag)
				far_enough_spawn_points.Add (pos);
		}
		if (far_enough_spawn_points.Count > 0) 
			return far_enough_spawn_points [Random.Range (0, far_enough_spawn_points.Count - 1)];
		else 
			return spawns_ [Random.Range (0, spawns_.Length - 1)];
	}

}
