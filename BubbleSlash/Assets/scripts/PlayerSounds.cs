using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour {
	private AudioSource source;
	public AudioClip jump_;
	public AudioClip slide_;
	public AudioClip attack_;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void jump(){
		if (GetComponent<PlayerPhysics> ().is_grounded_)
			source.pitch = 2;
		else
			source.pitch = 2.5f;
		source.clip = jump_;
		source.Play ();
	}


	public void startSlide(){
		source.pitch = 1f;
		source.loop = true;
		source.clip = slide_;
		source.Play ();
	}

	public void stopSlide(){
		source.loop = false;
	}
	public void attack(){
		source.clip = attack_;
		source.pitch = Random.Range (0.4f, 1.6f);
		source.Play ();
	}
	public void dash(){
		source.clip = attack_;
		source.pitch = 0.3f;
		source.Play ();
	}
}
