using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D playerRigidBody;
	public Transform checkGround;
	public LayerMask whatIsGround;
	public bool isSpinoff = false;

	public float maxSpeed = 5f;
	public float groundRadius = 0.1f;
	public float jumpForce = 500f;
//	public float dieForce = 500f;
	private int ExchangePoints = 0;
	private bool GroundColor = false;   //0->Blue, 1->Red

//	bool isFacingRight = true;
	bool isGrounded = false;
//	bool isDead = false;
//	bool isRestart = false;

	int direction;
	int health = 1;

	void Start() {
		//playerRigidBody = GetComponent<Rigidbody2D> ();
		//transform = GetComponent<Transform> ();
		//anim = GetComponent<Animator> ();
	}

	void FixedUpdate() {
		Movement ();
	}

	void Update() {

//		if(isFacingRight) direction = 1; 
//		else direction = -1;
		if(ExchangePoints < 0) Dead();
		Debug.Log(ExchangePoints);
		CheckJump();

//		if(Input.GetKeyDown(KeyCode.R)) {
//			anim.SetTrigger("Restart");
//			health = 1;
//			//anim.SetBool("Dead", false);
//		}
	}

	void Movement() {

		if(health == 0) return;

		isGrounded = Physics2D.OverlapCircle (checkGround.position, groundRadius, whatIsGround);
//		anim.SetBool ("isGrounded", isGrounded);

		float move = Input.GetAxis ("Horizontal");
		playerRigidBody.velocity = new Vector2 (move * maxSpeed, playerRigidBody.velocity.y);

//		anim.SetFloat("Speed", Mathf.Abs(move));
//		anim.SetFloat ("vSpeed", playerRigidBody.velocity.y);

//		if (move > 0 && !isFacingRight)
//			Flip ();
//		else if (move < 0 && isFacingRight)
//			Flip ();
	}

	void CheckJump() {
//		if(health == 0) return;
//		Debug.Log(isGrounded);
		if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))){
			Debug.Log("Jumping");
//			anim.SetBool ("isGrounded", false);
			playerRigidBody.AddForce(new Vector2(0, jumpForce));
		}
	}

	void OnCollisionEnter2D(Collision2D collide){
		if(!GroundColor && collide.gameObject.CompareTag("RedGround")){
			GroundColor = true;
			ExchangePoints -= 1;
		}
		if(GroundColor && collide.gameObject.CompareTag("BlueGround")){
			GroundColor = false;
			ExchangePoints -= 1;
		}

//		if(collide.gameObject.CompareTag("MovingPlatforms")){
//			this.transform.parent = collide.transform;
//		}
	}

	void OnTriggerEnter2D(Collider2D collide){
		//Debug.Log("Hello");
		if(collide.gameObject.CompareTag("ExchangePoints")){
			Destroy(collide.gameObject);
			ExchangePoints += 1;
		}
	}

//	void OnCollisionExit2D(Collision2D collide){
//		if(collide.gameObject.CompareTag("MovingPlatforms")){
//			this.transform.parent = null;
//		}
//	}
	void Dead() {
//		anim.SetTrigger("Dead");
		Debug.Log("Dead");
		Destroy(this.gameObject);
//		health = 0;
		//playerRigidBody.AddForce(new Vector2((direction * dieForce), 0));	//not working

	}


//	void Flip() {
//		//Transform transform = GetComponent<Transform> ();
//		isFacingRight = !isFacingRight;
//		Vector3 theScale = transform.localScale;
//		theScale.x *= -1;
//		transform.localScale = theScale;
//	}
//
}
