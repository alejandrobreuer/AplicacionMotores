using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MiVentana : EditorWindow
{
    public int unPar;
    private bool migrupo;
    private int clicks;
    public GameObject go;

    [MenuItem("CustomTools/MyWindow")]
    public static void OpenWindow()
    {
        var w = GetWindow<MiVentana>();
        w.wantsMouseMove = true;
        w.Show();
    }

    private void OnGUI()
    {
        //aca se dibuja todo en ventanas
        EditorGUILayout.LabelField("Hola");
        migrupo = EditorGUILayout.BeginToggleGroup("mi grupo", migrupo);
        EditorGUILayout.Toggle("un bool", false);
        EditorGUILayout.TextField("un texto", "nlebh");
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.LabelField("hice " + clicks + " hasta ahora");
        var col = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Hola", GUILayout.Height(50)))
            clicks++;

        GUI.backgroundColor = col;

        if(GUILayout.Button("abrir una ventana"))
        {
            //MoveAlongWindow.OpenWindow(clicks);

            var w = (EmptyWindow)ScriptableObject.CreateInstance(typeof(EmptyWindow));
            w.position = new Rect(Screen.width / 2, Screen.height / 2, Screen.width, Screen.height);
            w.ShowPopup();
        }

        if(mouseOverWindow == this)
        {
            EditorGUILayout.LabelField("SOBRE ESTO");
        }

        //        if (Event.current.type == EventType.MouseMove) Repaint();

        go = (GameObject)EditorGUILayout.ObjectField("FOCO ACA", go, typeof(GameObject), true);

        if(go != null)
        {
            var t = AssetPreview.GetAssetPreview(go);
            if(t != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.DrawTexture(GUILayoutUtility.GetRect(100, 100), t, ScaleMode.ScaleToFit);
                EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(go));
                EditorGUILayout.EndHorizontal();
            }
        }

        maxSize = new Vector2(500, 500);
        minSize = new Vector2(500, 500);
    }

    private void OnDestroy()
    {
        
    }

    private void OnFocus()
    {
        
    }

    private void OnLostFocus()
    {
        
    }

    private void OnHierarchyChange()
    {
        
    }

    private void OnInspectorUpdate()
    {
        
    }
}
