﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class RayCastController : MonoBehaviour {

	public LayerMask collisionMask;
	public LayerMask collisionMaskFromBelow;

	public const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing;

	[HideInInspector]
	public BoxCollider2D collider;
	public RaycastOrigins raycastOrigins;

	public virtual void Awake() {
		collider = GetComponent<BoxCollider2D> ();
	}

	public virtual void Start() {
		CalculateRaySpacing ();
	}

	public void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

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

	public void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		public float top, bottom, left, right;
		public float width, height;
	}
}