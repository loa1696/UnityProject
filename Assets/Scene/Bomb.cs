using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable {

	protected override void OnRabitHit(HeroRabbit rabit) {
		if (!rabit.beBig (false)) rabit.die(this.transform);
		this.CollectedHide();
	}

}