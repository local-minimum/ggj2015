using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inflammation : MonoBehaviour {

	public GameObject inflammationEffectsHolder;
	public Material[] inflammationParticleMaterials;

	private RoomProperties room;

	public Swarmer.SwarmerTypes[] inflamationTypes;
	private Swarmer.SwarmerTypes inflamationType;

	public float averageBetweenInflammationTime = 10f;
	public float betweenInflammationTimeVariation = 5f;
	private float nextInflammation;
	private float inflamationStart;
	private bool inflamated = false;

	// Use this for initialization
	void Start () {
		room = GetComponent<RoomProperties>();
		SetNextInflamation();
	}
	
	// Update is called once per frame
	void Update () {
		if (inflamated) {

		} else if (IsNewOutBreak()) {

		}
	}

	void SetOutbreak() {
		inflamationType = inflamationTypes[Random.Range(0, inflamationTypes.Length)];
		inflamationStart = Level.timeSinceLevelStart;
		ActivateEffects();
		inflamated = true;
	}

	Material getInflammationEffectMaterial() {
		return inflammationParticleMaterials[System.Array.IndexOf(inflamationTypes, inflamationType)];
	}

	void ActivateEffects() {
		foreach (ParticleSystem particleSystem in inflammationEffectsHolder.GetComponentsInChildren<ParticleSystem>()) {
			particleSystem.renderer.material = getInflammationEffectMaterial();
			particleSystem.Play();
		}
	}

	bool IsNewOutBreak() {
		return nextInflammation < Level.timeSinceLevelStart;
	}

	void SetNextInflamation() {
		nextInflammation = Level.timeSinceLevelStart + averageBetweenInflammationTime + Random.Range(-betweenInflammationTimeVariation, betweenInflammationTimeVariation);
	}
}
