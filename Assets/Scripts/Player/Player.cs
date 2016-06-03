using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (Rigidbody2D))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 1f;
	public float minJumpHeight = 1f;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 2.5f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	float gravity;
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

		if (Input.GetKeyDown (KeyCode.Space)) {
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
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}


		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

	}

	void OnTriggerEnter2D(Collider2D collideObject) {
		Debug.Log(collideObject.name);
		if(collideObject.CompareTag("PowerUp")) {
			if(collideObject.name == "JumpHigher") {
				Destroy(collideObject.gameObject);
				maxJumpHeight += 0.5f;
				timeToJumpApex = Mathf.Sqrt(2 * maxJumpHeight / Mathf.Abs(gravity));
				CalculateJump();
			}
		}
	}

	void CalculateJump() {
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		print ("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
	}

	void OnCollisionStay2D(Collision2D collideObject) {
		if(collideObject.gameObject.CompareTag("Enemy")) {
			if(checkHealth && collideObject.gameObject.name == "Enemy_Slug") {
				health -= 10;
				Debug.Log(health);
				checkHealth = false;
				StartCoroutine(ReduceHealth(1f));
			}
		}
	}

	IEnumerator ReduceHealth(float delay) {
		yield return new WaitForSeconds(delay);
		checkHealth = true;
	}
}