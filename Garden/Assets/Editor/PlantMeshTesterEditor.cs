using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlantMeshTester))]
public class PlantMeshTesterEditor : Editor {

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DrawDefaultInspector();
		if(GUILayout.Button("Regrow")){
			(target as PlantMeshTester).Regrow();
		}
	}
}
