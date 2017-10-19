using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable {

	protected override void OnRabitHit(HeroRabbit rabit) {
		this.CollectedHide();
	}

}