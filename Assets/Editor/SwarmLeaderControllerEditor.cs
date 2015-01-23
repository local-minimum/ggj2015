using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SwarmLeaderController))]
public class SwarmLeaderControllerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		SwarmLeaderController myTarget = (SwarmLeaderController) target;

		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		
		EditorGUILayout.HelpBox(string.Format(
			"Leader:\t\t{0}\nSwarm size:\t{1}", 
			myTarget.leader, myTarget.leader ? myTarget.leader.NumberOfNeighbours : 0), MessageType.Info);
		EditorGUI.indentLevel -= 1;
	}
}