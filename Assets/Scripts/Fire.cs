using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float speed = 2.5f;
	public float lifeTime = 2.0f;
	public bool resetTime = false;

	void Start () {
	
	}

	void Update () {
		transform.Translate(Vector2.left * speed * Time.deltaTime);
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
}
