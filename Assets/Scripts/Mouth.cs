using UnityEngine;
using System.Collections.Generic;

public class Mouth : MonoBehaviour {

	public GameObject[] FoodPrefabs;
	public Transform foodSpawnPosition;
	public float baseTimeBeweenSpawns = 5f;

	[Range(0, 1)]
	public float spawnTimeVariation = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
