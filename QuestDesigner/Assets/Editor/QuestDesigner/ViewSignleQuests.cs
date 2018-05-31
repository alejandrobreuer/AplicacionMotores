using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Object = UnityEngine.Object;


public class ViewSignleQuests : EditorWindow
{
    public GUIStyle myStyle;
    public GUIStyle style;
    private string _SearchName;
    public List<Object> assetList;
    private string newFolderName;
    private bool constantSearch = false;
    private bool _showFoldout;
    private string constantSearchText;
    private Vector2 sp = Vector2.zero;


    //[MenuItem("Quest Designer/Single Quests/View Single Quests")]
    public static void OpenWindow()
    {
        var w = GetWindow<ViewSignleQuests>();


        w.style = new GUIStyle();
        w.style.fontStyle = FontStyle.Italic;
        w.style.alignment = TextAnchor.MiddleCenter;
        w.style.wordWrap = true;


        w.myStyle = new GUIStyle();
        w.myStyle.fontStyle = FontStyle.BoldAndItalic;
        w.myStyle.fontSize = 15;
        w.myStyle.alignment = TextAnchor.MiddleCenter;
        w.myStyle.wordWrap = true;
        w.constantSearchText = "Constant Search";

        w.assetList = new List<Object>();
        w.Show();
    }

    private void OnGUI()
    {

        QuestFinder();


    }

    private void QuestFinder()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Busqueda de Quest:", myStyle);
        var aux = _SearchName;
        _SearchName = EditorGUILayout.TextField(aux);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (constantSearch)
        {
            GUI.backgroundColor = Color.green;

        }
        if (GUILayout.Button(constantSearchText))
        {
            if (constantSearch)
            {
                constantSearch = false;
                constantSearchText = "Constant Search";
            }
            else
            {
                constantSearch = true;
                constantSearchText = "On Constant Search";

            }

        }
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Clear"))
        {
            assetList.Clear();
        }

        if (GUILayout.Button("Search"))
        {
            assetList.Clear();
            string[] paths = AssetDatabase.FindAssets(_SearchName);


            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
                AssetDatabase.GetMainAssetTypeAtPath(paths[i]);
                if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SingleQuest))
                {
                    var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));
                    assetList.Add(loaded);
                }

            }


        }
        GUILayout.EndHorizontal();


        if (aux != _SearchName && constantSearch)
        {
            assetList.Clear();
            string[] paths = AssetDatabase.FindAssets(_SearchName);


            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);

                if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SingleQuest))
                {
                    var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));
                    assetList.Add(loaded);
                }
            }
        }
        WriteSearch(assetList);



    }
    public void WriteSearch(List<Object> assetList)
    {

        sp = EditorGUILayout.BeginScrollView(sp);

        for (int i = 0; i < assetList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(assetList[i].ToString());
            if (GUILayout.Button("Seleccionar"))
            {
                SubWindow.OpenWindow(assetList[i]);

            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

    }
}
