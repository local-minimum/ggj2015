using UnityEngine;
using System.Collections;

public class TriggerWarp : MonoBehaviour {
	public Vector3 WarpPosition;
	public Vector3 CameraPosition;
	public float CameraSize;
	
	void OnDrawGizmosSelected () {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, WarpPosition);

		Gizmos.color = new Color(0,0,1,0.1f);
		Gizmos.DrawCube(CameraPosition, new Vector3(1.6f, 0.9f, 1)*CameraSize*2.2f);
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		print("Colliding");
		if(col.tag == "Player")
		{
			col.transform.position = WarpPosition;
			Level.mainCamera.transform.position = CameraPosition+new Vector3(0,0,-10);
			Level.mainCamera.orthographicSize = CameraSize;
		}
	}
}
