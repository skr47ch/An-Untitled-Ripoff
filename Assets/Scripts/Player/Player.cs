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
	float velocityXSmoothing;
	float health = 100;
	bool checkHealth = true;

	Controller2D controller;
	Rigidbody2D playerRigidBody;

	void Start() {
		controller = GetComponent<Controller2D> ();
		playerRigidBody = GetComponent<Rigidbody2D> ();
		playerRigidBody.isKinematic = true;

		maxJumpHeight = startingJumpHeight;
		CalculateJump();
	}

	void Update() {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
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

				if (input.x != wallDirX && input.x != 0) {
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

		if (Input.GetButtonDown("Jump")) {
			if (wallSliding) {
				if (wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (input.x == 0) {
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
		if (Input.GetButtonUp("Jump")) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}


		velocity.y += gravity * Time.deltaTime;
		velocity.y = Mathf.Clamp(velocity.y, -3.5f, 6.0f);
		controller.Move (velocity * Time.deltaTime, input);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

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
//		Debug.Log(collideObject);
		if(collideObject.gameObject.CompareTag("Enemy")) {
			if(checkHealth && collideObject.gameObject.name == "Enemy_Slug") {
				health -= 10;
				Debug.Log(health);
				checkHealth = false;
				yield return new WaitForSeconds(1f);
				checkHealth = true;
			}
		}
	}
}