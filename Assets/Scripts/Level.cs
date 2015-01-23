using UnityEngine;
using System.Collections;

public class Level : Singleton<Level> {

	public float timeSinceLevelStart {
		get {
			return Time.timeSinceLevelLoad;
		}
	}
}
