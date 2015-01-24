using UnityEngine;
using System.Collections;

public class Inflammation : MonoBehaviour {

	public GameObject inflammationEffectsHolder;

	private RoomProperties room;

	private Swarmer.SwarmerTypes[] inflamationTypes;
	private Swarmer.SwarmerTypes inflamationType;

	public float averageBetweenInflammationTime = 10f;
	public float betweenInflammationTimeVariation = 5f;
	private float nextInflammation;
	private float inflamationStart;
	private bool inflamated = false;

	// Use this for initialization
	void Start () {
		room = GetComponent<RoomProperties>();
		inflamationTypes = Level.Instance.GetSwarmerTypes();
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

	void ActivateEffects() {
		foreach (ParticleSystem particleSystem in inflammationEffectsHolder.GetComponentsInChildren<ParticleSystem>())
			particleSystem.Play();
	}

	bool IsNewOutBreak() {
		return nextInflammation < Level.timeSinceLevelStart;
	}

	void SetNextInflamation() {
		nextInflammation = Level.timeSinceLevelStart + averageBetweenInflammationTime + Random.Range(-betweenInflammationTimeVariation, betweenInflammationTimeVariation);
	}
}
