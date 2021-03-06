﻿using UnityEngine;
using System.Collections;

public class TriggerWarp : MonoBehaviour {
	public Transform warpSource;
	public Transform warpMidpoint;
	public Transform warpTarget;
	public Transform CameraPosition;
	public float CameraSize;
	private RoomProperties DestinationRoom;
	private RoomProperties ThisRoom;


	void Start() {
		ThisRoom = GetComponentInParent<RoomProperties>();
		DestinationRoom = RoomManager.GetRoomClosestTO(warpTarget);
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(warpSource.position, warpMidpoint.position);
		Gizmos.DrawLine(warpMidpoint.position, warpTarget.position);
	
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = new Color(0,0,1,0.1f);
		Gizmos.DrawCube(warpSource.transform.position, new Vector3(1.6f, 0.9f, 1)*CameraSize*2.2f);
	}
		
	void OnTriggerEnter2D(Collider2D col) {
		Vector3 RelativeDist = col.transform.position-transform.position;
		Swarmer swarmer = col.GetComponent<Swarmer>();

		if (swarmer) {
			if(swarmer == SwarmLeaderController.Leader)
			{
				warpMidpoint.audio.Play();
				TransferSwarmer(swarmer, RelativeDist);
				TransferCamera();
				RoomManager.currentRoom = DestinationRoom;
			} else
				TransferSwarmer(swarmer, RelativeDist);

		}
	
	}

	void TransferCamera() {
		Level.mainCameraControl.MoveCameraToVia(warpSource, warpMidpoint, warpTarget, CameraSize, DestinationRoom.cameraSize / Level.mainCameraControl.leaderZoom, 1);

	}

	void TransferSwarmer(Swarmer swarmer, Vector3 relativeDist) {
		ThisRoom.RemoveSwarmer(swarmer);
		DestinationRoom.AddSwarmer(swarmer);
		swarmer.transform.position = warpTarget.position + relativeDist;
	}
}
