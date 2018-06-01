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

	private  QuestRequirement.requirementsType reqs = 0;
	private  QuestObjective.objectiveTypes obje = 0;
	private  QuestReward.rewardType rewa = 0;

	public  SingleQuest singleQuest;



	[MenuItem("Quest Designer/Single Quests/Create Single Quest")]
	public static void OpenWindow() 
	{
		CreateSingleQuest window = (CreateSingleQuest)GetWindow (typeof(CreateSingleQuest)); // Referencia a la ventana.
		window.wantsMouseMove = true; 
		window.Show();
	}

	private void OnGUI()
	{
		if (singleQuest == null)
			ToShow ();
		else
			EditQuest ();
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

		reqs = (QuestRequirement.requirementsType)EditorGUILayout.EnumFlagsField ("Requirements:",reqs);
		EditorGUILayout.Space();

		obje = (QuestObjective.objectiveTypes)EditorGUILayout.EnumFlagsField ("Objectives:",obje);
		EditorGUILayout.Space();

		rewa = (QuestReward.rewardType)EditorGUILayout.EnumFlagsField ("Rewards:",rewa);
		EditorGUILayout.Space();

		description = EditorGUILayout.TextField ("Description:", description);
		EditorGUILayout.Space();

		questOrigin = (QuestOrign)EditorGUILayout.ObjectField ("Quest origin:",questOrigin, typeof(QuestOrign),true);
		EditorGUILayout.Space();

		if (GUILayout.Button("Save"))
		{
			var singleQuests = new SingleQuest ();

			singleQuests.name = name;
			singleQuests.description = description;
			singleQuests.originator = questOrigin;
			singleQuests.rewa = rewa;
			singleQuests.obje = obje;
			singleQuests.reqs = reqs;
		}


	}

	void EditQuest()
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

		questID = EditorGUILayout.IntField ("Quest ID:", singleQuest.questID);
		EditorGUILayout.Space();

		name = EditorGUILayout.TextField ("Quest name:", singleQuest.name);
		EditorGUILayout.Space();

		reqs = (QuestRequirement.requirementsType)EditorGUILayout.EnumFlagsField ("Requirements:",singleQuests.reqs);
		EditorGUILayout.Space();

		obje = (QuestObjective.objectiveTypes)EditorGUILayout.EnumFlagsField ("Objectives:",singleQuests.obje);
		EditorGUILayout.Space();

		rewa = (QuestReward.rewardType)EditorGUILayout.EnumFlagsField ("Rewards:",singleQuests.rewa);
		EditorGUILayout.Space();

		description = EditorGUILayout.TextField ("Description:", singleQuest.description);
		EditorGUILayout.Space();

		questOrigin = (QuestOrign)EditorGUILayout.ObjectField ("Quest origin:",singleQuest.originator, typeof(QuestOrign),true);
		EditorGUILayout.Space();

		if (GUILayout.Button("Save Changes"))
		{
			singleQuest.name = name;
			singleQuest.description = description;
			singleQuest.originator = questOrigin;
			singleQuests.rewa = rewa;
			singleQuests.obje = obje;
			singleQuests.reqs = reqs;
		}	
	}
}
