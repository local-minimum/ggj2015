using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class MapRoom : MonoBehaviour {
	public Transform moveTo;
	public float camSize;
	public float moveSpeed;

	public Color baseRoomColor;
	public Color currentRoomColor;

	Color StatusColor;

	public RoomProperties ClosestRoom;

	public Image Food;
	public Image Microbe;

	public Sprite Circle;
	public Sprite Triangle;
	public Sprite Square;

	void Start () {

		foreach(Transform child in transform)
		{
			if(child.tag == "MicrobeIcon")
			{
				Microbe = child.GetComponent<Image>();
			}

			else if(child.tag == "FoodIcon")
			{
				Food = child.GetComponent<Image>();
			}

		}

		Food.enabled = false;
		Microbe.enabled = false;

		ClosestRoom = RoomManager.GetRoomClosestTO(moveTo);

	}

	// Use this for initialization
	public void OnClick () {

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

		print(ClosestRoom);
		if(RoomManager.Instance.roomsWithFood.Contains(ClosestRoom))
		{
			Food.enabled = true;

			//StatusColor = Color.red;
		}
		else
			Food.enabled = false;

		if(RoomManager.Instance.roomsWithInfection.Contains(ClosestRoom))
		{
			Microbe.enabled = true;

			if(ClosestRoom.inflammationType == Swarmer.SwarmerTypes.CIRCLE)
				Microbe.sprite = Circle;
			if(ClosestRoom.inflammationType == Swarmer.SwarmerTypes.TRIANGLE)
				Microbe.sprite = Triangle;
			if(ClosestRoom.inflammationType == Swarmer.SwarmerTypes.SQUARE)
				Microbe.sprite = Square;
			//StatusColor = Color.red;
		}
		else
			Microbe.enabled = false;

		if(RoomManager.currentRoom == ClosestRoom)
			GetComponent<Button>().image.color = currentRoomColor;
		else
			GetComponent<Button>().image.color = baseRoomColor;
	}
}
