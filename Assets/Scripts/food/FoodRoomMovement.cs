using UnityEngine;
using System.Collections.Generic;

public class FoodRoomMovement : MonoBehaviour {

	RoomProperties room;
	public float turnHomeDistance = 10f;
	public float forceFactor = 40f;
	public float forceFrequency = 1f;
	private float seed;

	public float torqueFrequency = 2f;
	public float torqueFactor = 100f;

	public float foodEffectDuration = 1f;
	bool phasing = false;

	public float roomChangingP = 0.2f;

	// Use this for initialization
	void Start () {
		room = GetComponentInParent<RoomProperties>();
		seed = Random.value * 1000f;
	}
	
	// Update is called once per frame
	void Update () {
		if (ChangeLocation()) {
			GoToRandom();
		} else if (!phasing && room) {
			AddForce();
			AddTorque();
		}
	}

	bool ChangeLocation() {
		return roomChangingP * Time.deltaTime > Random.value;
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

	public void GoToStomach() {
		StartCoroutine(_PhaseOutFood(RoomManager.Instance.stomach));
	}

	public void GoToRandom() {

		StartCoroutine(_PhaseOutFood(RoomManager.GetRandomOtherRoom(room)));
	}

	IEnumerator<WaitForSeconds> _PhaseOutFood(RoomProperties foodTarget) {

		phasing = true;
		float startTime = Level.timeSinceLevelStart;
		while (Level.timeSinceLevelStart - startTime < foodEffectDuration) {
			transform.localScale = Vector3.one * Mathf.Lerp(1, 0, (Level.timeSinceLevelStart - startTime) / foodEffectDuration);
			yield return null;
		}
		transform.localScale = Vector3.zero;
		if (room)
			room.RemoveFood(gameObject);
		foodTarget.AddFood(gameObject);
		room = foodTarget;
		gameObject.BroadcastMessage("PhaseInFood", SendMessageOptions.DontRequireReceiver);
	}

	void PhaseInFood() {
		StartCoroutine(_PhaseInFood());
	}

	IEnumerator<WaitForSeconds> _PhaseInFood() {
		float startTime = Level.timeSinceLevelStart;
		while (Level.timeSinceLevelStart - startTime < foodEffectDuration) {
			transform.localScale = Vector3.one * Mathf.Lerp(0, 1, (Level.timeSinceLevelStart - startTime) / foodEffectDuration);
			yield return null;
		}
		transform.localScale = Vector3.one;
		phasing = false;
	}
}
