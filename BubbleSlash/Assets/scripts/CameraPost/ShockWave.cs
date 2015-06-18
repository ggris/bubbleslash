using UnityEngine;
using System.Collections;

public class ShockWave : MonoBehaviour
{
	public Material shockwave_material_;
	public Material flip_;
	public float speed_ = 1.2f;
	public float max_time_ = 1;
	public float variance_ = 0.5f;
	public float amplitude_ = 0.01f;
	private Material internal_material_;
	private Vector3 center_;
	private float time_;

	void Start ()
	{
		time_ = Time.time - max_time_; // No Shockwave on start.
		internal_material_ = new Material (shockwave_material_);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("i")) {
			time_ = Time.time;
		}
		
	}

	public void pop (Vector3 center)
	{
		time_ = Time.time;
		center_ = center;
		GetComponent<AudioSource> ().Play ();
	}

	float getRadius (float delta_t)
	{
		float radius = delta_t * (0.2f * delta_t + 0.5f);
		radius *= speed_;

		return radius;
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		float delta_t = 0.01f + Time.time - time_;
		if (delta_t < max_time_) {
			float radius = getRadius (delta_t);
			Vector3 center = GetComponent<Camera> ().WorldToViewportPoint (center_);
			
			float scale = -transform.position.z;
			radius /= scale;
			internal_material_.SetFloat ("_Radius", radius / scale);
			internal_material_.SetVector ("_Center", center);
			internal_material_.SetFloat ("_Sigma", variance_ / scale * radius);
			internal_material_.SetFloat ("_Amplitude", amplitude_ / scale * radius);
			internal_material_.SetFloat ("_Ratio", GetComponent<Camera> ().aspect);
			Graphics.Blit (source, destination, internal_material_);
		} else {
			Graphics.Blit (source, destination, flip_);
		}
	}

}
