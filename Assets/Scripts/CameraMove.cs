using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	private Bounds camerabound;
	private Vector3 newCameraPosition;
	public Transform player;
	private Camera thisCamera;
	// Use this for initialization
	void Start () {
		thisCamera = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Debug.Log("(" + transform.position.x + " , " + transform.position.y + ")");
		camerabound = CameraExtensions.OrthographicBounds(thisCamera);

		if(player.transform.position.x > camerabound.max.x) 	MoveCamera(camerabound.size.x, 0);
		if(player.transform.position.x < camerabound.min.x) 	MoveCamera(-camerabound.size.x, 0);
		if(player.transform.position.y > camerabound.max.y) 	MoveCamera(0, camerabound.size.y);
		if(player.transform.position.y < camerabound.min.y) 	MoveCamera(0, -camerabound.size.y);	
	}

	void MoveCamera(float xBound, float yBound) {
		newCameraPosition = thisCamera.transform.position;
		newCameraPosition.x = thisCamera.transform.position.x + xBound;
		newCameraPosition.y = thisCamera.transform.position.y + yBound;
		thisCamera.transform.position = newCameraPosition;
	}
}