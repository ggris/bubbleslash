using UnityEngine;
using System.Collections;

public class FluidSim : MonoBehaviour {

	[Range(1,1024)]
	public int resolution_ = 64;
	[Range(0.001f,10.0f)]
	public float time_step_ = 0.02f;
	[Range(0.0f,1.0f)]
	public float dissipation_ = 0.005f;
	public float ambient_temperature_ = 0.0f;
	public float smoke_buoyancy_ = 1.0f;
	public float smoke_weight_ = 1.0f;
	public Vector2 source_pos_;
	public float source_temperature_ = 0.0f;
	public float source_density_ = 1.0f;
	public RenderTexture obstacles_, initial_density_, initial_velocity_;
	public Material advect_mat_, buoyancy_mat_, source_mat_, divergence_mat;
	public bool snap_ = true;


	float inverse_size_;

	RenderTexture[] density_, velocity_, temperature_, pressure_;
	RenderTexture divergence_;

	// Use this for initialization
	void Start () {
		inverse_size_ = 1.0f / resolution_;

		advect_mat_.SetFloat("_InverseSize", inverse_size_);
		advect_mat_.SetFloat("_TimeStep", time_step_);
		advect_mat_.SetTexture("_Obstacles", obstacles_);
		
		buoyancy_mat_.SetFloat("_AmbientTemperature", ambient_temperature_);
		buoyancy_mat_.SetFloat("_TimeStep", time_step_);
		buoyancy_mat_.SetFloat("_Sigma", smoke_buoyancy_);
		buoyancy_mat_.SetFloat("_Kappa", smoke_weight_);
		
		velocity_  = new RenderTexture[2];
		density_  = new RenderTexture[2];
		temperature_ = new RenderTexture[2];
		pressure_ = new RenderTexture[2];
		
		createBuffer(density_, RenderTextureFormat.RFloat, FilterMode.Bilinear);
		createBuffer(velocity_, RenderTextureFormat.RGFloat, FilterMode.Bilinear);
		createBuffer(temperature_, RenderTextureFormat.RFloat, FilterMode.Bilinear);
		createBuffer(pressure_, RenderTextureFormat.RFloat, FilterMode.Point);

		divergence_ = new RenderTexture(resolution_, resolution_, 0, RenderTextureFormat.RFloat);
		divergence_.filterMode = FilterMode.Point;
		divergence_.wrapMode = TextureWrapMode.Clamp;
		divergence_.Create();

		post_material.SetTexture ("_Blood", density_ [0]);

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

	void swapBuffer (RenderTexture[] buffer) {
		RenderTexture tmp = buffer [0];
		buffer [0] = buffer [1];
		buffer [1] = tmp;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("o")) {
			snap_ = true;
		}

		// Advect
		advect (velocity_[0], velocity_ [0], velocity_ [1], dissipation_);
		advect (velocity_[0], temperature_ [0], temperature_ [1], dissipation_);
		advect (velocity_[0], density_ [0], density_ [1], dissipation_);
		swapBuffer (velocity_);
		swapBuffer (temperature_);
		swapBuffer (density_);

		// Buoyancy
		buoyancy (velocity_ [0], temperature_ [0], density_ [0], velocity_ [1]);
		swapBuffer (velocity_);

		// Source
		source(temperature_[0], source_temperature_);
		source(density_[0], source_density_);

		// Divergence field

		divergence(velocity_[0], divergence_);

		// Clear texture
		Graphics.SetRenderTarget (pressure_ [0]);
		GL.Clear (false, true, new Color (0, 0, 0, 0));		
		Graphics.SetRenderTarget (null);
	}
	
	public Material post_material;
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (snap_) {
			Graphics.Blit (source, velocity_ [0], post_material);
			Graphics.Blit (source, density_ [0], post_material);
			Graphics.Blit (source, temperature_ [0], post_material);
			Graphics.Blit (source, pressure_ [0], post_material);
			snap_ = false;
		}

		Graphics.Blit (source, destination, post_material);
	}
	
	void advect(RenderTexture velocity, RenderTexture source, RenderTexture dest, float dissipation)
	{
		advect_mat_.SetFloat("_Dissipation", dissipation);
		advect_mat_.SetTexture("_Velocity", velocity);
		advect_mat_.SetTexture("_Source", source);
		
		Graphics.Blit(null, dest, advect_mat_);
	}

	void buoyancy(RenderTexture velocity, RenderTexture temperature, RenderTexture density, RenderTexture dest)
	{
		
		buoyancy_mat_.SetTexture("_Velocity", velocity);
		buoyancy_mat_.SetTexture("_Temperature", temperature);
		buoyancy_mat_.SetTexture("_Density", density);
		
		Graphics.Blit(null, dest, buoyancy_mat_);
	}
	
	void source(RenderTexture dest, float val)
	{
		source_mat_.SetVector("_Point", source_pos_);
		source_mat_.SetVector("_FillColor", new Vector3(val,val,val));
		
		Graphics.Blit(null, dest, source_mat_);
		
	}

	void divergence(RenderTexture velocity,  RenderTexture dest)
	{
		
		divergence_mat.SetFloat("_HalfInverseCellSize", 0.5f / 1.0f);
		divergence_mat.SetTexture("_Velocity", velocity);
		divergence_mat.SetVector("_InverseSize", new Vector2(inverse_size_, inverse_size_));
		
		Graphics.Blit(null, dest, divergence_mat);
	}


}
