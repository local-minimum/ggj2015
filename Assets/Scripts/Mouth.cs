using UnityEngine;
using System.Collections.Generic;

public class Mouth : MonoBehaviour {

	public GameObject[] FoodPrefabs;
	public Transform foodSpawnPosition;
	public float baseTimeBeweenSpawns = 5f;
	public float eatingTime = 3f;
	private Animator anim;

	[Range(0, 1)]
	public float spawnTimeVariation = 0.1f;

	private float nextSpawn;

	// Use this for initialization
	void Start () {
		SetNextSpawnTime();
	}
	
	// Update is called once per frame
	void Update () {
		if (nextSpawn < Level.timeSinceLevelStart)
			SpawnFood();
	}

	void SetNextSpawnTime() {
		nextSpawn = baseTimeBeweenSpawns * (1 + Random.Range(-spawnTimeVariation, spawnTimeVariation)) / Level.currentDifficulty + Level.timeSinceLevelStart;
	}

	void SpawnFood() {
		GameObject newFood = GenerateFood();
		StartCoroutine(AnimateEating(newFood));
		SetNextSpawnTime();
	}

	GameObject GenerateFood() {
		return (GameObject) Instantiate(FoodPrefabs[Random.Range(0, FoodPrefabs.Length)]);
	}

	IEnumerator<WaitForSeconds> AnimateEating(GameObject food) {
		float startTime = Level.timeSinceLevelStart;
		while (Level.timeSinceLevelStart - startTime < eatingTime) {
			food.transform.position = Vector3.Lerp(foodSpawnPosition.transform.position, transform.position, (Level.timeSinceLevelStart - startTime) / eatingTime);
			yield return null;
		}

		if (anim)
			anim.SetTrigger("Eat");
		Level.registerNewFood(food.GetComponent<Food>());

		food.GetComponent<FoodRoomMovement>().GoToStomach();

	}
		            
}
