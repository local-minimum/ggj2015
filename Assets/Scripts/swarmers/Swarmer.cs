using UnityEngine;
using System.Collections.Generic;

public class Swarmer : MonoBehaviour {

	public enum SwarmerTypes {TRIANGLE, CIRCLE, SQUARE};

	public SwarmerTypes SwarmerType;

	private HashSet<Swarmer> swarm = new HashSet<Swarmer>();

	public float forceFactor = 1f;

	public int NumberOfNeighbours {
		get {
			return swarm.Count;
		}
	}

	public Swarmer isSameSwarmerType(Collider2D other) {
		Swarmer otherSwarmer = other.GetComponent<Swarmer>();
		return otherSwarmer && otherSwarmer.SwarmerType == SwarmerType ? otherSwarmer : null;
	}

	void Update() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		Swarmer swarmer = isSameSwarmerType(other);
		if (swarmer)
			swarm.Add(swarmer);

	}

	void OnTriggerExit2D(Collider2D other) {
		Swarmer swarmer = isSameSwarmerType(other);
		if (swarmer)
			swarm.Remove(swarmer);
	}
	

	public void AddForceLeader(Vector2 force) {
		rigidbody2D.AddForce(force * forceFactor);

		foreach (Swarmer swarmer in swarm)
			swarmer.AddForceFollower(force, this);
	}

	public void AddForceFollower(Vector2 force, Swarmer leader) {
		Vector2 relativeVector = transform.position - leader.transform.position;
		float angleFactor = -1 * Vector2.Dot(force.normalized, relativeVector.normalized);
		angleFactor += 0.5f;
		angleFactor = Mathf.Min(0, angleFactor);
		Debug.Log(angleFactor);
		rigidbody2D.AddForce(force * angleFactor * forceFactor);
//		Debug.DrawRay(transform.position, force.normalized, Color.cyan, force.magnitude * forceFactor * angleFactor);
	}

}
