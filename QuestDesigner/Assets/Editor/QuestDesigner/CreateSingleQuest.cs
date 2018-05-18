using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CreateSingleQuest : EditorWindow {


	private int questID;
	private string name, description;
	private QuestOrign questOrigin;
	private QuestRequirement requirements;
	private List<QuestObjective> objectives;
	private List<QuestReward> rewards;


	[MenuItem("Quest Designer/Single Quests/Create Single Quest")]
	public static void OpenWindow() 
	{
		CreateSingleQuest window = (CreateSingleQuest)GetWindow (typeof(CreateSingleQuest)); // Referencia a la ventana.
		window.wantsMouseMove = true; 
		window.Show();
	}

	private void OnGUI()
	{
		ToShow ();
	}

	private void ToShow()  // Cambiar este nombre despues, solo temporal.
	{

		GUIStyle titleStyle = new GUIStyle ();
		titleStyle.alignment = TextAnchor.UpperCenter;
		titleStyle.fontSize = 24;


		for (int i = 0; i < 2; i++) {
			
			EditorGUILayout.Space();
		}

		EditorGUILayout.LabelField("Single quest creator", titleStyle);

		for (int i = 0; i < 3; i++) {

			EditorGUILayout.Space();
		}
		//_toPreview = (GameObject)EditorGUILayout.ObjectField("Mostrar: ", _toPreview, typeof(GameObject), true);
	
		questID = EditorGUILayout.IntField ("Quest ID:", questID);
		EditorGUILayout.Space();
		name = EditorGUILayout.TextField ("Quest name:", name);
		EditorGUILayout.Space();
		description = EditorGUILayout.TextField ("Description:", description);
		EditorGUILayout.Space();
		questOrigin = (QuestOrign)EditorGUILayout.ObjectField ("Quest origin:",questOrigin, typeof(QuestOrign),true);
		EditorGUILayout.Space();

		/*var req = ScriptableObject.CreateInstance<QuestRequirement>();
		SerializedObject serializedRequirement = new UnityEditor.SerializedObject (req);
		SerializedProperty serializedPropertyMyRequirements = serializedRequirement.FindProperty ("requirements");

		req = EditorGUILayout.PropertyField (serializedPropertyMyRequirements,req);*/


	}
}
