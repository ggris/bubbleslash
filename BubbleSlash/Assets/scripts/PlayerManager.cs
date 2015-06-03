using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public GameObject[] hat_prefabs;
	public GameObject player_prefab;
	public PlayerSettings.Hat [] hatChoices;

	public Transform[] spawn_points;
	public GameObject [] players;
	public float min_distance_spawn;
	public float death_altitude;
	private int [] score;
	private CameraTracking camera_tracking;


	void Start () {
		score = new int[players.Length];
		camera_tracking = (CameraTracking)FindObjectOfType (typeof(CameraTracking));
		spawn (0, spawn_points [0]);
		spawn (1, spawn_points [1]);
	}

	void Update () {
	}
	public void dealWithDeath(int i){
		score [i] -= 1;
		Destroy(players [i]);
		StartCoroutine (spawnLater (i, players [i].transform.position, 0.5f));

	}
	public IEnumerator spawnLater(int i, Vector3 position_death, float delayTime){
		yield return new WaitForSeconds(delayTime);
		Transform t = findRandomSpawnPoint (i, position_death);
		spawn (i, t);
	}


	public void spawn(int i, Transform t){
		players [i] = GameObject.Instantiate (player_prefab, t.position, new Quaternion (0, 0, 0, 0)) as GameObject;
		players [i].GetComponent<PlayerPhysics> ().playerNumber = i + 1;
		players [i].gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		SetColor (i);
		SetHatRendering (i);
		GameObject hat_i = GameObject.Instantiate (hat_prefabs [(int)hatChoices[i]], new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		hat_i.transform.parent = players [i].transform;
		players [i].GetComponent<PlayerPhysics>().hat= hat_i;

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
		/*string color;
		switch (i) {
		case 1:
			color = "red";
			break;
		default :
			color = "blue";
			break;
		}*/
		Object [] sprites = Resources.LoadAll<Sprite>("sprites/playerSpriteSheet_"+"blue");
		Object weapon = Resources.Load<Sprite> ("sprites/playerSpriteSheet_sword");
		players [i].transform.Find ("animation").Find ("body").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites [0];
		players [i].transform.Find ("animation").Find ("eye").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites[1];
		players [i].transform.Find ("animation").Find ("weapon_trans").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites[2];
		players [i].transform.Find ("animation").Find ("weapon_trans").Find ("weapon").gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)sprites[3];

	}
	void SetHatRendering(int i){
		PlayerSettings.Hat h = hatChoices[i];
		GameObject anim = players [i].transform.Find ("animation").gameObject;
		if (h == PlayerSettings.Hat.dashHat) {
			anim.transform.FindChild("dodgeHat").gameObject.SetActive(false);
			anim.transform.FindChild("dashHat").gameObject.SetActive(true);
		}
		if (h == PlayerSettings.Hat.dodgeHat) {
			anim.transform.FindChild("dashHat").gameObject.SetActive(false);
			anim.transform.FindChild("dodgeHat").gameObject.SetActive(true);
		}
	}

	void OnGUI(){
		for (int i=0; i <players.Length; i++) {
			string message = "P" + i + " : " + score[i];
			GUI.Box (new Rect (50*i, 10, 50, 25), message);
		}
	}
}
