using UnityEngine;
using System.Collections;

public class PlayerSettings {
	public enum Hat {testHat=0, speedHat=1, dashHat=2, dodgeHat=3};
	public enum Weapon {sword};
	public Hat hat;
	public Weapon weapon;
	public int player_number;

	public PlayerSettings(int playernumber, string hatname, string swordname){

		switch (hatname) {
		case "testhat":
			hat=Hat.testHat;
			break;
		case "speedhat":
			hat=Hat.speedHat;
			break;
		default :
			hat=Hat.speedHat;
			break;
		}
		weapon = Weapon.sword;
	}
}
