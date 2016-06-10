using UnityEngine;
using System.Collections;

public class Enemy_Slugs : MonoBehaviour {

	public LayerMask collisionMask;
	public float xSpeed = 4f;
	private float tempXCoord;

//	public BoxCollider2D collider2D;
	Renderer rend;
	RaycastOrigins raycastOrigins;

	int directionX = 1;
	float rayLength;
	Vector2 velocity;

	void Start () {
//		collider2D = GetComponent<BoxCollider2D>();
		rend = GetComponent<Renderer>();
	}
		
	void Update() {
		UpdateRayCastOrigins();
		rayLength = xSpeed * Time.deltaTime;
		Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
		for(int i = 0; i<2; i++) {
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength * 6 ,Color.green);
			if (hit.fraction > 0) {
					directionX *= -1;
					break;
			}
			rayOrigin.y += raycastOrigins.height;
		}

		transform.Translate(Vector2.right * directionX * xSpeed * Time.deltaTime);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		public float top, bottom, left, right;
		public float width, height;
	}

	void UpdateRayCastOrigins() {
//		Bounds bounds = collider2D.bounds;
		Bounds bounds = rend.bounds;
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
//		raycastOrigins.top = bounds.max.y;
//		raycastOrigins.bottom = bounds.min.y;
//		raycastOrigins.left = bounds.min.x;
//		raycastOrigins.right = bounds.max.x;
//		raycastOrigins.width = bounds.max.x - bounds.min.x;
		raycastOrigins.height = bounds.max.y - bounds.min.y;
	}
}
