using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour {
	private AudioSource source;
	public AudioClip jump_;
	public AudioClip slide_;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void jump(){
		if (GetComponent<PlayerPhysics> ().is_grounded)
			source.pitch = 2;
		else
			source.pitch = 2.5f;
		source.clip = jump_;
		source.Play ();
	}


	public void startSlide(){
		source.loop = true;
		source.clip = slide_;
		source.Play ();
	}

	public void stopSlide(){
		source.loop = false;
	}
}
