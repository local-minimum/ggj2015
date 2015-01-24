﻿using UnityEngine;
using System.Collections;

public class SwarmerActions : MonoBehaviour {

	bool hovering = false;
	private Swarmer swarmer;

	public float bitingCapacity;
	public float biteSize;

	// Use this for initialization
	void Start () {
		swarmer = GetComponentInParent<Swarmer>();
		if (!swarmer)
			Debug.Log(string.Format("{0} is lacking its swarmer!", gameObject));
	}
	
	// Update is called once per frame
	void Update () {
		if (hovering && Input.GetButtonDown("Fire1"))
			SwarmLeaderController.SetLeader(swarmer);
	}

	void OnMouseEnter() {
		hovering = true;
//		Debug.Log(string.Format("{0} is hovered", gameObject));
	}

	void OnMouseExit() {
		hovering = false;
	}

	bool canBite {
		get {
			return Mathf.Sign(bitingCapacity) == 1;
		}
	}

	float GetBiteSize() {
		float biteSize = Mathf.Clamp(this.biteSize * Time.deltaTime, 0, bitingCapacity);
		bitingCapacity -= biteSize;
		return biteSize;
	}

	void EatFood(Food food) {

		if (canBite) {
			if (food.isEdible(swarmer))
				food.Eat(swarmer.SwarmerType, GetBiteSize());
		} else
			DieFromEating();
	}

	void DieFromEating() {
		SwarmLeaderController.RemoveMeFromPower(swarmer);
		Destroy(gameObject);
	}
}
