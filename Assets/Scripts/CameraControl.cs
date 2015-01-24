using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	 
	public Vector3 offset = new Vector3(0,0,-10);
	public Rect movementBounds;
	
	Swarmer Current;
	public bool coRoutineRunning;

	// Use this for initialization
	void OnDrawGizmosSelected () {
		Gizmos.color = new Color(0,1,0,0.3f);
		Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y,0), new Vector3(movementBounds.width, movementBounds.height));
	}
	
	// Update is called once per frame
	void Update () {

		if(Current != SwarmLeaderController.Leader) {
			OnLeaderChange();
		}

		if(!coRoutineRunning && Current != null)
		{
			if(Current.transform.position.x > transform.position.x + movementBounds.width/2 && Current.rigidbody2D.velocity.x > 0)
				rigidbody2D.velocity = Current.rigidbody2D.velocity;

			if(Current.transform.position.x < transform.position.x - movementBounds.width/2 && Current.rigidbody2D.velocity.x < 0)
				rigidbody2D.velocity = Current.rigidbody2D.velocity;

			if(Current.transform.position.y > transform.position.y + movementBounds.height/2 && Current.rigidbody2D.velocity.y > 0)
				rigidbody2D.velocity = Current.rigidbody2D.velocity;

			if(Current.transform.position.y < transform.position.y - movementBounds.height/2 && Current.rigidbody2D.velocity.y < 0)
				rigidbody2D.velocity = Current.rigidbody2D.velocity;
		}
//		if(Current.transform.position.x >= transform.position.x + movementBounds.width/2)
//		{
//			transform.position = Current.transform.position - ;
//		}

//		if(transform.position > Level.currentRoom)
//
//		if(!coRoutineRunning)
//			transform.position = SwarmLeaderController.Leader.transform.position+offset;
	}

	void OnLeaderChange () {
		Current = SwarmLeaderController.Leader;
		Debug.Log("Leader Changed");
		MoveCameraTo(Current.transform, 10);
	}

	void MoveCameraTo (Transform destination, float speed) {
		StopAllCoroutines();
		StartCoroutine(Move(destination, speed));
	}

	IEnumerator Move (Transform destination, float speed) {

		//TODO Fixa så kameran inte hackar vid bankanter;

		coRoutineRunning = true;
		rigidbody2D.velocity = Vector2.zero;
		while(Vector3.Distance(transform.position-offset, destination.position) > 1)
		{
			Vector3 delta = Vector3.Lerp(transform.position, destination.position+offset, Time.deltaTime*speed) - transform.position;

//			if(CheckCameraBounds(transform.position + delta))
//			{
//				transform.position -= delta;
//				return false;
//			}
//			else
				transform.position += delta;

			yield return null;


		}
		coRoutineRunning = false;
	}

	bool CheckCameraBounds(Vector3 destination) {
		//if(!Level.currentRoom.Bounds.Contains(Vector2.Lerp(transform.position, destination, Time.deltaTime*speed))
		if(!Level.currentRoom.Bounds.Contains(camera.ScreenToWorldPoint(Vector3.zero)))
			return true;
		else if(!Level.currentRoom.Bounds.Contains(camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth,camera.pixelHeight,1))))
			return true;
		else
			return false;
	}
}
