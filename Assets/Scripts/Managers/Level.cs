using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Level : Singleton<Level> {

	public int maxSwarmersPerType = 500;
	public Transform dormantSwarmersHolder;

	public List<Swarmer> swarmerTypes = new List<Swarmer>();

	private Dictionary<Swarmer.SwarmerTypes, List<Swarmer>> swarmPool = new Dictionary<Swarmer.SwarmerTypes, List<Swarmer>>();

	void Start() {
		InitEmptySwarmerLists();
		FindAllExistingSwarmers();
		FillUpWithInactiveSwarmers();
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

	public static Swarmer GetSwarmer(Swarmer.SwarmerTypes swarmerType) {
		Swarmer swarmer = Instance.swarmPool[swarmerType].FirstOrDefault(s => !s.gameObject.activeSelf && s.SwarmerType == swarmerType);
		swarmer.gameObject.SetActive(true);
		return swarmer;
	}
}
