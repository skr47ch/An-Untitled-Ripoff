using UnityEngine;
using System.Collections;

public class PlayerController_RayCast : MonoBehaviour {

	float gravity = 1f;
	float maxSpeed = 5;
	float maxFall = 2;
	float jump = 2;
	float acceleration = 1f;

	int layerMask;
	Rect box;
	Vector2 velocity;
	bool grounded = false;
	bool falling = false;

	int horizontalRays = 6;
	int verticalRays = 4;
	float margin = 0.2f;

	BoxCollider2D collide;
	// Use this for initialization
	void Start () {
		layerMask = LayerMask.NameToLayer("CollisionLayer");
		collide = GetComponent<BoxCollider2D>();
	}

	void FixedUpdate () {
		box = new Rect(
			collide.bounds.min.x,
			collide.bounds.min.y,
			collide.bounds.size.x,
			collide.bounds.size.y
			);

		if(!grounded) {
			velocity = new Vector2(velocity.x, Mathf.Max(velocity.y - gravity, -maxFall));
		}

		if(velocity.y < 0) falling = true;

		if(grounded || falling) {
			Debug.Log("falling =" + falling);

			Vector2 startPoint = new Vector2(box.xMin + margin, box.center.y);
			Vector2 endPoint = new Vector2(box.xMax - margin, box.center.y);

			RaycastHit2D hit;

			// add half box height since we're starting in centre
			float distance = box.height/2 + (grounded ? margin : Mathf.Abs(velocity.y * Time.deltaTime));

			// check if we hit anything
			bool connected = false;

			for (int i = 0; i<verticalRays; i++) {
				
				float lerpAmount = (float)i / (float)verticalRays - 1;
				Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

				Ray2D ray = new Ray2D(origin, Vector2.down);
				Debug.DrawRay( origin, Vector3.down, Color.green );
				hit = Physics2D.Raycast(ray.origin, ray.direction, distance, layerMask);

//				Debug.Log(connected);

				if(hit) {
					grounded = true;
					falling = false;
					Debug.Log("grounded =" + grounded);
					transform.Translate(Vector2.down * (distance - box.height/2));
					velocity = new Vector2(velocity.x, 0);
					break;
				}
			}
//
//			if(!connected) {
//				grounded = false;
//			}
		}


		// ---------------------------------//
		// ---- Lateral Movement -----------//
		// --------------------------------=//

		float horizontalAxis = Input.GetAxisRaw("Horizontal");
		float newVelocityX = velocity.x;

		if(horizontalAxis != 0) {
			newVelocityX  += acceleration * horizontalAxis;
			newVelocityX = Mathf.Clamp(newVelocityX, -maxSpeed, maxSpeed);
		}
		else if(velocity.x != 0) {
			int modifier = velocity.x > 0 ? -1 : 1;
			newVelocityX += acceleration * modifier;
		}
	}

	void LateUpdate () {
		transform.Translate(velocity * Time.deltaTime);
	}
}
