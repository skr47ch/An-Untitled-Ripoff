using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Transform thisTransform;
	public Rigidbody2D thisRigidbody;

	public float jumpForce = 100;
	public bool isGrounded = true;

	// Use this for initialization
	void Start () {
		thisRigidbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Jump")){
			thisRigidbody.AddForce(new Vector2(0, jumpForce));
			isGrounded = false;
		}
	}
		
}
