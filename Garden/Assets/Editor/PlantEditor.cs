using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant))]
public class PlantEditor : Editor {

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DrawDefaultInspector();
		if(GUILayout.Button("Regrow")){
			(target as Plant).Regrow();
		}
	}
}
