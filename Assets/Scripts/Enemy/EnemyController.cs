using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public GameObject enemyPrefab;
	Camera mainCamera;
	GameObject spawnedObject;
	bool canSpawn = false;

	void Start () {
		mainCamera = Camera.main;
		spawnedObject = (GameObject) Instantiate(enemyPrefab, transform.position, transform.rotation);
	}

	void Update () {
		Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
		bool keep = (screenPoint.x > 0) && (screenPoint.y > 0) && (screenPoint.x < 1) && (screenPoint.y < 1);

		if(!keep && spawnedObject != null) {
			Destroy(spawnedObject);
			canSpawn = true;
		}
		if(keep && spawnedObject == null && canSpawn) {
			spawnedObject = (GameObject) Instantiate(enemyPrefab, transform.position, transform.rotation);
			canSpawn = false;
		}
		if(!keep) {
			canSpawn = true;
		}
	}
}
