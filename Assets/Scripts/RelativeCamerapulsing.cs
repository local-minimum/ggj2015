using UnityEngine;
using System.Collections;

public class RelativeCamerapulsing : MonoBehaviour {
	public float Max;
	public float Speed;

	public float BaseSize;

	// Use this for initialization
	void Start () {
		BaseSize = camera.orthographicSize;
		StartCoroutine(Grow());
	}
	
	// Update is called once per frame
	IEnumerator Grow () {
		StopCoroutine(Shrink ());
		while(camera.orthographicSize < BaseSize+Max-0.5f)
		{
			Debug.Log("Growing");
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, BaseSize+Max, Time.deltaTime*Speed);
			yield return null;
		}

		StartCoroutine(Shrink());
	}

	IEnumerator Shrink () {
		StopCoroutine(Grow());
		while(camera.orthographicSize > BaseSize+0.5f)
		{
			Debug.Log("Shrinking");
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, BaseSize, Time.deltaTime*Speed);
			yield return null;
		}
		
		StartCoroutine(Grow());
	}
}
