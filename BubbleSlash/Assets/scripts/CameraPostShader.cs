using UnityEngine;
using System.Collections;

public class CameraPostShader : MonoBehaviour
{
	public Material post_material;
	public float speed_ = 1.2f;
	public Vector3 center_;

	private float time_;

	void Start () {
		time_ = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("i")) {
			time_ = Time.time;
		}
		
	}

	void pop(Vector3 center)
	{
		time_ = Time.time;
		center_ = center;
	}

	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		//mat is the material containing your shader

		float radius = Time.time - time_;
		radius *= (0.2f*radius+1);
		radius *= speed_;
		Vector3 center = center_ - transform.position;
		float scale = GetComponent<Camera>().orthographicSize;
		center /= scale;
		center = GetComponent<Camera> ().WorldToViewportPoint (center_);
		radius /= scale;
		post_material.SetFloat ("_Radius", radius);
		post_material.SetVector ("_Center", center);
		Graphics.Blit (source, destination, post_material);
	}

}
