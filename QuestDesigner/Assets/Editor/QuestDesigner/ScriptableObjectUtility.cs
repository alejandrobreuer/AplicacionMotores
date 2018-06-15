using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ScriptableObjectUtility
{
    public static void CreateAsset<T>( string name) where T : ScriptableObject
    {
        if (AssetDatabase.LoadAssetAtPath("Assets/" + name + ".asset", typeof(object)) == null)
        {
            //Creamos la instancia del asset
            T asset = ScriptableObject.CreateInstance<T>();

            //Creamos la ubicación donde vamos a guardar el asset
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + name + ".asset");

            //Creamos el asset
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            
        }

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

    }
}
