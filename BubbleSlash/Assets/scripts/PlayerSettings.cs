using UnityEngine;
using System.Collections;

public class PlayerSettings {

	public enum Hat {testHat=0, speedHat=1, dashHat=2, dodgeHat=3};
	public enum Weapon {sword};

	public static Hat nextHat(Hat myhat){
		int output = ((int)myhat + 1) % 4;
		return (Hat)output;
	}

	public static string ToString(Hat myHat){
		switch (myHat){
		case Hat.testHat :
			return "test";
		case Hat.speedHat :
			return "speed";
		case Hat.dodgeHat :
			return "dodge";
		case Hat.dashHat :
			return "dash";
		default :
			return "";
		}

	}
}
