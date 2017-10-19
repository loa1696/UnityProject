using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Vector3 MoveBy;
	public float speed = 2;
	public float pauseTime = 1;

	Vector3 pointA;
	Vector3 pointB;

	bool isGoingToA = false;
	bool pause = false;
	float currentPauseTime = 0;

	// Use this for initialization
	void Start () {
		this.pointA = this.transform.position;
		this.pointB = this.pointA + MoveBy;
	}

	bool hasArrived (Vector3 position, Vector3 destination) {
		position.z = 0;
		destination.z = 0;
		return Vector3.Distance (position, destination) < 0.2f;
	}

	// Update is called once per frame
	void Update () {
		Vector3 myPos = this.transform.position;
		Vector3 target;
		myPos.z = 0;

		if (pause) {
			currentPauseTime -= Time.deltaTime;
			if (currentPauseTime <= 0) pause = false;
			return;
		}

		if (isGoingToA) 	target = this.pointA;
		else 				target = this.pointB;

		Vector3 movementVector = target - myPos;
		movementVector.z = 0;

		if (movementVector.magnitude <= speed * Time.deltaTime) {
			this.transform.position = Vector3.Lerp(myPos, target, movementVector.magnitude);
			if (hasArrived (myPos, target)) this.isGoingToA = !this.isGoingToA;
			pause = true;
			currentPauseTime = pauseTime;
		} else {
			this.transform.position = Vector3.Lerp(myPos, target, speed * Time.deltaTime);
		}	
	}
}