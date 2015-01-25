using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapRoom : MonoBehaviour {
	public Transform moveTo;
	public float camSize;
	public float moveSpeed;

	public Color baseRoomColor;
	public Color currentRoomColor;

	public RoomProperties ClosestRoom;

	void Start () {
		ClosestRoom = RoomManager.GetRoomClosestTO(moveTo);

	}

	// Use this for initialization
	public void OnClick () {

		GetComponent<Button>().image.color = currentRoomColor;

//		if(SwarmLeaderController.Leader == null)
//		{
//			Level.mainCameraControl.MoveCameraTo(moveTo, camSize, moveSpeed);
//		}
//		else
	//	{
			SwarmLeaderController.SetLeader(null);
			RoomManager.currentRoom = ClosestRoom;
			Level.mainCameraControl.MoveCameraTo(moveTo, camSize, moveSpeed);
			print(RoomManager.currentRoom);
		//}

		print(RoomManager.currentRoom);
	}
	
	// Update is called once per frame
	void Update () {
		if(RoomManager.currentRoom != ClosestRoom)
			GetComponent<Button>().image.color = baseRoomColor;
	}
}
