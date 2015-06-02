using UnityEngine;
using System.Collections;

public abstract class HatAbstractClass : MonoBehaviour {
	protected GameObject player;
	protected Animator anim;

	public float length = 0.5f;
	public float cd = 0.5f;
	public float timer;

	void Start () {
		player = gameObject.transform.parent.gameObject;
		anim = gameObject.GetComponent<Animator> ();
		applyPassiveEffect ();
		startHat ();
	}
	
	public virtual void applyPassiveEffect(){}
	public virtual void onSpecialStateEnter(){}
	public virtual void onSpecialStateExit(){}
	public virtual bool hasSpecialState(){return false;}
	public virtual bool canAttack(){return true;}

	public virtual void updateHat (){}
	public virtual void startHat (){}

	public bool isNotInCd(){
		return !anim.GetCurrentAnimatorStateInfo (0).IsName ("cd");
	}

	public void Update () {
		if (hasSpecialState ()) {
			float nextTimer = timer + Time.deltaTime;
			if (timer < length && nextTimer > length) {
				anim.SetTrigger ("finish");
			}
			if (timer < length + cd && nextTimer > length + cd) {
				anim.SetTrigger ("cdover");
			}
			timer = nextTimer;
		}
		updateHat ();

	}
}
