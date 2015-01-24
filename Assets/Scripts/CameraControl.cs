using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	 
	public static Vector3 offset = new Vector3(0,0,-10);
	public static Rect movementBounds;

	Swarmer Current;
	bool coRoutineRunning;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Current != SwarmLeaderController.Leader) {
			OnLeaderChange();
		}


//		if(transform.position > Level.currentRoom)
//
//		if(!coRoutineRunning)
//			transform.position = SwarmLeaderController.Leader.transform.position+offset;
	}

	void OnLeaderChange () {
		Current = SwarmLeaderController.Leader;
		Debug.Log("Leader Changed");
		MoveCameraTo(Current.transform.position+offset, 10);
	}

	void MoveCameraTo (Vector3 destination, float speed) {
		StopAllCoroutines();
		StartCoroutine(Move(destination, speed));
	}

	IEnumerator Move (Vector3 destination, float speed) {
		coRoutineRunning = true;
		while(transform.position != destination)
		{
			transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime*speed);
			yield return null;
		}
		coRoutineRunning = false;
	}
}
