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
			"Remaing Energy:\t{2}\nCurrent Fraction:\t{0}\nDepleted:\t{1}", 
			myTarget.energyFractionRemaining, myTarget.depleted, myTarget.energy), MessageType.Info);
		EditorGUI.indentLevel -= 1;
	}
}