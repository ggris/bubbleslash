using UnityEngine;
using System.Collections;

public class FluidSim : MonoBehaviour {


	[Range(1,1024)]
	public int grid_scale_ = 64;
	[Range(0, 100)]
	public float percent_dissipation_ = 1.0f;
	public Vector2 source_pos_ = new Vector2(0.5f, 0.5f);
	public float source_density_ = 1.0f;
	public Vector2 initial_velocity_ = new Vector2(0, 0);
	public float rand_velocity = 1.0f;
	public float velocity_scale_ = 2.0f;
	public Material liquid_mat_, source_mat_;
	
	float inverse_size_;
	float delta_;

	public float emit_time_ = 0.1f;	
	public float end_time_ = 2.0f;
	float start_time_;
	float last_time_;

	public Material post_material;
	
	RenderTexture[] fluid_;
	RenderTexture obstacles_;
	
	// Use this for initialization
	void Start () {

		// Init member variables

		start_time_ = Time.time + end_time_;
		last_time_ = Time.time;

		inverse_size_ = 1.0f / grid_scale_;

		// Create fluid computation buffer

		fluid_ = createBuffer(RenderTextureFormat.ARGBFloat, FilterMode.Bilinear);

		// Create and fill obstacles texture

		obstacles_ = new RenderTexture(grid_scale_, grid_scale_, 0, RenderTextureFormat.RFloat);
		obstacles_.filterMode = FilterMode.Bilinear;
		obstacles_.Create();

		Camera camera = GetComponent<Camera> ();
		camera.aspect = 1.0f;
		camera.targetTexture = obstacles_;
		camera.Render ();

		// Creating a copy of the post material to display the fluid.

		Material post_mat_copy = new Material (post_material);
		post_mat_copy.SetTexture ("_Blood", fluid_[0]);
		GetComponent<SpriteRenderer> ().material = post_mat_copy;
		
	}
	
	RenderTexture[] createBuffer(RenderTextureFormat format, FilterMode filter)
	{
		RenderTexture[] buffer = new RenderTexture[2];
		buffer[0] = new RenderTexture(grid_scale_, grid_scale_, 0, format);
		buffer[0].filterMode = filter;
		buffer[0].wrapMode = TextureWrapMode.Clamp;
		buffer[0].Create();
		Graphics.SetRenderTarget (buffer [0]);
		GL.Clear (false, true, new Color (0, 0, 0, 0));		
		Graphics.SetRenderTarget (null);
		
		buffer[1] = new RenderTexture(grid_scale_, grid_scale_, 0, format);
		buffer[1].filterMode = filter;
		buffer[1].wrapMode = TextureWrapMode.Clamp;
		buffer[1].Create();
		Graphics.SetRenderTarget (buffer [1]);
		GL.Clear (false, true, new Color (0, 0, 0, 0));		
		Graphics.SetRenderTarget (null);

		return buffer;
	}
	
	void swapBuffer (RenderTexture[] buffer) {
		RenderTexture tmp = buffer [0];
		buffer [0] = buffer [1];
		buffer [1] = tmp;
	}
	
	// Update is called once per frame
	void Update () {
		if (start_time_ + end_time_ > Time.time)
			SimUpdate ();
	}

	void SimUpdate () {
		delta_ = (Time.time - last_time_) * 0.01f * velocity_scale_;
		last_time_ = Time.time;
		
		if (start_time_ + emit_time_ > Time.time)
			source (fluid_[0], new Vector3((initial_velocity_.x + Random.value*rand_velocity)*velocity_scale_, (initial_velocity_.y + Random.value*rand_velocity)*velocity_scale_,source_density_));

		liquid_mat_.SetFloat("_Delta", delta_);
		liquid_mat_.SetFloat("_InvGridScale", inverse_size_);
		liquid_mat_.SetInt("_GridScale", grid_scale_);
		liquid_mat_.SetFloat("_VelScale", velocity_scale_);
		liquid_mat_.SetTexture("_Obstacles", obstacles_);
		liquid_mat_.SetTexture("_UxUyD", fluid_[0]);
		
		Graphics.Blit(null, fluid_[1], liquid_mat_);
		
		swapBuffer (fluid_);
	}

	void source()
	{
		start_time_ = last_time_ = Time.time;
	}
	
	void source(RenderTexture dest, Vector3 val)
	{
		source_mat_.SetVector("_Point", source_pos_);
		source_mat_.SetVector("_FillColor", val);
		
		Graphics.Blit(null, dest, source_mat_);
		
	}
	
	
}
