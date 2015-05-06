using UnityEngine;
using System.Collections;

public class FluidSim : MonoBehaviour {

	[Range(1,1024)]
	public int resolution_ = 64;
	[Range(0.001f,10.0f)]
	public float time_step_ = 1.0f;
	public RenderTexture obstacles_, initial_density_, initial_velocity_;


	float inverse_size_;

	RenderTexture[] density_, velocity_;
	Material advect_mat_;

	// Use this for initialization
	void Start () {
		inverse_size_ = 1.0f / resolution_;

		advect_mat_ = new Material ("Advect");
		advect_mat_.SetFloat("_InverseSize", inverse_size_);
		advect_mat_.SetFloat("_TimeStep", time_step_);
		advect_mat_.SetTexture("_Obstacles", obstacles_);
		
		velocity_  = new RenderTexture[2];
		density_  = new RenderTexture[2];

		createBuffer(velocity_, RenderTextureFormat.RGFloat, FilterMode.Bilinear);
		createBuffer(density_, RenderTextureFormat.RFloat, FilterMode.Bilinear);

	}

	void createBuffer(RenderTexture[] buffer, RenderTextureFormat format, FilterMode filter)
	{
		buffer[0] = new RenderTexture(resolution_, resolution_, 0, format);
		buffer[0].filterMode = filter;
		buffer[0].wrapMode = TextureWrapMode.Clamp;
		buffer[0].Create();
		
		buffer[1] = new RenderTexture(resolution_, resolution_, 0, format);
		buffer[1].filterMode = filter;
		buffer[1].wrapMode = TextureWrapMode.Clamp;
		buffer[1].Create();
	}
	
	// Update is called once per frame
	void Update () {
		advect (velocity_[0], density_ [0], density_ [1], 0.0f);
	
	}
	
	public Material post_material;
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		density_[0] = source;
		velocity_ [0] = source;
		Graphics.Blit (density_[1], destination, post_material);
	}
	
	void advect(RenderTexture velocity, RenderTexture source, RenderTexture dest, float dissipation)
	{
		advect_mat_.SetFloat("_Dissipation", dissipation);
		advect_mat_.SetTexture("_Velocity", velocity);
		advect_mat_.SetTexture("_Source", source);
		
		Graphics.Blit(null, dest, advect_mat_);
	}

}
