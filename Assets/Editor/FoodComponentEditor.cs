using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FoodComponent))]
public class FoodComponentEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		FoodComponent myTarget = (FoodComponent) target;

		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		
		EditorGUILayout.HelpBox(string.Format(
			"Current Fraction:\t{0}", 
			myTarget.energyFractionRemaining), MessageType.Info);
		EditorGUI.indentLevel -= 1;
	}
}