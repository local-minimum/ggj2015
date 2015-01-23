using UnityEngine;
using System.Collections.Generic;

public class Swarmer : MonoBehaviour {

	public enum SwarmerTypes {TRIANGLE, CIRCLE, SQUARE};

	public SwarmerTypes SwarmerType;

	private HashSet<Swarmer> swarm = new HashSet<Swarmer>();

	public float forceFactor = 1f;

	private float wobbleSeed;

	public float wobbleFrequency;

	public float wobbleAmplitude;

	[Range(0, 1)]
	public float followerAnticipationForce = 0.4f;

	public int NumberOfNeighbours {
		get {
			return swarm.Count;
		}
	}

	public Swarmer isSameSwarmerType(Collider2D other) {
		Swarmer otherSwarmer = other.GetComponent<Swarmer>();
		return otherSwarmer && otherSwarmer.SwarmerType == SwarmerType ? otherSwarmer : null;
	}

	void Start() {
		wobbleSeed = Random.Range(0, 1000000);
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
		rigidbody2D.AddForce(wobble(force * forceFactor));

		foreach (Swarmer swarmer in swarm)
			swarmer.AddForceFollower(force, this);
	}

	public Vector2 wobble(Vector2 force) {
		return Mathf.Sin(wobbleSeed + Level.timeSinceLevelStart * wobbleFrequency) * wobbleAmplitude * force.normalized + force;
	}

	public void AddForceFollower(Vector2 force, Swarmer leader) {
		Vector2 relativeVector = transform.position - leader.transform.position;
		float x = Mathf.Sign(relativeVector.x) == Mathf.Sign(force.x) ? force.x * followerAnticipationForce : force.x;
		float y = Mathf.Sign(relativeVector.y) == Mathf.Sign(force.y) ? force.y * followerAnticipationForce : force.y;
		float absForceX = Mathf.Abs(force.x);
		float absForceY = Mathf.Abs(force.y);

//		float angleFactor = -1 * Vector2.Dot(force.normalized, relativeVector.normalized);
//		angleFactor += 0.5f;
//		angleFactor = Mathf.Clamp(angleFactor, 0, 1.1f);
//		Debug.Log(angleFactor);
		rigidbody2D.AddForce(wobble(new Vector2(Mathf.Clamp(x, -absForceX, absForceX), Mathf.Clamp(y, -absForceY, absForceY)) * forceFactor));
//		Debug.DrawRay(transform.position, force.normalized, Color.cyan, force.magnitude * forceFactor * angleFactor);
	}

}
