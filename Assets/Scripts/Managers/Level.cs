using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Level : Singleton<Level> {

	public int maxSwarmersPerType = 500;
	public Transform dormantSwarmersHolder;
	public Text clock;

	public List<Swarmer> swarmerTypes = new List<Swarmer>();

	private Dictionary<Swarmer.SwarmerTypes, List<Swarmer>> swarmPool = new Dictionary<Swarmer.SwarmerTypes, List<Swarmer>>();

	public int deathByMicrobes = 150;
	public int deathByFoodCount = 20;
	private List<Food> foods = new List<Food>(); 

	[Range(1, 2)]
	public float difficultyAmplifier = 1.1f;
	public float difficultySurvivalExpectency = 60f;

	void Start() {
		InitEmptySwarmerLists();
		FindAllExistingSwarmers();
		FillUpWithInactiveSwarmers();
	}


	void Update() {
		clock.text = string.Format("{0}", Mathf.Round(Level.timeSinceLevelStart));
	}

	void InitEmptySwarmerLists() {
		foreach (Swarmer.SwarmerTypes swarmType in GetSwarmerTypes())
			swarmPool[swarmType] = new List<Swarmer>();
	}

	void FindAllExistingSwarmers() {
		Swarmer[] allSwarmers = GameObject.FindObjectsOfType<Swarmer>();

		foreach (Swarmer.SwarmerTypes swarmType in GetSwarmerTypes())
			swarmPool[swarmType].AddRange(allSwarmers.Where(s => s.SwarmerType == swarmType));

	}

	void FillUpWithInactiveSwarmers() {
		foreach (Swarmer.SwarmerTypes swarmType in GetSwarmerTypes()) {
			Swarmer prefab = swarmerTypes.Where(s => s.SwarmerType == swarmType).FirstOrDefault();
			if (prefab) {
				while (swarmPool[swarmType].Count() < maxSwarmersPerType) {
					GameObject swarmerGO = (GameObject) Instantiate(prefab.gameObject);
					swarmerGO.name = swarmerGO.name + swarmPool[swarmType].Count();
					swarmerGO.transform.parent = dormantSwarmersHolder;
					Swarmer swarmer = swarmerGO.GetComponent<Swarmer>();
					swarmPool[swarmType].Add(swarmer);
					swarmerGO.SetActive(false);
				}
			} else
				Debug.LogError(string.Format("Missing swarmer prefab of type {0}", swarmType));
		}
	}
	
	public Swarmer.SwarmerTypes[] GetSwarmerTypes() {
		return (Swarmer.SwarmerTypes[]) System.Enum.GetValues(typeof(Swarmer.SwarmerTypes));
	}

	public static float timeSinceLevelStart {
		get {
			return Time.timeSinceLevelLoad;
		}
	}


	public static Camera mainCamera {
		get {
			return Camera.main;
		}
	}

	private CameraControl _mainCameraControl;

	public static CameraControl mainCameraControl {
		get {
			if (!Instance._mainCameraControl)
				Instance._mainCameraControl = mainCamera.GetComponent<CameraControl>();
			return Instance._mainCameraControl;
		}
	}

	public static Vector2 pointerPositionInWorld {
		get {
			return mainCamera.ScreenToWorldPoint(pointerPosition);
		}
	}

	public static Vector3 pointerPosition {
		get {
			return Input.mousePosition;
		}
	}

	public static float PerlinValue {
		get {
			return 0.465f;
		}
	}

	public static void MakeSwarmerDormant(Swarmer swarmer) {
		SwarmLeaderController.RemoveMeFromPower(swarmer);
		swarmer.transform.parent = Instance.dormantSwarmersHolder;
		swarmer.gameObject.SetActive(false);
	}

	public static bool IsAvailable(Swarmer.SwarmerTypes swarmerType) {
		return Instance.swarmPool[swarmerType].Any(s => !s.gameObject.activeSelf && s.SwarmerType == swarmerType);
	}

	public static Swarmer GetRandomSwarmer() {
		int index = Random.Range(0, Instance.swarmPool.Keys.Count() - 1);
		return GetSwarmer(Instance.swarmPool.Keys.ToArray()[index]);
	}

	public static Swarmer GetSwarmer(Swarmer.SwarmerTypes swarmerType) {
		Swarmer swarmer = Instance.swarmPool[swarmerType].FirstOrDefault(s => !s.gameObject.activeSelf && s.SwarmerType == swarmerType);
		swarmer.gameObject.SetActive(true);
		return swarmer;
	}

	public static float currentDifficulty {
		get {
			return Mathf.Pow((Instance.difficultySurvivalExpectency * 0.5f + timeSinceLevelStart) / Instance.difficultySurvivalExpectency, Instance.difficultyAmplifier);
		}
	}

	public static void registerNewFood(Food food) {
		Instance.foods.Add(food);
	}

	public float fractionOfFoodDeath {
		get {
			return Instance.foods.Sum(f => f.gameObject.activeSelf ? 1 : 0) / Instance.deathByFoodCount;
		}
	}

	public float fractionOfMicrobeDeath {
		get {
			float dormant = 0;
			foreach (KeyValuePair<Swarmer.SwarmerTypes, List<Swarmer>> kvp in Instance.swarmPool)
				dormant += kvp.Value.Sum(s => s.gameObject.activeSelf ? 1 : 0);

			return dormant / Instance.deathByMicrobes;
		}
	}
}
