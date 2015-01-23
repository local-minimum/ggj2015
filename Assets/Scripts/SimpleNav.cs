using UnityEngine;
using System.Collections;

public class SimpleNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*3;
	
	}
}
