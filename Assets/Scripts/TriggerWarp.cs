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
		Swarmer swarmer = col.GetComponent<Swarmer>();

		if (swarmer) {
			if(swarmer == SwarmLeaderController.Leader)
			{
				TransferSwarmer(swarmer, RelativeDist);
				TransferCamera();
			} else
				TransferSwarmer(swarmer, RelativeDist);

		}
	
	}

	void TransferCamera() {
		Level.mainCamera.transform.position = CameraPosition.position + new Vector3(0,0,-10);
		Level.mainCamera.orthographicSize = CameraSize;
		Level.currentRoom = DestinationRoom;

	}

	void TransferSwarmer(Swarmer swarmer, Vector3 relativeDist) {
		ThisRoom.RemoveSwarmer(swarmer);
		DestinationRoom.AddSwarmer(swarmer);
		col.transform.position = WarpPosition.position + relativeDist;
	}
}
