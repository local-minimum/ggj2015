using UnityEngine;
using System.Collections;

public class Level : Singleton<Level> {

	public static float timeSinceLevelStart {
		get {
			return Time.timeSinceLevelLoad;
		}
	}

	public static Camera mainCamera {
		get {
			return Camera.main;
		}
	}

	public static Vector2 pointerPositionInWorld {
		get {
			return mainCamera.ScreenToWorldPoint(pointerPosition);
		}
	}

	public static Vector3 pointerPosition {
		get {
			return Input.mousePosition;
		}
	}
}
