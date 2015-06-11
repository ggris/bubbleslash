using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxController : MonoBehaviour {

	public PlayerPhysics physics;
	private List<Collider2D> ignoredColliders;
	public Collider2D body;
	public void Awake(){
		ignoredColliders = new List<Collider2D>();
	}
	public void enter(Collider2D box, Collider2D other){
		if (!ignoredColliders.Contains (other)) {
			switch(box.gameObject.name){
			case "feet" :
				physics.is_grounded_ = true;
				break;
			case "left" :
				physics.is_touching_left_ = true;
				break;
			case "right" :
				physics.is_touching_right_ = true;
				break;
			default :
				break;
			}
		}
	}

	public void stay(Collider2D box, Collider2D other){
		if (!ignoredColliders.Contains (other)) {
			switch(box.gameObject.name){
			case "feet" :
				if (physics.getInputDirection().y<0){
					ignoredColliders.Add (other);
					Physics2D.IgnoreCollision(body,other,true);
				}
				else {
					physics.is_grounded_=true;
				}
				break;
			case "left" :
				physics.is_touching_left_ = true;
				break;
			case "right" :
				physics.is_touching_right_ = true;
				break;
			case "head" :
				if(physics.playerInputButton("Jump")){
					ignoredColliders.Add (other);
					Physics2D.IgnoreCollision(body,other,true);
				}
				break;
			default :
				break;
			}
		}

	}

	public void exit (Collider2D box, Collider2D other){
		//if (!ignoredColliders.Contains (other)) {
			switch(box.gameObject.name){
			case "feet" :
				physics.is_grounded_ = false;
				break;
			case "left" :
				physics.is_touching_left_ = false;
				break;
			case "right" :
				physics.is_touching_right_ = false;
				break;
			default :
				break;
			}
		//}
	}
	
	public void stopIgnore(Collider2D other){
		while (ignoredColliders.Contains (other)) {
			ignoredColliders.Remove(other);
		}
		Physics2D.IgnoreCollision (body, other, false);
	}
	public bool isIgnored(Collider2D other){
		return ignoredColliders.Contains (other);
	}

	void OnTriggerExit2D(Collider2D other){
		stopIgnore (other);
	}

}
