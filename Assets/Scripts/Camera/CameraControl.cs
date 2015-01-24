using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	 
	public Vector3 offset = new Vector3(0,0,-10);
	public Rect movementBounds;
	public float changeLeaderTime = 10f;
	public float leaderZoom = 4f;

	Swarmer current;

	private bool coRoutineRunning;

	// Use this for initialization
	void OnDrawGizmosSelected () {
		Gizmos.color = new Color(0,1,0,0.3f);
		Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y,0), new Vector3(movementBounds.width, movementBounds.height));
	}

	void Start() {
		OnLeaderChange();
	}

	// Update is called once per frame
	void Update () {

		if(current != SwarmLeaderController.Leader) {
			OnLeaderChange();
		}

		if(!coRoutineRunning && current != null)
		{
			transform.position = current.transform.position + offset;
//			if(Current.transform.position.x > transform.position.x + movementBounds.width/2 && Current.rigidbody2D.velocity.x > 0)
//				rigidbody2D.velocity = Current.rigidbody2D.velocity;
//
//			if(Current.transform.position.x < transform.position.x - movementBounds.width/2 && Current.rigidbody2D.velocity.x < 0)
//				rigidbody2D.velocity = Current.rigidbody2D.velocity;
//
//			if(Current.transform.position.y > transform.position.y + movementBounds.height/2 && Current.rigidbody2D.velocity.y > 0)
//				rigidbody2D.velocity = Current.rigidbody2D.velocity;
//
//			if(Current.transform.position.y < transform.position.y - movementBounds.height/2 && Current.rigidbody2D.velocity.y < 0)
//				rigidbody2D.velocity = Current.rigidbody2D.velocity;
		}
	}

	void OnLeaderChange () {
		Debug.Log("Leader Changed");
		current = SwarmLeaderController.Leader;
		if (current) 
			MoveCameraTo(current.transform, RoomManager.currentRoom.Bounds.size.magnitude / leaderZoom, changeLeaderTime);
		else
			MoveCameraTo(RoomManager.currentRoom.leaderlessCameraPosition, RoomManager.currentRoom.cameraSize, changeLeaderTime);
	}

	void MoveCameraTo (Transform destination, float orthoSize, float speed) {
		StopAllCoroutines();
		StartCoroutine(Move(destination,  orthoSize, speed));
	}

	IEnumerator Move (Transform destination, float orthoSize, float duration) {
		Debug.Log(camera.orthographicSize);
		Debug.Log(orthoSize);
		coRoutineRunning = true;
		float timeTransitionStartTime = Level.timeSinceLevelStart;
		while(Level.timeSinceLevelStart - timeTransitionStartTime < duration)
		{
			float timeFraction = (Level.timeSinceLevelStart - timeTransitionStartTime) / duration;
			transform.position = Vector3.Lerp(transform.position, destination.position + offset, timeFraction); 
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, orthoSize, timeFraction);

			yield return null;


		}
		coRoutineRunning = false;
	}
}
