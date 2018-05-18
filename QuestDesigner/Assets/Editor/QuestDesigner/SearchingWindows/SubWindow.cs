using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SubWindow : EditorWindow
{
    public int amount;
    public GUIStyle myStyle;
    public GUIStyle style;

    private Texture2D _preview;
    public Object _focusedObject;
    private string nName;
    private string nPath;
    private string currentPath;
    private string newName;


    public static void OpenWindow(Object focusedObject)
    {
        var along = (SubWindow)GetWindow(typeof(SubWindow));
        along._focusedObject = focusedObject;
        along.style = new GUIStyle();
        along.style.fontStyle = FontStyle.Italic;
        along.style.alignment = TextAnchor.MiddleCenter;
        along.style.wordWrap = true;


        along.myStyle = new GUIStyle();
        along.myStyle.fontStyle = FontStyle.BoldAndItalic;
        along.myStyle.fontSize = 15;
        along.myStyle.alignment = TextAnchor.MiddleCenter;
        along.myStyle.wordWrap = true;
        along.Show();

    }

    private void OnGUI()
    {
        DrawPrefabSettings();
        if (GUILayout.Button("CLose"))
            Close();
    }
    private void DrawPrefabSettings()
    {

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Informacion general", myStyle);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        var prevState = _preview;
        _preview = AssetPreview.GetAssetPreview(_focusedObject);
        if (_preview != null)
        {
            if (prevState == null)
                Repaint();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100, 100, 100), _preview, ScaleMode.ScaleToFit);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Path: ", myStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            currentPath = AssetDatabase.GetAssetPath(_focusedObject);
            EditorGUILayout.LabelField(currentPath, style);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Name: ", myStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_focusedObject.name, style);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Path: ", myStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            currentPath = AssetDatabase.GetAssetPath(_focusedObject);
            EditorGUILayout.LabelField(currentPath, style);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Name: ", myStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_focusedObject.name, style);
            EditorGUILayout.EndHorizontal();
        }




        for (int i = 0; i < 2; i++)
            EditorGUILayout.Space();

        EditorGUILayout.LabelField("Herramientas", myStyle);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Open Asset"))
        {
            AssetDatabase.OpenAsset(_focusedObject);
        }

        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("DestroyAsset - Trash"))
        {
            AssetDatabase.MoveAssetToTrash(currentPath);
            UpdateDatabase();
        }

        if (GUILayout.Button("DestroyAsset - Full"))
        {
            AssetDatabase.DeleteAsset(currentPath);
            UpdateDatabase();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("New name:", GUILayout.Width(75));
        nName = EditorGUILayout.TextField(nName);
        if (GUILayout.Button("Rename"))
        {
            AssetDatabase.RenameAsset(currentPath, nName);
            nName = "";
            UpdateDatabase();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("New path: Assets/", GUILayout.Width(110));
        nPath = EditorGUILayout.TextField(nPath);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("New name in path: ", GUILayout.Width(110));
        newName = EditorGUILayout.TextField(newName);
        if (newName == null|| newName =="")
        {
            EditorGUILayout.HelpBox("Si se deja en blanco toma el nombre original.", MessageType.Info);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Copy Asset"))
        {
            string theName;
            if (newName == null || newName == " " || newName == "")
            {
                theName = _focusedObject.name;
            }
            else
            {
                theName = newName;
            }
            if (nPath!=null)
            {
                
                char sa = "/"[0];
                var aux = nPath.Split(sa);
                if (!AssetDatabase.IsValidFolder("Assets/" + nPath))
                {
                    AssetDatabase.CreateFolder("Assets", aux[aux.Length - 1]);
                    AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(_focusedObject), "Assets/" + nPath + "/" + theName + ".prefab");
                    UpdateDatabase();

                }
            }
            ChechIfAssetSameName(nPath, theName,false);

        }

        if (GUILayout.Button("Move Asset"))
        {
            string theName;
            if (newName == null || newName == " " || newName == "")
            {
                theName = _focusedObject.name;
            }
            else
            {
                theName = newName;
            }
            if (nPath != null)
            {

                char sa = "/"[0];
                var aux = nPath.Split(sa);
                if (!AssetDatabase.IsValidFolder("Assets/" + nPath))
                {
                    AssetDatabase.CreateFolder("Assets", aux[aux.Length - 1]);
                    AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(_focusedObject), "Assets/" + nPath + "/" + theName + ".prefab");
                    UpdateDatabase();

                }
            }
            ChechIfAssetSameName(nPath, theName,false);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        string theName2;
        if (newName == null || newName == " " || newName == "")
        {
            theName2 = _focusedObject.name;
        }
        else
        {
            theName2 = newName;
        }
        ChechIfAssetSameName(nPath, theName2,true);
    }
    public void UpdateDatabase()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public bool ChechIfAssetSameName(string path,string name,bool write)
    {
        if (path== null || path=="")
        {
            string[] lookFor = new string[] {"Assets"};
            if (AssetDatabase.IsValidFolder("Assets"))
            {
                var a = AssetDatabase.FindAssets(name, lookFor);
                if (a.Length > 0)
                {
                    if (write)
                    {
                        EditorGUILayout.HelpBox("Ya Existe un Asset con ese nombre en esta carpeta.", MessageType.Warning);

                    }
                    return true;
                }
            }
        }
        else
        {
            string[] lookFor = new string[] { "Assets/" + path };
            if (AssetDatabase.IsValidFolder("Assets/" + nPath))
            {
                var a = AssetDatabase.FindAssets(name, lookFor);
                if (a.Length > 0)
                {
                    if (write)
                    {
                        EditorGUILayout.HelpBox("Ya Existe un Asset con ese nombre en esta carpeta.", MessageType.Warning);

                    }
                    return true;

                }
            }
            
        }
        return false;

    }

}
