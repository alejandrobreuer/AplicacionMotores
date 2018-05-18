using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class QuestSaveManager : MonoBehaviour {


	public static void CreateAsset<T>() where T : ScriptableObject{

		T asset = ScriptableObject.CreateInstance<T> ();

		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") {
			path = "Assets";
		} else if (Path.GetExtension (path) != ""){
			path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)),"");
		}

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/new " + typeof(T).ToString () + "");

		AssetDatabase.CreateAsset (asset, assetPathAndName);

		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;



	}


}
