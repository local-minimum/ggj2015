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

	public ParticleSystem leaderEffect;
	public ParticleSystem followerEffect;

	[Range(0, 1)]
	public float slimeDragFactor = 0.5f;

	[Range(0, 1)]
	public float slimeForceFactor = 0.8f;

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
		LeaderEffect();
		FollowerEffect();
	}

	void LeaderEffect() {
		bool isLeader = SwarmLeaderController.IsLeader(this);
		if (leaderEffect.isPlaying != isLeader) {
			if (isLeader)
				leaderEffect.Play();
			else
				leaderEffect.Stop();
		}
	}

	void FollowerEffect() {
		if (!SwarmLeaderController.Leader)
			return;

		bool isFollower = SwarmLeaderController.Leader.isInSwarm(this) && rigidbody2D.velocity.magnitude > 0.25f;
		if (followerEffect.isPlaying != isFollower) {
			if (isFollower)
				followerEffect.Play();
			else
				followerEffect.Stop();
		}
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
		DirectLeaderEffect(force);

		foreach (Swarmer swarmer in swarm)
			swarmer.AddForceFollower(force, this);
	}

	public void DirectLeaderEffect(Vector2 force) {
		if (force.magnitude < 0.1f)
			leaderEffect.transform.localRotation = Quaternion.identity;
		else
			leaderEffect.transform.rotation = Quaternion.LookRotation(force * -1);
	}

	public Vector2 wobble(Vector2 force) {
		return Mathf.Sin(wobbleSeed + Level.timeSinceLevelStart * wobbleFrequency) * wobbleAmplitude * force.normalized + force;
	}

	public bool isInSwarm(Swarmer swarmer) {
		return swarm.Contains(swarmer);
	}

	public void AddForceFollower(Vector2 force, Swarmer leader) {
		Vector2 relativeVector = transform.position - leader.transform.position;
		float x = Mathf.Sign(relativeVector.x) == Mathf.Sign(force.x) ? force.x * followerAnticipationForce : force.x;
		float y = Mathf.Sign(relativeVector.y) == Mathf.Sign(force.y) ? force.y * followerAnticipationForce : force.y;
		float absForceX = Mathf.Abs(force.x);
		float absForceY = Mathf.Abs(force.y);

		rigidbody2D.AddForce(wobble(new Vector2(Mathf.Clamp(x, -absForceX, absForceX), Mathf.Clamp(y, -absForceY, absForceY)) * forceFactor));

	}


	public void OnSlimeEnter() {
		Debug.Log("InSlime");
		forceFactor *= slimeForceFactor;
		rigidbody2D.drag *= slimeDragFactor;
	}

	public void OnSlimeExit() {
		forceFactor /= slimeForceFactor;
		rigidbody2D.drag /= slimeDragFactor;
	}
}
