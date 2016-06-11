using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (Rigidbody2D))]
public class Player : MonoBehaviour {

	public float moveSpeed = 2.5f;
	public float gravity = -10.0f;
	public float startingJumpHeight = 0.8f;
	public float jumpHeightAfterUpgrade1 = 1.3f;
	public float jumpHeightAfterUpgrade2 = 1.6f;

	float maxJumpHeight;
	public float minJumpHeight = 0.5f;
	public bool jumpButtonDown, jumpButtonUp;

	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float targetVelocityX;
	float velocityXSmoothing;
	float maxHealth = 100;
	public float currentHealth;
	bool checkHealth = true;

	Controller2D controller;
	Rigidbody2D playerRigidBody;

	int bossLife;

	void Start() {
		controller = GetComponent<Controller2D> ();
		playerRigidBody = GetComponent<Rigidbody2D> ();
		playerRigidBody.isKinematic = true;
		currentHealth = maxHealth;
		maxJumpHeight = startingJumpHeight;
//		playerRigidBody.gravityScale = 0;
		bossLife = 3;
		CalculateJump();
	}

	void Update() {

		if(controller.currentCollision != null && !controller.currentCollision.CompareTag("Ground")) {
			Debug.Log(controller.currentCollision.name);

			if(controller.currentCollision != null && controller.currentCollision.CompareTag("Enemy")) {
				Destroy(controller.currentCollision);
				controller.currentCollision = null;
			}

			if(controller.currentCollision != null && controller.currentCollision.CompareTag("Boss")) {
				if(bossLife < 0) {
					Destroy(controller.currentCollision);
					controller.currentCollision = null;	
				}
				bossLife -= 1;
				controller.currentCollision = null;	
				Debug.Log("Ouch :" + (4-bossLife));
//				break;
			}
		}
		
		#if UNITY_STANDALONE
			MoveControl(Input.GetAxisRaw ("Horizontal"));
			jumpButtonDown = Input.GetButtonDown("Jump");
			jumpButtonUp = Input.GetButtonUp("Jump");
		#endif

		int wallDirX = (controller.collisions.left) ? -1 : 1;

		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (Mathf.Sign(targetVelocityX) != wallDirX /*&& input.x != 0*/) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

		if (jumpButtonDown) {
			if (wallSliding) {
				if (wallDirX == Mathf.Sign(targetVelocityX)) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (Mathf.Sign(targetVelocityX) == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
				else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
			}
		}
		if (jumpButtonUp) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}

//		Debug.Log(velocity.y);
		velocity.y += gravity * Time.deltaTime;
		velocity.y = Mathf.Clamp(velocity.y, -3.5f, 6.0f);
		controller.Move (velocity * Time.deltaTime, new Vector2(Mathf.Sign(targetVelocityX), 0));

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

	}

	public void MoveControl(float thisInput) {
		targetVelocityX = thisInput * moveSpeed;
	}

	void OnTriggerEnter2D(Collider2D collideObject) {
//		Debug.Log(collideObject.name);
		if(collideObject.CompareTag("PowerUp")) {
			if(collideObject.name == "JumpUpgrade2") {
				Destroy(collideObject.gameObject);
				maxJumpHeight = (maxJumpHeight < jumpHeightAfterUpgrade2) ? jumpHeightAfterUpgrade2 : maxJumpHeight;
				CalculateJump();
			}
			else if(collideObject.name == "JumpUpgrade1") {
				Destroy(collideObject.gameObject);
				maxJumpHeight = (maxJumpHeight < jumpHeightAfterUpgrade1) ? jumpHeightAfterUpgrade1 : maxJumpHeight;
				CalculateJump();
			}
		}
	}

	void CalculateJump() {
		maxJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * maxJumpHeight);
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		print ("Gravity: " + gravity + "  Jump Height: " + maxJumpHeight + "  Jump Velocity: " + maxJumpVelocity);
	}

	IEnumerator OnTriggerStay2D(Collider2D collideObject) {
		if(collideObject != null && collideObject.gameObject.CompareTag("Enemy")) {
			
			if(checkHealth && collideObject.gameObject.name == "Enemy_Trigger") {
				currentHealth -= 10;
				checkHealth = false;
				yield return new WaitForSeconds(1f);
				checkHealth = true;
			}
		}
		if(collideObject != null && collideObject.gameObject.CompareTag("Boss")) {

			if(checkHealth && collideObject.gameObject.name == "Boss_Grotto_Trigger") {
				currentHealth -= 20;
				checkHealth = false;
				yield return new WaitForSeconds(1f);
				checkHealth = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collideObject) {
		Debug.Log("Collided :" + collideObject.gameObject.name);
		if(collideObject.gameObject.CompareTag("Enemy")) {
			if(collideObject.gameObject.name == "Enemy_Slug") {
				Destroy(collideObject.gameObject);
				Debug.Log("Destroyed");
			}
		}
		if(collideObject.gameObject.CompareTag("Boss")) {
			if(collideObject.gameObject.name == "Boss_Grotto") {
				bossLife -= 1;
				Debug.Log(bossLife);
				if(bossLife <= 0) {
					Destroy(collideObject.gameObject);
					Debug.Log("Destroyed");
				}
			}
		}
	}
}