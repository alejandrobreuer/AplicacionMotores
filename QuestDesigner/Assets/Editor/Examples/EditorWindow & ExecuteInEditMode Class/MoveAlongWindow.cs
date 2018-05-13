using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveAlongWindow : EditorWindow 
{
    public int amount;
    public GUIStyle myStyle;

    //Si almacenaramos la cantidad en esta ventana, cuando se cierre se perderia, porlo que la pasamos como parámetro
    public static void OpenWindow(int times)
    {
        var along = (MoveAlongWindow)GetWindow(typeof(MoveAlongWindow));
        along.amount = times;
        along.myStyle = new GUIStyle();
        along.myStyle.alignment = TextAnchor.MiddleCenter;
        along.myStyle.fontStyle = FontStyle.BoldAndItalic;
        along.myStyle.fontSize = 20;
        along.Show();
    }

    private void OnGUI()
    {
        /*if (amount == 1)
            EditorGUILayout.LabelField("I TOLD YOU ALREADY", myStyle);
        else if(amount > 0)
            EditorGUILayout.LabelField("I TOLD YOU " + amount +" TIMES ALREADY", myStyle);

        GUI.DrawTexture(GUILayoutUtility.GetRect(200, 200), (Texture2D)Resources.Load("nothing"));

        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i + ")", GUILayout.MaxWidth(50), GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Move Along"))
                Close();
            EditorGUILayout.EndHorizontal();
        }*/

        ShowNotification(new GUIContent("ESTO ES UNA NOTIFIC"));
        ShowNotification(new GUIContent("OTIFIC"));
        //RemoveNotification();

    }
}
