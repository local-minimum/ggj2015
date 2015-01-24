using UnityEngine;
using System.Collections;

public class Level : Singleton<Level> {

	public static float timeSinceLevelStart {
		get {
			return Time.timeSinceLevelLoad;
		}
	}

	public static RoomProperties currentRoom;

	public static Camera mainCamera {
		get {
			return Camera.main;
		}
	}
}
