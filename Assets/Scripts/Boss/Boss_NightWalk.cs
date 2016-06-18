using UnityEngine;
using System.Collections;

public class Boss_NightWalk : MonoBehaviour {

	public Transform pos1, pos2, pos3, pos4;

	float xDistance = 0.5f;
	Vector2 newPosition;
	bool moveTime = false;
	bool cannonTime = false;
	bool checkPlayer = true;
	bool fireBlue = false;

	public float moveInterval = 4.0f;
	public float fireInterval = 2.0f;
	public float initialWait = 2.0f;
	public float raduisToCheck = 3.0f;

	public Transform firePrefab;
	public Transform BossFirePrefab;
	GameObject newSpawn;
	int newBossRand;

	void Start () {
		transform.position = RandomPosition();
	}
	
	// Update is called once per frame
	void Update () {
		if(moveTime) {
			BossMove();
			StartCoroutine(TrackMoveTime(moveInterval));
		}

		if(cannonTime) {
			FireCannon();
			StartCoroutine(TrackFireTime(fireInterval));
		}

		if(checkPlayer) CheckIfPlayerIsNear(raduisToCheck);
	}

	void FireCannon() {
		int[] array = {2, 2, 3, 3, 3, 3, 3};
		int index = Random.Range(0, array.Length);
		int num = array[index];
		int[] rand = new int[num];

		rand[0] = Random.Range(1, 5);
		rand[1] = Random.Range(1, 5);
		while(rand[1] == rand[0]) {
			rand[1] = Random.Range(1, 5);
		}
		if(num == 3) {
			rand[2] = Random.Range(1, 5);
			while(rand[2] == rand[0] || rand[2] == rand[1]) {
				rand[2] = Random.Range(1, 5);
			}
		}
		Debug.Log(rand[0] + ", " + rand[1]);
		for(int i = 0; i < num ; i++) {
			Object prefab;

			prefab = (rand[i] == newBossRand) ? BossFirePrefab : firePrefab;
			if(rand[i] == newBossRand && fireBlue) {
				prefab = BossFirePrefab;
				fireBlue = false;
			}
			else prefab = firePrefab;

			if(rand[i] == 1) Instantiate(prefab, pos1.position, pos1.rotation);
			else if(rand[i] == 2) Instantiate(prefab, pos2.position, pos2.rotation);
			else if(rand[i] == 3) Instantiate(prefab, pos3.position, pos3.rotation);
			else if(rand[i] == 4) Instantiate(prefab, pos4.position, pos4.rotation);
		}

	}
		

	void BossMove() {
		transform.position = RandomPosition();
	}

	IEnumerator TrackFireTime(float delay) {
		cannonTime = false;
		yield return new WaitForSeconds(delay);
		cannonTime = true;
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
		cannonTime = true;
	}

	Vector2 RandomPosition () {
		newBossRand = Random.Range(1, 5);

		switch (newBossRand) {
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
		fireBlue = true;
		newPosition.x += xDistance;
		return newPosition;
	}
}
