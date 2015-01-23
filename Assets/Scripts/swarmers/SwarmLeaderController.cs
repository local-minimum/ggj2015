using UnityEngine;
using System.Collections;

public class SwarmLeaderController : Singleton<SwarmLeaderController> {

	private Swarmer _leader = null;
	public float forceFactor = 1f;

	public Swarmer leader {get { return _leader;}}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (_leader) {
			Vector2 force = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			_leader.AddForce(force * Time.deltaTime * forceFactor);
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
