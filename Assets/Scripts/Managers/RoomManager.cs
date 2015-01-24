﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RoomManager : Singleton<RoomManager> {
	private RoomProperties [] _Rooms;


	public RoomProperties[] Rooms {

		get {
			if (_Rooms == null)
				_Rooms = FindObjectsOfType<RoomProperties>();
			return _Rooms;
		}
	}
	
	private RoomProperties _currentRoom;

	public static RoomProperties currentRoom {
		get {
			if (!Instance._currentRoom)
					Instance._currentRoom = Instance.Rooms.OrderBy(r => Vector2.Distance(r.transform.position, Level.mainCamera.transform.position)).FirstOrDefault();
			return Instance._currentRoom;
		}

		set {
			Instance._currentRoom = value;
		}
	}

}
