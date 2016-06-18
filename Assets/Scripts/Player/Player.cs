using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (Rigidbody2D))]
public class Player : MonoBehaviour {

	public float moveSpeed = 2.5f;
	public float gravity = -10.0f;

	public float startingJumpHeight = 0.8f;
	public float jumpHeightUpgrade1 = 1.3f;
	public float jumpHeightUpgrade2 = 1.6f;
	public float doubleJumpHeightUpgrade1 = 0.5f;
	public float doubleJumpHeightUpgrade2 = 1.0f;
	public float minJumpHeightOnAttack = 0.5f;
	public float maxJumpHeightOnAttack = 1.0f;

	float maxJumpHeight;
	float maxDoubleJumpHeight;
	public float minJumpHeight = 0.5f;

	public bool jumpButtonDown;
	public bool jumpButtonUp;
	public bool jumpButtonPressed;
	bool doubleJumpEnabled = false;
	bool canDoubleJumpNow = true;

	float maxJumpVelocity;
	float minJumpVelocity;
	public float maxDoubleJumpVelocity;
	public float minOnAttackJumpVelocity;
	public float maxOnAttackJumpVelocity;

	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	public Vector3 velocity;
	float targetVelocityX;
	float velocityXSmoothing;
	public float maxHealth = 100;
	public float currentHealth;

	public Controller2D controller;
	Rigidbody2D playerRigidBody;

	void Start() {
		controller = GetComponent<Controller2D> ();
		playerRigidBody = GetComponent<Rigidbody2D> ();
		playerRigidBody.isKinematic = true;
		currentHealth = maxHealth;
		maxJumpHeight = startingJumpHeight;
		CalculateJump();
	}

	void Update() {
		
		#if UNITY_STANDALONE
			MoveControl(Input.GetAxisRaw ("Horizontal"));
			jumpButtonDown = Input.GetButtonDown("Jump");
			jumpButtonPressed = Input.GetButton("Jump");
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
				Debug.Log("Jump1");
				velocity.y = maxJumpVelocity;
//				canDoubleJumpNow = true;
			}
			else if (doubleJumpEnabled && canDoubleJumpNow) {
				Debug.Log("Jump2");
				velocity.y = maxDoubleJumpVelocity;
				canDoubleJumpNow = false;
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
			canDoubleJumpNow = true;
		}

	}

	public void MoveControl(float thisInput) {
		targetVelocityX = thisInput * moveSpeed;
	}

	void OnTriggerEnter2D(Collider2D collideObject) {
		if(collideObject.CompareTag("PowerUp")) {
			if(collideObject.name == "JumpUpgrade2") {
				Destroy(collideObject.gameObject);
				maxJumpHeight = (maxJumpHeight < jumpHeightUpgrade2) ? jumpHeightUpgrade2 : maxJumpHeight;
			}
			else if(collideObject.name == "JumpUpgrade1") {
				Destroy(collideObject.gameObject);
				maxJumpHeight = (maxJumpHeight < jumpHeightUpgrade1) ? jumpHeightUpgrade1 : maxJumpHeight;
			}
			else if(collideObject.name == "DoubleJumpUpgrade1") {
				doubleJumpEnabled = true;
				Destroy(collideObject.gameObject);
				maxDoubleJumpHeight = (maxDoubleJumpHeight < doubleJumpHeightUpgrade1) ? doubleJumpHeightUpgrade1 : maxDoubleJumpHeight;
			}
			else if(collideObject.name == "DoubleJumpUpgrade2") {
				doubleJumpEnabled = true;
				Destroy(collideObject.gameObject);
				maxDoubleJumpHeight = (maxDoubleJumpHeight < doubleJumpHeightUpgrade2) ? doubleJumpHeightUpgrade2 : maxDoubleJumpHeight;
			}
			CalculateJump();
		}
		if(collideObject.CompareTag("HealthUp")) {
			if(collideObject.name == "HealthUpgrade") {
				Destroy(collideObject.gameObject);
				currentHealth += 10;
				maxHealth += 10;
			}
			if(collideObject.name == "FullHealth") {
				Destroy(collideObject.gameObject);
				currentHealth = maxHealth;
			}
		}
	}

	void CalculateJump() {
		maxJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * maxJumpHeight);
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

		maxDoubleJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs (gravity) * maxDoubleJumpHeight);

		maxOnAttackJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * maxJumpHeightOnAttack);
		minOnAttackJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeightOnAttack);

		print ("Gravity: " + gravity + 
				", Jump1 Height: " + maxJumpHeight + 
				", Jump1 Velocity: " + maxJumpVelocity + 
				", Jump2 Height: " + maxDoubleJumpHeight + 
				", Jump2 Velocity: " + maxDoubleJumpVelocity);
	}
}