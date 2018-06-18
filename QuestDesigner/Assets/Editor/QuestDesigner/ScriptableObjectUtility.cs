using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ScriptableObjectUtility
{
    public static void CreateAsset<T>( string folder,string name) where T : ScriptableObject
    {
        if (!AssetDatabase.IsValidFolder("Assets/Quests"))
        {
            AssetDatabase.CreateFolder("Assets", "Quests");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Quests/" + folder))
        {
            AssetDatabase.CreateFolder("Assets/Quests", folder);
        }

        if (AssetDatabase.LoadAssetAtPath("Assets/Quests/" + folder + "/" + name + ".asset", typeof(object)) == null) 
        {
            //Creamos la instancia del asset
            T asset = ScriptableObject.CreateInstance<T>();

            //Creamos la ubicación donde vamos a guardar el asset
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/Quests/" + folder + "/" + name + ".asset");
            //Creamos el asset
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            
        }

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

    }
}
