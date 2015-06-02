using UnityEngine;
using System.Collections;

public class SpeedHatScript : HatAbstractClass {


	override public void applyPassiveEffect(){
		player.GetComponent<PlayerPhysics> ().ground_acc = 20000;
		player.GetComponent<PlayerPhysics> ().air_acc = 5000;
		player.GetComponent<PlayerPhysics> ().max_horizontal_speed = 30;

		player.GetComponent<PlayerPhysics> ().push_wall_speed = 25;
		player.GetComponent<PlayerPhysics> ().push_air_speed = 25;
	}

	override public bool hasSpecialState(){
		return false;
	}
}
