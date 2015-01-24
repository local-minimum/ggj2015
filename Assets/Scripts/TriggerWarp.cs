using UnityEngine;
using System.Collections;

public class TriggerWarp : MonoBehaviour {
	public Transform WarpPosition;
	public Transform CameraPosition;
	public float CameraSize;
	public RoomProperties DestinationRoom;
	private RoomProperties ThisRoom;


	void Start() {
		ThisRoom = GetComponentInParent<RoomProperties>();
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, WarpPosition.position);

		Gizmos.color = new Color(0,0,1,0.1f);
		Gizmos.DrawCube(CameraPosition.position, new Vector3(1.6f, 0.9f, 1)*CameraSize*2.2f);
	
	}

	void OnTriggerEnter2D(Collider2D col) {

		Vector3 RelativeDist = col.transform.position-transform.position;

		if(SwarmLeaderController.Leader != null && col.gameObject == SwarmLeaderController.Leader.gameObject)
		{
			ThisRoom.RemoveSwarmer(col.GetComponent<Swarmer>());
			DestinationRoom.AddSwarmer(col.GetComponent<Swarmer>());
			col.transform.position = WarpPosition.position + RelativeDist;
			Level.mainCamera.transform.position = CameraPosition.position + new Vector3(0,0,-10);
			Level.mainCamera.orthographicSize = CameraSize;
			Level.currentRoom = DestinationRoom;
		}
		if(col.tag == "Microbe")
		{
			ThisRoom.RemoveSwarmer(col.GetComponent<Swarmer>());
			DestinationRoom.AddSwarmer(col.GetComponent<Swarmer>());
			col.transform.position = WarpPosition.position + RelativeDist;
		}
	}
}
