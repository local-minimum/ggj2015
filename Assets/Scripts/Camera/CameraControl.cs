using UnityEngine;
using System.Collections.Generic;

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
			MoveCameraTo(current.transform,  RoomManager.currentRoom.cameraSize / leaderZoom, changeLeaderTime);
		else
			MoveCameraTo(RoomManager.currentRoom.leaderlessCameraPosition, RoomManager.currentRoom.cameraSize, changeLeaderTime);
	}

	public void MoveCameraTo (Transform destination, float orthoSize, float speed) {
		StopAllCoroutines();
		StartCoroutine(Move(destination,  orthoSize, speed));
	}

	public void MoveCameraToVia (Transform source, Transform midPoint, Transform destination, float orthoSizeMid, float orthoSizeFinal, float duration) {
		StopAllCoroutines();
		StartCoroutine(Move(source, midPoint, destination, orthoSizeMid,  orthoSizeFinal, duration));
	}

	IEnumerator<WaitForSeconds> Move (Transform destination, float orthoSize, float duration) {

		coRoutineRunning = true;

		float timeTransitionStartTime = Level.timeSinceLevelStart;
		
		while(Level.timeSinceLevelStart - timeTransitionStartTime < duration)
		{
			UpdateCameraPositionAndSize((Level.timeSinceLevelStart - timeTransitionStartTime) / duration, destination, orthoSize);
			yield return null;

		}
		coRoutineRunning = false;
	}

	IEnumerator<WaitForSeconds> Move (Transform sourcePoint, Transform midPoint, Transform destination, float orthoSizeMid, float orthoSizeTarget, float duration) {
		
		coRoutineRunning = true;
		
		float timeTransitionStartTime = Level.timeSinceLevelStart;
		while(Level.timeSinceLevelStart - timeTransitionStartTime < duration / 2f)
		{
			UpdateCameraPositionAndSize((Level.timeSinceLevelStart - timeTransitionStartTime) / duration, sourcePoint, midPoint, orthoSizeMid);
			yield return null;
			
		}

		timeTransitionStartTime = Level.timeSinceLevelStart;
		while(Level.timeSinceLevelStart - timeTransitionStartTime < duration / 2f)
		{
			UpdateCameraPositionAndSize((Level.timeSinceLevelStart - timeTransitionStartTime) / duration, transform, destination, orthoSizeTarget);
			yield return null;
			
		}
		coRoutineRunning = false;
	}


	void UpdateCameraPositionAndSize(float timeFraction, Transform destination, float orthoSizeTarget ) {
		UpdateCameraPositionAndSize(timeFraction, transform, destination, orthoSizeTarget);
	}

	void UpdateCameraPositionAndSize(float timeFraction, Transform source, Transform destination, float orthoSizeTarget ) {
		
		transform.position = Vector3.Lerp(source.position, destination.position + offset, timeFraction); 
		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, orthoSizeTarget, timeFraction);
	}
}
