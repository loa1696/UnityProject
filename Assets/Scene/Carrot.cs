using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Collectable {

	float speed = 2;
	float direction = 0;

	void Start() {
		StartCoroutine(destroyLater());
	}

	IEnumerator destroyLater () {
		yield return new WaitForSeconds(3.0f);
		Destroy(this.gameObject);
	}

	public void launch (float direction) {
		this.direction = direction;
		if (direction < 0) GetComponent<SpriteRenderer>().flipX = true;
	}

	public void Update () {
		Debug.Log ("update");
		Vector3 pos = this.transform.position;
		this.transform.position = pos + this.direction * Vector3.right * Time.deltaTime * speed;
	}

	protected override void OnRabitHit (HeroRabbit rabit){
		Debug.Log ("hit");
		if (!rabit.beBig (false)) rabit.die(this.transform);
		Destroy(this.gameObject);
	}
}