using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
	public float min_spawn_distance_ = 1.0f;

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

	Vector2 getSpawnPoint (Vector2 player_pos)
	{
		List<Vector2> far_enough_spawn_points = new List<Vector2> ();
		float sqr_max_mag = min_spawn_distance_ * min_spawn_distance_;
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
