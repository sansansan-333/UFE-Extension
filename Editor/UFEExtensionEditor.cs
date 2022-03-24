using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UFEExtensionInfo))]
public class UFEExtensionEditor : Editor {
	public override void OnInspectorGUI() {
		if (GUILayout.Button("Open U.F.E Extension Window"))
			UFEExtensionWindow.Init();
	}
}
