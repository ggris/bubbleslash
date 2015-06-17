using UnityEngine;
using System.Collections;

public class MotionBlur : MonoBehaviour
{
	public Material accumulate_;
	Material internal_material_;
	RenderTexture last_;
	
	void Start ()
	{
		internal_material_ = new Material (accumulate_);
		last_ = new RenderTexture(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight, 0, RenderTextureFormat.Default);
		last_.filterMode = FilterMode.Bilinear;
		last_.Create();
		internal_material_.SetTexture("_Buffer", last_);

	}
	
	// Update is called once per frame
	void Update ()
	{	
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination, internal_material_);
		Graphics.Blit (destination, last_);
	}
}
