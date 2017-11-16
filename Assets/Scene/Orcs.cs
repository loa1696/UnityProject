using UnityEngine;
using System.Collections;

public class Orcs : MonoBehaviour
{
	
	public Vector3 MoveBy;

	protected Vector3 pointA;
	protected Vector3 pointB;

	protected float speed;

	protected Animator animator = null;
	protected Rigidbody2D body = null;
	protected SpriteRenderer sr = null;

	public enum Mode {
		GoToA,
		GoToB,
		Attack,
		Die
	}

	protected Mode mode = Mode.GoToA;

	// Use this for initialization
	protected void Start () {

		if (MoveBy.x > 0) {
			this.pointA = this.transform.position;
			this.pointB = this.pointA + MoveBy;
		} else {
			this.pointB = this.transform.position;
			this.pointA = this.pointB + MoveBy;
		}

		animator = this.GetComponent<Animator>();
		body = this.GetComponent<Rigidbody2D>();
		sr = this.GetComponent<SpriteRenderer>();
	}

	void FixedUpdate () {
		if (mode == Mode.Die) return;
		checkSetAttackMode ();
		if ((mode == Mode.GoToA || mode == Mode.Attack) && isArrived(pointA)) {
			mode = Mode.GoToB;
		} else if ((mode == Mode.GoToB || mode == Mode.Attack) && isArrived(pointB)) {
			mode = Mode.GoToA;
		}

		move();
		attack ();
	}

	protected virtual void checkSetAttackMode () {
		if (isRabitAttack ()) mode = Mode.Attack;
	}

	public virtual void move () {

		float value= getDirection();
		if (Mathf.Abs(value) > 0) {   
			Vector2 vel = body.velocity;
			vel.x = value * speed;
			body.velocity = vel;
			if (value < 0) sr.flipX = false;
			else sr.flipX = true;

			animator.SetBool("walk", true);
		}
	}

	virtual protected void attack () {
	}

	float getDirection() {
		Vector3 my_pos = this.transform.position;
		if (mode == Mode.GoToA) {
			if (my_pos.x > pointA.x) return -1;
			else return 1;
		} else if (mode == Mode.GoToB) {
			if (my_pos.x < pointB.x) return 1;
			else return -1;
		} else if (mode == Mode.Attack) {
			if (my_pos.x < HeroRabbit.lastRabit.transform.position.x) return 1;
			else return -1;
		}
		return 0;
	}

	virtual protected bool isRabitAttack() {
		return false;
	}

	protected bool isRabitWin() {
		Vector3 rabit_pos = HeroRabbit.lastRabit.transform.position;
		Vector3 my_pos = this.transform.position;

		return Mathf.Abs(rabit_pos.y) > Mathf.Abs(my_pos.y) && Mathf.Abs(rabit_pos.x - my_pos.x) < 1f;
	}

	virtual protected bool isRabitClose() {
		Vector3 rabit_pos = HeroRabbit.lastRabit.transform.position;
		Vector3 my_pos = this.transform.position;
		if (!HeroRabbit.lastRabit.isBigRabit()) {
			return Mathf.Abs(rabit_pos.x - my_pos.x) < 2f && (Mathf.Abs(rabit_pos.y) - Mathf.Abs(my_pos.y))<1.7f;
		} else {
			return Mathf.Abs(rabit_pos.x - my_pos.x) < 2f && (Mathf.Abs(rabit_pos.y) - Mathf.Abs(my_pos.y)) < 2f;
		}
	}

	protected bool isArrived(Vector3 target) { 
		return Mathf.Abs(this.transform.position.x-target.x) < 0.1f; 
	}

	protected IEnumerator die() {
		if (mode == Mode.Die) {
			animator.SetBool ("die", true);
			animator.SetBool ("attack", false);
			animator.SetBool ("walk", false);

			yield return new WaitForSeconds(0.8f);
			Destroy(this.gameObject);
		}
	}
}
