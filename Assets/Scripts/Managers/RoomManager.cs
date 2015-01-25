using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoomManager : Singleton<RoomManager> {
	private List<RoomProperties> _Rooms = new List<RoomProperties>();
	public RoomProperties stomach;

	public List<RoomProperties> Rooms {

		get {
			if (_Rooms.Count() == 0)
				_Rooms.AddRange(FindObjectsOfType<RoomProperties>());
			return _Rooms;
		}
	}
	
	private RoomProperties _currentRoom;

	public static RoomProperties currentRoom {
		get {
			if (!Instance._currentRoom)
					Instance._currentRoom = GetRoomClosestTO(Level.mainCamera.transform);
			return Instance._currentRoom;
		}

		set {
			Instance._currentRoom = value;
		}
	}

	public static RoomProperties GetRandomOtherRoom(RoomProperties currentRoom) {
		IEnumerable<RoomProperties> rooms = Instance.Rooms.Where(r => r!=currentRoom);
		return rooms.ToArray()[Random.Range(0, rooms.Count())];
	}

	public static RoomProperties GetRoomClosestTO(Transform place) {
		return Instance.Rooms.OrderBy(r => Vector2.Distance(r.transform.position, place.position)).FirstOrDefault();
	}
}
