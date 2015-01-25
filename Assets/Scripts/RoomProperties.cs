using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoomProperties : MonoBehaviour {

	public Rect Bounds;
	public Transform foodPoint;
	public Transform swarmHolder;
	public Transform leaderlessCameraPosition;

	private Inflammation inflammation;
	private HashSet<Swarmer> swarm = new HashSet<Swarmer>();
	public float cameraSize = 30f;

	void Start () {
		inflammation = GetComponent<Inflammation>();
		foreach (Swarmer swarmer in swarmHolder.GetComponentsInChildren<Swarmer>())
			swarm.Add(swarmer);
		if (this == RoomManager.Instance.stomach)
			StartGutPopulation();
	}

	void StartGutPopulation() {
		while (swarm.Count() < 10) {
			Swarmer swarmer = Level.GetRandomSwarmer();
			swarmer.transform.position = inflammation.RandomSpawnPosition;
			AddSwarmer(swarmer);
		}
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = new Color(0,1,0,0.3f);
		Gizmos.DrawCube(new Vector3(Bounds.center.x,Bounds.center.y,0), new Vector3(Bounds.width, Bounds.height));
	}

	public void RemoveSwarmer(Swarmer swarmer) {
		swarm.Remove(swarmer);
	}

	public void AddSwarmer(Swarmer swarmer) {
		swarm.Add(swarmer);
		swarmer.transform.parent = swarmHolder;
	}

	public IEnumerable<Swarmer> GetSwarmerByType(Swarmer.SwarmerTypes swarmererType) {
		return swarm.Where(s => s.SwarmerType == swarmererType);
	}

	public float swarmerCount {
		get {
			return swarm.Count();
		}
	}

	public bool inflammated {
		get {
			return inflammation.inflammated;
		}
	}

	public Swarmer.SwarmerTypes inflammationType {
		get {
			return inflammation.inflamationType;
		}
	}

	public bool hasFood {
		get {
			return foodPoint.childCount > 0;
		}
	}
}
