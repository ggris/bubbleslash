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

		float d = smoothDelta (dy, my_camera.orthographicSize);

		d /= my_camera.orthographicSize;

		//dy *= speed;
		//my_camera.orthographicSize *= (1.0f - speed);
		my_camera.orthographicSize += d;
	}

	void updatePosition() {
		float x = (players [0].position.x + players [1].position.x) / 2.0f;
		float y = (players [0].position.y + players [1].position.y) / 2.0f;
		float dx = smoothDelta (x, transform.position.x);
		float dy = smoothDelta (y, transform.position.y);
		Vector3 target_position = new Vector3 (dx, dy, 0);
		transform.position += target_position;
	}

	float smoothDelta(float a, float b) {
		float d = a - b;
		d *= Mathf.Abs (d) * speed;
		return d;
	}
}
