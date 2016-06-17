using UnityEngine;
using System.Collections;

public class Boss_NightWalk : MonoBehaviour {

	public Transform pos1, pos2, pos3, pos4;
	float xDistance = 0.5f;
	Vector2 newPosition;
	bool moveTime = false;
	bool checkPlayer = true;
	public float moveInterval = 3.0f;
	public float initialWait = 2.0f;
	public float raduisToCheck = 3.0f;

	void Start () {
		transform.position = RandomPosition();
	}
	
	// Update is called once per frame
	void Update () {
		if(moveTime) {
			BossMove();
			StartCoroutine(TrackMoveTime(moveInterval));
		}

		if(checkPlayer) CheckIfPlayerIsNear(raduisToCheck);
	}

	void BossMove() {
		transform.position = RandomPosition();
	}

	IEnumerator TrackMoveTime (float delay) {
		moveTime = false;
		yield return new WaitForSeconds(delay);
		moveTime = true;
	}

	void CheckIfPlayerIsNear(float radius) {
		Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius);

		if(hitCollider.CompareTag("Player")) {
			checkPlayer = false;
			Debug.Log("Player Found");
			StartCoroutine(BeginBoss());
		}
	}

	IEnumerator BeginBoss() {
		yield return new WaitForSeconds(initialWait);
		moveTime = true;
	}

	Vector2 RandomPosition () {
		int newRand = Random.Range(1, 5);

		switch (newRand) {
		case 1:	
			newPosition = pos1.position;
			break;
		case 2:
			newPosition = pos2.position;
			break;
		case 3:
			newPosition = pos3.position;
			break;
		case 4:
			newPosition = pos4.position;
			break;
		default :
			newPosition = pos2.position;
			break;
		}
		Debug.Log("Move " + newRand);
		newPosition.x += xDistance;
		return newPosition;
	}
}
