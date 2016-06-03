using UnityEngine;
using System.Collections;

public class BottomCollisions : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collideObject) {
		if(collideObject.CompareTag("Enemy")) {
			Destroy(collideObject.gameObject);
		}
	}
}
