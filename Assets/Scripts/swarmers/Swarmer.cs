using UnityEngine;
using System.Collections.Generic;

public class Swarmer : MonoBehaviour {

	public enum SwarmerTypes {TRIANGLE, CIRCLE, SQUARE};

	public SwarmerTypes SwarmerType;

	private HashSet<GameObject> neighbours = new HashSet<GameObject>();

	public float forceFactor = 1f;

	public int NumberOfNeighbours {
		get {
			return neighbours.Count;
		}
	}

	public bool isSameSwarmerType(GameObject other) {
		Swarmer otherSwarmer = other.GetComponent<Swarmer>();
		return otherSwarmer && otherSwarmer.SwarmerType == SwarmerType;
	}

	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isSameSwarmerType(other.gameObject)) {
			neighbours.Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (isSameSwarmerType(other.gameObject))
			neighbours.Remove(other.gameObject);
	}

	public void AddForce(Vector2 force) {
		if (SwarmLeaderController.IsLeader(this))
		    AddForceLeader(force);
		else
			AddForceFollower(force, this);
		
	}

	public void AddForceLeader(Vector2 force) {
		rigidbody2D.AddForce(force * forceFactor);
//		Debug.DrawRay(transform.position, force.normalized, Color.green, force.magnitude * forceFactor);
	}

	public void AddForceFollower(Vector2 force, Swarmer leader) {
		Vector2 relativeVector = transform.position - leader.transform.position;
		float angleFactor = -1 * Vector2.Dot(force.normalized, relativeVector.normalized);
		angleFactor += 0.5f;
		angleFactor = Mathf.Min(0, angleFactor);
		rigidbody2D.AddForce(force * angleFactor * forceFactor);
//		Debug.DrawRay(transform.position, force.normalized, Color.cyan, force.magnitude * forceFactor * angleFactor);
	}

}
