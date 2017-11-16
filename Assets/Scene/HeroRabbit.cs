using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabbit : MonoBehaviour {
	public float speed = 1;

	bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;
	Rigidbody2D myBody = null;
	Animator animator;
	Transform heroParent = null;
	// Use this for initialization


	bool isBig = false;
	bool isDead = false;


	public static HeroRabbit lastRabit = null;

	void Awake() {
		lastRabit = this;
	}

		void Start () {
		this.heroParent = this.transform.parent;
			myBody = this.GetComponent<Rigidbody2D> (); 
			//class HeroRabit, void Start()
			//Зберігаємо позицію кролика на початку
			LevelController.current.setStartPosition (transform.position);

			animator = GetComponent<Animator> ();
		}

		// Update is called once per frame
		void Update () {

		}

		void FixedUpdate (){
			float value = Input.GetAxis ("Horizontal");
			if (Mathf.Abs (value) > 0) {
				Vector2 vel = myBody.velocity;
				vel.x = value * speed;
				myBody.velocity = vel;
			}


			SpriteRenderer sr = GetComponent<SpriteRenderer> ();
			if (value < 0) {
				sr.flipX = true;
			} else if (value > 0) {
				sr.flipX = false;
			}



			
			if(Mathf.Abs(value) > 0) {
				animator.SetBool ("run", true);
			} else {
				animator.SetBool ("run", false);
			}

			//class HeroRabit, void FixedUpdate()
			Vector3 from = transform.position + Vector3.up * 0.3f;
			Vector3 to = transform.position + Vector3.down * 0.1f;
			int layer_id = 1 << LayerMask.NameToLayer ("Ground");
			//Перевіряємо чи проходить лінія через Collider з шаром Ground
			RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
			if(hit) {
				isGrounded = true;
			} else {
				isGrounded = false;
			}
			//Намалювати лінію (для розробника)
			Debug.DrawLine (from, to, Color.red);

			//HeroRabit::FixedUpdate
			//Якщо кнопка тільки що натислась
			if(Input.GetAxis ("Vertical")>0&& isGrounded) {
				this.JumpActive = true;
			}
			if (this.JumpActive) {
				//Якщо кнопку ще тримають
				if (Input.GetAxis ("Vertical")>0
				) {
					this.JumpTime += Time.deltaTime;
					if (this.JumpTime < this.MaxJumpTime) {
						Vector2 vel = myBody.velocity;
						vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
						myBody.velocity = vel;
					}
				} else {
					this.JumpActive = false;
					this.JumpTime = 0;
				}
			}

			if(this.isGrounded) {
				animator.SetBool ("jump", false);
			} else {
				animator.SetBool ("jump", true);
			}

		if(hit) {
			isGrounded = true;
			if(hit.transform != null && hit.transform.GetComponent<MovingPlatform>() != null){
				//Приліпаємо до платформи
				setNewParent(hit.transform);
			}
		} else {
			isGrounded = false;
			setNewParent (heroParent);
		}
		if (isDead) {
			LevelController.current.onRabitDeath (this);
			isDead = false;
			animator.SetBool ("die", false);

		}
		}
	void setNewParent(Transform newParent) {
		if (this.transform.parent != newParent) {
			//Засікаємо позицію у Глобальних координатах
			Vector3 pos = this.transform.position;

			//Встановлюємо нового батька
			this.transform.parent = newParent;

			//Після зміни батька координати кролика зміняться
			//Оскільки вони тепер відносно іншого об’єкта
			//повертаємо кролика в ті самі глобальні координати
			this.transform.position = pos;
		}
	}


	public bool beBig(bool big) {
		if (big && !this.isBig) {
			this.isBig = true;
			this.transform.localScale += new Vector3 (0.5f, 0.5f, 0);
			return true;
		} else if (!big && this.isBig) {
			this.isBig = false;
			this.transform.localScale -= new Vector3 (0.5f, 0.5f, 0);
			return true;
		}
		return false;
	}

	public void die (Transform die) {
		
		if (die!= null) 
			animator.SetBool ("die", true);

		isDead = true;
	}
	public bool isBigRabit () {
		return isBig;
	}

	public bool isDeadRabit () {
		return isDead;
	}
}