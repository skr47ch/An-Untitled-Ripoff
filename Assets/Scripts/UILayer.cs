using UnityEngine;
using System.Collections;

public class UILayer : MonoBehaviour {

	Camera thisCamera;

	void Start () {
		thisCamera = Camera.main;
		transform.position = thisCamera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = thisCamera.transform.position;
	}
}
