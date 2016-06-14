using UnityEngine;
using System.Collections;

public class Boss_Grotto : MonoBehaviour {

	public LayerMask collisionMask;
	public float maxYSpeed = 2f;
	public float maxXSpeed = 0.05f;
	public float upLimit = -2.0f;
	float ySpeed;
	float xSpeed;
	private float tempYCoord;
	
	Renderer rend;
	public RaycastOrigins raycastOrigins;

	int directionY = -1;
	int directionX = -1;

	float rayLength;
	int numberOfRays = 3;
	float nextRayDistance;
	Vector2 velocity;

	bool moveTime = false;
	bool checkPlayer;
	public int bossLives = 3;

	public float raduisToCheck = 3f;

	void Start () {
		rend = GetComponent<Renderer>();
		UpdateRayCastOrigins();
		nextRayDistance = raycastOrigins.width / (numberOfRays-1);
		ySpeed = maxYSpeed;
		directionY = -1;
		xSpeed = 0;
		checkPlayer = true;
	}

	void Update() {
		UpdateRayCastOrigins();
		rayLength = ySpeed * Time.deltaTime;
		Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
		for(int i = 0; i<numberOfRays; i++) {
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength * 6 ,Color.green);
			if (directionY == 1 && (transform.position.y > upLimit || hit.fraction > 0)) {
				directionY = -1;
				break;
			}
			if(directionY == -1 && hit.fraction > 0) {
				ySpeed = 0f;
				xSpeed = 0f;
				break;
			}
			rayOrigin.x += nextRayDistance;
		}

		transform.Translate(directionX * xSpeed * Time.deltaTime, directionY * ySpeed * Time.deltaTime, 0f);

		if(checkPlayer) CheckIfPlayerIsNear(raduisToCheck);
		if(moveTime) {
			BossMove();
			StartCoroutine(trackMoveTime(6f));
		}

	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		public float top, bottom, left, right;
		public float width, height;
	}

	IEnumerator trackMoveTime(float delay) {
		moveTime = false;
		yield return new WaitForSeconds(delay);
		moveTime = true;
	}

	void CheckIfPlayerIsNear(float radius) {
		Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius);

		if(hitCollider.CompareTag("Player")) {
			moveTime = true;
			checkPlayer = false;
		}
	}

	void BossMove() {
		Debug.Log("BossMove");
		ySpeed = maxYSpeed;
		xSpeed = maxXSpeed;
		directionY = 1;
		directionX *= -1;
	}

	void UpdateRayCastOrigins() {
		Bounds bounds = rend.bounds;
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
		raycastOrigins.top = bounds.max.y;
		raycastOrigins.bottom = bounds.min.y;
		raycastOrigins.left = bounds.min.x;
		raycastOrigins.right = bounds.max.x;
		raycastOrigins.width = bounds.max.x - bounds.min.x;
		raycastOrigins.height = bounds.max.y - bounds.min.y;
	}
}