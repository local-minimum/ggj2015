using UnityEngine;
using System.Collections;

public class FoodRoomMovement : MonoBehaviour {

	RoomProperties room;
	public float turnHomeDistance = 10f;
	public float forceFactor = 40f;
	public float forceFrequency = 1f;
	private float seed;

	public float torqueFrequency = 2f;
	public float torqueFactor = 100f;

	// Use this for initialization
	void Start () {
		room = GetComponentInParent<RoomProperties>();
		seed = Random.value * 1000f;
	}
	
	// Update is called once per frame
	void Update () {
		AddForce();
		AddTorque();
	}

	void AddForce() {
		Vector2 force = new Vector2(Mathf.PerlinNoise(seed, forceFrequency * Level.timeSinceLevelStart) - Level.PerlinValue, Mathf.PerlinNoise(forceFrequency * Level.timeSinceLevelStart, seed) - Level.PerlinValue);
		Vector2 relativePosition = transform.position - room.foodPoint.position;
		
		if (relativePosition.magnitude < turnHomeDistance)
			force = (force + relativePosition * -1).normalized;
		
		rigidbody2D.AddForce(force * Time.deltaTime * forceFactor);
	}

	void AddTorque() {
		rigidbody2D.AddTorque((Mathf.PerlinNoise(seed * 10f, Level.timeSinceLevelStart * torqueFrequency) - Level.PerlinValue) * torqueFactor);
	}
}
