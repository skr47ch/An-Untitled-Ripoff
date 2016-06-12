using UnityEngine;
using System.Collections;

public class GetObjectBounds : MonoBehaviour {

	RaycastOrigins raycastOrigins;
	
	// Update is called once per frame
	void Update () {
	
	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		public float top, bottom, left, right;
		public float width, height;
	}

	public void UpdateRayCastOrigins(GameObject gameObject) {
		Renderer rend = gameObject.GetComponent<Renderer>();
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
