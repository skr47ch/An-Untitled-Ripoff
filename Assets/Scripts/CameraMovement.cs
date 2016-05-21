using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Camera cameraFollow;
	public float xOffset = 0.5f;
	public float yOffset = 0.1f;

	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
			Vector3 point = cameraFollow.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - cameraFollow.ViewportToWorldPoint(new Vector3(xOffset, yOffset, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

	}
}
