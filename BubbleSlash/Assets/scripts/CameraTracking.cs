using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

	public Transform[] players;
	public float max_zoom = 2.0f;
	public float speed = 0.9f;

	private Camera my_camera;

	// Use this for initialization
	void Start () {
		my_camera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		updatePosition ();
		updateZoom ();

	}

	void updateZoom() {
		float dx = Mathf.Abs(players [0].position.x - players [1].position.x);
		float dyx = dx / my_camera.aspect;
		float dy = Mathf.Abs (players [0].position.y - players [1].position.y);
		dy = Mathf.Max(dyx, dy);
		
		dy = Mathf.Max (dy, max_zoom);
		dy *= speed;
		my_camera.orthographicSize *= (1.0f - speed);
		my_camera.orthographicSize += dy;
	}

	void updatePosition() {
		float x = (players [0].position.x + players [1].position.x) / 2.0f;
		float y = (players [0].position.y + players [1].position.y) / 2.0f;
		Vector3 target_position = new Vector3 (x, y, transform.position.z);
		target_position *= speed;
		transform.position *= (1.0f) - speed;
		transform.position += target_position;

	}
}
