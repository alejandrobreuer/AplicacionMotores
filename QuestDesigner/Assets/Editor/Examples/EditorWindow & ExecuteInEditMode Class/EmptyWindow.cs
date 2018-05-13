using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EmptyWindow : EditorWindow
{
    private void OnGUI()
    {
        //muestra notificaciones dentro de la ventana
        //ShowNotification(new GUIContent("IT'S EMPTY!!"));

        //para hacerlo desaparecer...
        //RemoveNotification();

        if (GUILayout.Button("CLoSe"))
            Close();
    }
}
