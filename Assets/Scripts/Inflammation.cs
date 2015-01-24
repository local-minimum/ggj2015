using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inflammation : MonoBehaviour {

	public GameObject inflammationEffectsHolder;
	public Material[] inflammationParticleMaterials;
	public Transform[] spawningPositions;

	private RoomProperties room;

	public Swarmer.SwarmerTypes[] inflamationTypes;
	private Swarmer.SwarmerTypes inflamationType;

	public float averageBetweenInflammationTime = 10f;
	public float betweenInflammationTimeVariation = 5f;
	private float nextInflammation;
	private float inflamationStart;
	private bool inflamated = false;

	public float swarmerDuplicationP = 0.1f;
	public float spontaneousGenerationP = 0.2f;

	public float spawnOffsetMax = 1f;

	public float noCurePeriod = 2f;


	// Use this for initialization
	void Start () {
		room = GetComponent<RoomProperties>();
		SetNextInflamation();
	}
	
	// Update is called once per frame
	void Update () {
		if (inflamated) {
			SpontaneousGeneration();
			SwarmerDuplication();
			TestCured();
		} else if (IsNewOutBreak()) {
			SetOutbreak();
		}
	}

	void SpontaneousGeneration() {
		if (Random.value < spontaneousGenerationP * Time.deltaTime)
			NewSpawn(spawningPositions[Random.Range(0, spawningPositions.Length)].position);

	}

	void SwarmerDuplication() {
		Swarmer[] swarmers = room.GetSwarmerByType(inflamationType).ToArray();
		foreach (Swarmer swarmer in swarmers) {
			if (Random.value < swarmerDuplicationP * Time.deltaTime)
				NewSpawn(swarmer.transform.position);

		}
	}

	void NewSpawn(Vector3 position) {
		if (Level.IsAvailable(inflamationType)) {
			Swarmer swarmer = Level.GetSwarmer(inflamationType);
			swarmer.transform.position = position + new Vector3(Random.Range(-spawnOffsetMax, spawnOffsetMax), Random.Range(-spawnOffsetMax, spawnOffsetMax));
			room.AddSwarmer(swarmer);

		}
	}
	
	void TestCured() {
		if (Level.timeSinceLevelStart - inflamationStart > noCurePeriod) {
			if (Random.value < 0.4f * Time.deltaTime)
				SetNextInflamation();
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
		inflamated = false;
		nextInflammation = Level.timeSinceLevelStart + averageBetweenInflammationTime + Random.Range(-betweenInflammationTimeVariation, betweenInflammationTimeVariation);
	}
}
