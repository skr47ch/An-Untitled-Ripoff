using UnityEngine;
using System.Collections;

public class BottomCollisions : MonoBehaviour {

	float velocity;

	void Update() {
		StartCoroutine(currentVelocity(0.1f));
	}

	void OnTriggerEnter2D(Collider2D collideObject) {
		if(collideObject.CompareTag("Enemy") && velocity <= 0f) {
			Destroy(collideObject.gameObject);
		}
	}

	IEnumerator currentVelocity(float delay) {
		float previous;
		float current;

		previous = transform.position.y;

		yield return new WaitForSeconds(delay);
		current = transform.position.y;
		velocity = current - previous;
		if (velocity == 0)
		{
			velocity = 0.000001f;
		}
		velocity = velocity / delay;
		if (velocity < 0.0001f && velocity > -0.0001f)
		{
			velocity = 0;
		}
	}
}
