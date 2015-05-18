using UnityEngine;
using System.Collections;

public abstract class HatAbstractClass : MonoBehaviour {
	protected GameObject player;

	void Start () {
		player = gameObject.transform.parent.gameObject;
	}
	
	public virtual void applyPassiveEffect(){}
	public virtual void onSpecialStateEnter(){}
	public virtual void onSpecialStateExit(){}
	public virtual bool hasSpecialState(){return false;}

}
