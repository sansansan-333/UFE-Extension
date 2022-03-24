using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class UFEExtensionAsset {
    [MenuItem("Assets/Create/U.F.E./Extension File")]
    public static void CreateAsset() {
		UFEExtensionInfo asset = ScriptableObject.CreateInstance<UFEExtensionInfo>();
		Object referencePath = Selection.activeObject;

		string path = AssetDatabase.GetAssetPath(referencePath);
		if (path == "") {
			path = "Assets";
		}
		else if (Path.GetExtension(path) != "") {
			path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(referencePath)), "");
		}

		string fileName = "New UFE Extension";
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName + ".asset");

		if (!AssetDatabase.Contains(asset)) AssetDatabase.CreateAsset(asset, assetPathAndName);

		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;

		UFEExtensionWindow.Init();
	}
}
