using UnityEngine;
using System.Collections;

public class SwarmerActions : MonoBehaviour {

	bool hovering = false;
	private Swarmer swarmer;

	// Use this for initialization
	void Start () {
		swarmer = GetComponent<Swarmer>();
		if (!swarmer)
			Debug.Log(string.Format("{0} is lacking its swarmer!", gameObject));
	}
	
	// Update is called once per frame
	void Update () {
		if (hovering && Input.GetButtonDown("Fire1"))
			SwarmLeaderController.SetLeader(swarmer);
	}

	void OnMouseEnter() {
		hovering = true;
		Debug.Log(string.Format("{0} is hovered", gameObject));
	}

	void OnMouseExit() {
		hovering = false;
	}
}
