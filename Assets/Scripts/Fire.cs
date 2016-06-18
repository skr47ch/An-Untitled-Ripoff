using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float speed = 2.5f;
	public int direction = -1;
	public float lifeTime = 2.0f;
	public bool resetTime = false;
	Player player;
	Rigidbody2D myRigidBody;

	void Start () {
		player = FindObjectOfType<Player>();
		myRigidBody = GetComponent<Rigidbody2D>();
		myRigidBody.gravityScale = 0;
	}

	void Update () {
		transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
		StartCoroutine(Destroyer());
	}

	IEnumerator Destroyer () {
		yield return new WaitForSeconds(lifeTime);
		if(!resetTime) {
			Destroy(gameObject);
		}
		else {
			yield return new WaitForSeconds(1f);
			resetTime = false;
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {

		if(this.CompareTag("Fire")) {
			if(collider.CompareTag("Player")) {
				player.currentHealth -= 10;
				Destroy(gameObject);
			}
		}
		else if (this.CompareTag("BackFire")) {
			if(collider.CompareTag("Player")) {
				direction *= -1;
				resetTime = true;
			}
//			else if(collider.CompareTag("Boss")) {
//				Debug.Log("BossCollide");
//				Destroy(gameObject);
//			}
		}
	}
}
