using UnityEngine;
using System.Collections;

public class PlayerController_RayCast : MonoBehaviour {

	float gravity = 1f;
	float maxSpeed = 10f;
	float maxFall = 15f;
	float jump = 8f;
	float acceleration = 2f;

	public LayerMask layerMask;
	Rect box;
	Vector2 velocity;
	bool grounded = false;
	bool falling = false;

	int horizontalRays = 6;
	int verticalRays = 4;
	int margin = 1;

	bool connected = false;

	BoxCollider2D collide;
	// Use this for initialization
	void Start () {
//		layerMask = LayerMask.NameToLayer("CollisionLayer");
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

			Vector2 startPoint = new Vector2(box.xMin + margin, box.center.y);
			Vector2 endPoint = new Vector2(box.xMax - margin, box.center.y);

			RaycastHit2D hitInfo;

			// add half box height since we're starting in centre
			float distance = box.height/2 + (grounded ? margin : Mathf.Abs(velocity.y * Time.deltaTime));
			// check if we hit anything


			for (int i = 0; i < verticalRays; i++) {
				float lerpAmount = (float)i / (float) (verticalRays - 1);
				Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);

				hitInfo = Physics2D.Raycast(origin, Vector2.down, distance, layerMask);
				Debug.DrawRay(origin, Vector2.down);

				if(hitInfo.fraction > 0) {
					connected = true;
				}
			}
			if(connected) {
				grounded = true;
				falling = false;
				transform.Translate(Vector2.down * (distance - box.height/2));
				velocity = new Vector2(velocity.x, 0);
				Debug.Log("grounded4 =" + grounded);
			}
			else {
				grounded = false;
			}
		}



		// ---------------------------------//
		// ---- Lateral Movement -----------//
		// --------------------------------=//
//
//		float horizontalAxis = Input.GetAxisRaw("Horizontal");
//		float newVelocityX = velocity.x;
//
//		if(horizontalAxis != 0) {
//			newVelocityX  += acceleration * horizontalAxis;
//			newVelocityX = Mathf.Clamp(newVelocityX, -maxSpeed, maxSpeed);
//		}
//		else if(velocity.x != 0) {
//			int modifier = velocity.x > 0 ? -1 : 1;
//			newVelocityX += acceleration * modifier;
//		}
	}

	void LateUpdate () {
		transform.Translate(velocity * Time.deltaTime * 0.1f);
//		Debug.Log(velocity);
	}
}
