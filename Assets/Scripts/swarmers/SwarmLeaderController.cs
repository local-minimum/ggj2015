using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SwarmLeaderController : Singleton<SwarmLeaderController> {

	private Swarmer _leader = null;
	public float forceFactor = 1f;

	public Swarmer leader {get { return _leader;}}
	public static Swarmer Leader {get { return Instance._leader;}}

	// Use this for initialization
	void Start () {
//		float x = Random.value * 1000f;
//		float[] p = new float[100000];
//		for (int i=0; i<p.Length;  i++)
//			p[i] = Mathf.PerlinNoise(x, i);
//		Debug.Log(p.Sum()/p.Count());
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 force;

		if (_leader) {
			if (Input.GetMouseButton(0)) {
				force = (Level.pointerPositionInWorld - (Vector2) leader.transform.position).normalized;
			} else
				force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			_leader.AddForceLeader(force * Time.deltaTime * forceFactor);
		}

	}

	public static void SetLeader(Swarmer usurper) {
		Instance._leader = usurper;
	}

	public static void RemoveMeFromPower(Swarmer leader) {
		if (Instance._leader && Instance._leader == leader)
			Instance._leader = null;
	}

	public static bool IsLeader(Swarmer swarmer) {
		return swarmer == Instance._leader;
	}
}
