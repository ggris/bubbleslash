using UnityEngine;
using System.Collections;

public interface HatInterface {

	void applyPassiveEffect();
	void onSpecialStateEnter();
	void onSpecialStateExit();
	bool hasSpecialState();
}
