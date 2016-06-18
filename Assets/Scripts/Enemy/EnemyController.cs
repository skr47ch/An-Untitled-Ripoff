using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject enemyPrefab;
	Camera mainCamera;
	GameObject spawnedObject;
	Player player;
	Enemy_Slugs slug;
	Boss_Grotto bossGrotto;
	Boss_NightWalk bossNightWalk;

	bool canSpawn = false;
	bool damagePlayer = true;
	bool damageBoss = true;
	bool dead = false;

	void Start () {
		player = FindObjectOfType<Player>();
		mainCamera = Camera.main;
		SpawnEnemy();
	}

	void Update () {

		CheckIfBossDead();
		CheckPlayerCollision();

		Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
		bool keep = (screenPoint.x > 0) && (screenPoint.y > 0) && (screenPoint.x < 1) && (screenPoint.y < 1);

		if(!keep) {
			Destroy(spawnedObject);
			canSpawn = true;
		}
		if(keep && canSpawn && !dead) SpawnEnemy();
	}

	void CheckIfBossDead() {
		if(spawnedObject) {
			if(spawnedObject.name == "Boss_Grotto") {
				if(bossGrotto.bossLives < 1) {
					Destroy(spawnedObject);
					dead = true;
				}
			}
			else if(spawnedObject.name == "Boss_NightWalk") {
				if(bossNightWalk.bossLives < 1) {
					Destroy(spawnedObject);
					dead = true;
				}
			}
		}
	}

	void SpawnEnemy() {
		if(transform.tag == "Boss") {
			if(transform.name == "BossGrotto") {
				spawnedObject = (GameObject) Instantiate(enemyPrefab, transform.position, transform.rotation);
				spawnedObject.name = "Boss_Grotto";
				bossGrotto = spawnedObject.GetComponent<Boss_Grotto>();
			}
			else if(transform.name == "BossNightWalk") {
				spawnedObject = (GameObject) Instantiate(enemyPrefab, transform.position, transform.rotation);
				spawnedObject.name = "Boss_NightWalk";
				bossNightWalk = spawnedObject.GetComponent<Boss_NightWalk>();
			}
		}

		if(transform.name == "Slug") {
			spawnedObject = (GameObject) Instantiate(enemyPrefab, transform.position, transform.rotation);
			spawnedObject.name = "Enemy_Slug";
			slug = spawnedObject.GetComponent<Enemy_Slugs>();
		}
		canSpawn = false;
	}

	void CheckPlayerCollision() {
		float sLeft, sRight, sTop, sBottom, pLeft, pRight, pTop, pBottom;
		sLeft = sRight = sTop = sBottom = -1;
		pLeft = pRight = pTop = pBottom = 0;

		if(spawnedObject) {
			if(spawnedObject.name == "Enemy_Slug") {
				sLeft = slug.raycastOrigins.left;
				sRight = slug.raycastOrigins.right;
				sTop = slug.raycastOrigins.top;
				sBottom = slug.raycastOrigins.bottom;
			}
			else if(spawnedObject.name == "Boss_Grotto") {
				sLeft = bossGrotto.raycastOrigins.left;
				sRight = bossGrotto.raycastOrigins.right;
				sTop = bossGrotto.raycastOrigins.top;
				sBottom = bossGrotto.raycastOrigins.bottom;
			}

			if(player) {
				pLeft = player.controller.raycastOrigins.left;
				pRight = player.controller.raycastOrigins.right;
				pTop = player.controller.raycastOrigins.top;
				pBottom = player.controller.raycastOrigins.bottom;
			}

			if(damagePlayer) {
				if((sLeft <= pLeft || sLeft <= pRight) && (sRight >= pLeft || sRight >= pRight)){
					if(sTop >= pBottom && sTop <= pTop && player.velocity.y < 0) {

						if(player.jumpButtonPressed) player.velocity.y = player.maxOnAttackJumpVelocity;
						else player.velocity.y = player.minOnAttackJumpVelocity;

						if(slug) Destroy(spawnedObject);
						if(bossGrotto && damageBoss) StartCoroutine (ReduceBossLife());
					}
					else if((sBottom <= pBottom || sBottom <= pTop) && (sTop >= pBottom || sTop >= pTop)) {
						if(damagePlayer) StartCoroutine (DecreaseHealthAfter(1f));
					}
				}
			}
		}
	}

	IEnumerator DecreaseHealthAfter(float delay) {
		damagePlayer = false;
		player.currentHealth -= 10;
		yield return new WaitForSeconds(delay);
		damagePlayer = true;
	}

	IEnumerator ReduceBossLife() {
		damageBoss = false;
		damagePlayer = false;


		if(bossGrotto.bossLives >= 1) bossGrotto.bossLives -= 1;

		yield return new WaitForSeconds(1f);
		damagePlayer = true;

		yield return new WaitForSeconds(1f);
		damageBoss = true;
	}
}
