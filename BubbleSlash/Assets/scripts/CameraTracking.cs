using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

	private Transform[] players;
	public float min_zoom = 20.0f;
	public float max_zoom = 4.0f;


	public float acceleration_ = 0.9f;
	public float damp_ = 0.9f;

	private Camera my_camera;

	private Vector2 max_;
	private Vector2 min_;

	private float zoom_speed_;
	private Vector2 pos_speed_;

	// Use this for initialization
	void Start () {
		my_camera = GetComponent<Camera> ();
		resetPlayers ();
		zoom_speed_ = 0.0f;
		pos_speed_ = new Vector2 ();
	}
	
	// Update is called once per frame
	void Update () {
		if (players.Length>=1){
			updateMinMax ();
			updatePosition ();
			updateZoom ();
		}
	}
	public void resetPlayers(){
		Object [] pp = FindObjectsOfType (typeof(PlayerPhysics));
		players = new Transform [pp.Length];
		for (int i = 0; i<pp.Length; i++){
			players[i]=((Component)pp[i]).gameObject.transform;
		}
	}

	void updateMinMax() {
		min_ = players [0].position;
		max_ = players [0].position;
		for (int i=1; i< players.Length; ++i) {
			Vector2 pos = players [i].position;
			min_.x = Mathf.Min(pos.x, min_.x);
			min_.y = Mathf.Min(pos.y, min_.y);
			max_.x = Mathf.Max(pos.x, max_.x);
			max_.y = Mathf.Max(pos.y, max_.y);
		}
	}

	void updateZoom() {
		Vector2 delta = max_ - min_;
		delta.x /= my_camera.aspect;
		float delta_ortho = Mathf.Max (delta.x, delta.y);
		delta_ortho += max_zoom;
		delta_ortho /= 1.3f;
		delta_ortho = moveTowards (delta_ortho, my_camera.orthographicSize);
		delta_ortho /= my_camera.orthographicSize;

		zoom_speed_ *= damp_;
		zoom_speed_ += delta_ortho;

		my_camera.orthographicSize += delta_ortho;

		my_camera.orthographicSize = Mathf.Clamp (my_camera.orthographicSize, max_zoom, min_zoom);
	}

	void updatePosition() {
		Vector2 center = (max_ + min_) / 2;
		float dx = moveTowards (center.x, transform.position.x);
		float dy = moveTowards (center.y, transform.position.y);
		dx = Mathf.Clamp (dx, -acceleration_*2, acceleration_*2);
		dy = Mathf.Clamp (dy, -acceleration_*2, acceleration_*2);

		pos_speed_ *= damp_;
		pos_speed_ += new Vector2 (dx, dy);


		Vector3 target_position = new Vector3 (pos_speed_.x, pos_speed_.y, 0);
		transform.position += target_position;
		transform.position += new Vector3 (Mathf.Sin (10.0f*Time.time), Mathf.Cos (10.0f*Time.time), 0) * 0.1f;
	}

	float moveTowards(float a, float b) {
		float d = a - b;
		d *= Mathf.Abs (d) * acceleration_;
		return d;
	}
}
