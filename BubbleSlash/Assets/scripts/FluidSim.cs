using UnityEngine;
using System.Collections;

public class FluidSim : MonoBehaviour {

	public Vector2 speed;

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
	public float source_velocity = 1.0f;
	public Material advect_mat_, buoyancy_mat_, source_mat_, divergence_mat_, jacoby_mat_, substract_gradient_;
	public int jacobi_itts_ = 1;
	public bool snap_ = true;
	public float grad_scale_ = 1.0f;
	public float cell_size_ = 1.0f;

	float inverse_size_;

	public Material post_material;
	
	RenderTexture[] density_, velocity_, temperature_, pressure_;
	RenderTexture divergence_, obstacles_;
	
	// Use this for initialization
	void Start () {
		inverse_size_ = 1.0f / resolution_;
		
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

		obstacles_ = new RenderTexture(resolution_, resolution_, 0, RenderTextureFormat.RFloat);
		obstacles_.filterMode = FilterMode.Point;
		divergence_.wrapMode = TextureWrapMode.Clamp;
		obstacles_.Create();
		
		post_material.SetTexture ("_Blood", density_ [0]);
		Material post_mat_copy = new Material (post_material);
		GetComponent<SpriteRenderer> ().material = post_mat_copy;

		//source ();
		
	}
	
	void createBuffer(RenderTexture[] buffer, RenderTextureFormat format, FilterMode filter)
	{
		buffer[0] = new RenderTexture(resolution_, resolution_, 0, format);
		buffer[0].filterMode = filter;
		buffer[0].wrapMode = TextureWrapMode.Clamp;
		buffer[0].Create();
		Graphics.SetRenderTarget (buffer [0]);
		GL.Clear (false, true, new Color (0, 0, 0, 0));		
		Graphics.SetRenderTarget (null);
		
		buffer[1] = new RenderTexture(resolution_, resolution_, 0, format);
		buffer[1].filterMode = filter;
		buffer[1].wrapMode = TextureWrapMode.Clamp;
		buffer[1].Create();
		Graphics.SetRenderTarget (buffer [1]);
		GL.Clear (false, true, new Color (0, 0, 0, 0));		
		Graphics.SetRenderTarget (null);
	}
	
	void swapBuffer (RenderTexture[] buffer) {
		RenderTexture tmp = buffer [0];
		buffer [0] = buffer [1];
		buffer [1] = tmp;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Advect
		advect (velocity_[0], density_ [0], density_ [1], dissipation_);
		advect (velocity_[0], velocity_ [0], velocity_ [1], dissipation_);
		//advect (velocity_[0], temperature_ [0], temperature_ [1], dissipation_);
		swapBuffer (velocity_);
		//swapBuffer (temperature_);
		swapBuffer (density_);
		
		// Buoyancy
		//buoyancy (velocity_ [0], temperature_ [0], density_ [0], velocity_ [1]);
		//swapBuffer (velocity_);
		
		// Divergence field

		divergence(velocity_[0], divergence_);
		
		// Clear texture
		Graphics.SetRenderTarget (pressure_ [0]);
		GL.Clear (false, true, new Color (0, 0, 0, 0));		
		Graphics.SetRenderTarget (null);
		
		for(int i = 0; i < jacobi_itts_; ++i) 
		{
			jacobi(pressure_[0], divergence_, pressure_[1]);
			swapBuffer(pressure_);
		}

		subGrad(velocity_[0], pressure_[0], velocity_[1]);
		
		swapBuffer(velocity_);
		
	}

	void source()
	{
		//source(temperature_[0], new Vector3(source_temperature_,source_temperature_,source_temperature_));
		source(density_[0], new Vector3(source_density_,source_density_,source_density_));
		source (velocity_[0], new Vector3(speed.x, speed.y, 0));
	}
	
	void advect(RenderTexture velocity, RenderTexture source, RenderTexture dest, float dissipation)
	{
		
		advect_mat_.SetFloat("_InverseSize", inverse_size_);
		advect_mat_.SetFloat("_TimeStep", time_step_);
		advect_mat_.SetTexture("_Obstacles", obstacles_);

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
	
	void source(RenderTexture dest, Vector3 val)
	{
		source_mat_.SetVector("_Point", source_pos_);
		source_mat_.SetVector("_FillColor", val);
		
		Graphics.Blit(null, dest, source_mat_);
		
	}
	
	void divergence(RenderTexture velocity,  RenderTexture dest)
	{
		
		divergence_mat_.SetFloat("_HalfInverseCellSize", 0.5f / cell_size_);
		divergence_mat_.SetTexture("_Velocity", velocity);
		divergence_mat_.SetVector("_InverseSize", new Vector2(inverse_size_, inverse_size_));
		
		Graphics.Blit(null, dest, divergence_mat_);
	}
	
	void jacobi(RenderTexture pressure, RenderTexture divergence, RenderTexture dest)
	{
		float alpha = cell_size_ * cell_size_;
		jacoby_mat_.SetTexture("_Pressure", pressure);
		jacoby_mat_.SetTexture("_Divergence", divergence);
		jacoby_mat_.SetVector("_InverseSize", new Vector2(inverse_size_, inverse_size_));
		jacoby_mat_.SetFloat("_Alpha", alpha);
		jacoby_mat_.SetFloat("_RBeta", 1/(4+alpha));
		
		Graphics.Blit(null, dest, jacoby_mat_);
	}
	
	void subGrad(RenderTexture velocity, RenderTexture pressure, RenderTexture dest)
	{
		substract_gradient_.SetTexture("_Velocity", velocity);
		substract_gradient_.SetTexture("_Pressure", pressure);
		substract_gradient_.SetFloat("_GradScale", grad_scale_);
		substract_gradient_.SetVector("_InverseSize", new Vector2(inverse_size_, inverse_size_));
		
		Graphics.Blit(null, dest, substract_gradient_);
	}
	
	
}
