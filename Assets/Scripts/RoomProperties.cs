using UnityEngine;
using System.Collections;

public class RoomProperties : MonoBehaviour {

	public Rect Bounds;
	public bool StartRoom;

	void Awake () {
		if(StartRoom)
			Level.currentRoom = this;
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = new Color(0,1,0,0.3f);
		Gizmos.DrawCube(new Vector3(Bounds.center.x,Bounds.center.y,0), new Vector3(Bounds.width, Bounds.height));
	}
}
