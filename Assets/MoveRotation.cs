using UnityEngine;
using System.Collections;

public class MoveRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 diff = rigidbody2D.velocity;
		diff.Normalize();
		
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		if(rigidbody2D.angularVelocity < 10)
			rigidbody2D.MoveRotation(Mathf.LerpAngle(transform.eulerAngles.z, rot_z, Time.deltaTime*10));
		//transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
	}
}
