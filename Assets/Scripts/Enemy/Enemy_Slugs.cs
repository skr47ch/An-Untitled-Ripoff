using UnityEngine;
using System.Collections;

public class Enemy_Slugs : MonoBehaviour {

	public LayerMask collisionMask;
	public float xSpeed = 4f;
	private float tempXCoord;

	BoxCollider2D collider2D;
	RaycastOrigins raycastOrigins;

	int directionX = 1;
	float rayLength;
	Vector2 velocity;

	void Start () {
		collider2D = GetComponent<BoxCollider2D>();

	}
		
	void Update() {
		UpdateRayCastOrigins();
		rayLength = xSpeed * Time.deltaTime;
		Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
		Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);
		if (hit.fraction > 0) {
				directionX *= -1;
		}

		transform.Translate(Vector2.right * directionX * xSpeed * Time.deltaTime);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	void UpdateRayCastOrigins() {
		Bounds bounds = collider2D.bounds;
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
}
