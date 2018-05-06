using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ElementGrid))]
public class ElementGridEditor : Editor
{
    private ElementGrid _target;

    private void OnEnable()
    {
        _target = (ElementGrid)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (_target.prefab != null)
        {
            if (GUILayout.Button("PLACE ELEMENTS"))
                _target.SpawnElements();
        }
        else
            EditorGUILayout.HelpBox("PREFAB IS NULL", MessageType.Error);
    }
}
