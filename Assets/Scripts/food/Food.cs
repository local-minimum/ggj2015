﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Food : MonoBehaviour {

	public List<FoodComponent> foodComponents = new List<FoodComponent>();
	public List<Transform> foodComponentPositions = new List<Transform>();
	public List<AudioClip> eatingSounds = new List<AudioClip>();
	[Range(0, 1)]
	public float eatVolume = 0.5f;

	// Use this for initialization
	void Start () {
		VerifyIntegrity();
		InstantiateFoodComponents();
	}

	void VerifyIntegrity() {
		
		if (foodComponents.Count != foodComponentPositions.Count)
			Debug.LogError(string.Format("{0} missmatch in number of food components and positions", gameObject));

	}

	void InstantiateFoodComponents() {
		for (int i=0, l=foodComponents.Count; i<l; i++) {
			FoodComponent instance = ((GameObject) Instantiate(foodComponents[i].gameObject)).GetComponent<FoodComponent>();
			instance.transform.parent = foodComponentPositions[i];
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localScale = Vector3.one;
			foodComponents[i] = instance;
		}

	}

	void Update() {
		if (depleted)
			DieFromDepletion();
	}

	private FoodComponent getFirstNonDepletedComponent(Swarmer.SwarmerTypes foodType) {
		return foodComponents.Where(fc => !fc.depleted && fc.foodType == foodType).FirstOrDefault();
	}

	public bool depleted {
		get {
			return !foodComponents.Where (fc => !fc.depleted).Any();
		}
	}

	public bool isEdible(Swarmer swarmer) {
		return getFirstNonDepletedComponent(swarmer.SwarmerType) != null;
	}

	public void Eat(Swarmer.SwarmerTypes foodType, float biteSize) {
		FoodComponent foodComponent = getFirstNonDepletedComponent(foodType);
		if (foodComponent) {
			foodComponent.Eat(biteSize);
			audio.PlayOneShot(eatingSounds[Random.Range(0, eatingSounds.Count() - 1)], eatVolume);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		other.BroadcastMessage("EatFood", this, SendMessageOptions.DontRequireReceiver);
	}

	void DieFromDepletion() {
		gameObject.SetActive(false);
	}
}
