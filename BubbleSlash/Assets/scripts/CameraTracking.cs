using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

	private Transform[] players;
	public float min_zoom = 20.0f;
	public float max_zoom = 4.0f;


	public float acceleration_target_ = 0.9f;
	private float acceleration_;
	public float damp_target_ = 0.9f;
	private float damp_;

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
		acceleration_ = acceleration_target_;
		damp_ = damp_target_;
	}
	
	// Update is called once per frame
	void Update () {
		if (players.Length>=1){
			updateMinMax ();
			updatePosition ();
			//updateZoom ();
		}
		acceleration_ *= damp_;
		acceleration_ += acceleration_target_ * (1 - damp_);
		damp_ *= 0.9f;
		damp_ += damp_target_ * (1 - 0.9f);
		if (Input.GetKeyDown(KeyCode.U)) {
			hit(new Vector2(1, 1));
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
		min_ = new Vector2();
		max_ = new Vector2();
		for (int i=0; i< players.Length; ++i) {
			Vector2 pos = players [i].position;
			min_.x = Mathf.Min(pos.x, min_.x);
			min_.y = Mathf.Min(pos.y, min_.y);
			max_.x = Mathf.Max(pos.x, max_.x);
			max_.y = Mathf.Max(pos.y, max_.y);
		}
	}

	float deltaZoom() {
		Vector2 delta = max_ - min_;
		delta.x /= my_camera.aspect;
		float delta_ortho = Mathf.Max (delta.x, delta.y);
		delta_ortho += max_zoom;
		delta_ortho /= 1.3f;
		delta_ortho = moveTowards (delta_ortho, my_camera.orthographicSize);
		delta_ortho /= my_camera.orthographicSize;
		
		zoom_speed_ *= damp_;
		zoom_speed_ += delta_ortho;

		return delta_ortho;
	}

	void updateZoom() {

		my_camera.orthographicSize += deltaZoom ();

		my_camera.orthographicSize = Mathf.Clamp (my_camera.orthographicSize, max_zoom, min_zoom);
	}

	void updatePosition() {
		Vector2 center = (max_ + min_) / 2;
		Vector2 delta = max_ - min_;
		delta.x /= my_camera.aspect;
		float dz = Mathf.Max (delta.x, delta.y);
		dz += max_zoom;
		dz *= -1.5f;
		float dx = moveTowards (center.x, transform.position.x);
		float dy = moveTowards (center.y, transform.position.y);
		dz = moveTowards (dz, transform.position.z);
		dx = Mathf.Clamp (dx, -acceleration_*2, acceleration_*2);
		dy = Mathf.Clamp (dy, -acceleration_*2, acceleration_*2);

		pos_speed_ *= damp_;
		pos_speed_ += new Vector2 (dx, dy);
		zoom_speed_ *= damp_;
		zoom_speed_ += dz;

		Vector3 target_position = new Vector3 (pos_speed_.x, pos_speed_.y, 0.1f*zoom_speed_);
		transform.position += target_position;
		//transform.position += new Vector3 (Mathf.Sin (10.0f*Time.time), Mathf.Cos (10.0f*Time.time), 0) * 0.1f;
	}

	public void hit(Vector2 direction) {
		direction += 0.3f * new Vector2 (Random.Range(-1, 1), Random.Range(-1, 1));
		direction *= 0.1f;
 		pos_speed_ += direction;
		damp_ = 0.8f;
		acceleration_ *= 0.0f;
	}

	float moveTowards(float a, float b) {
		float d = a - b;
		d *= Mathf.Abs (d) * acceleration_;
		return d;
	}
}
