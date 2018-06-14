using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CreateSingleQuest : EditorWindow {


	private int questID;
    private new string name;
    private string description;
	private QuestOrign questOrigin;
	private QuestRequirement requirements;
	private List<QuestObjective> objectives;
	private List<QuestReward> rewards;

	private  QuestRequirement.requirementsType reqs = 0;
	private  QuestObjective.objectiveTypes obje = 0;
	private  QuestReward.rewardType rewa = 0;

	public SingleQuest singleQuest;

	public QuestSaveManager saveManager;
    public NodeQuest node;
    public CreateChainQuest chain;



	[MenuItem("Quest Designer/Single Quests/Create Single Quest")]
	public static void OpenWindow() 
	{
        CreateSingleQuest window = GetWindow<CreateSingleQuest>(); // Referencia a la ventana.
		window.wantsMouseMove = true;
		window.minSize = new Vector2(600,300);
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
	

		if ((QuestSaveManager)AssetDatabase.LoadAssetAtPath("Assets/" + "SaveManager" + ".asset", typeof(object)) == null)
        {
            ScriptableObjectUtility.CreateAsset<QuestSaveManager>("SaveManager");
        }

        saveManager = (QuestSaveManager)AssetDatabase.LoadAssetAtPath("Assets/" + "SaveManager" + ".asset", typeof(object));

		for (int i = 0; i < 2; i++) {
			
			EditorGUILayout.Space();
		}

		EditorGUILayout.LabelField("Single quest creator", titleStyle);

		for (int i = 0; i < 3; i++) {

			EditorGUILayout.Space();
		}
		//_toPreview = (GameObject)EditorGUILayout.ObjectField("Mostrar: ", _toPreview, typeof(GameObject), true);
	

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
			if(name != null)
			{
				if(AssetDatabase.LoadAssetAtPath("Assets/" + name + ".asset", typeof(object)) == null)
				{	
					saveManager.SINGLEID++;
					SingleQuest singleQuests = ScriptableObject.CreateInstance<SingleQuest>();

					singleQuests.name = name;
					singleQuests.description = description;
					singleQuests.originator = questOrigin;
					singleQuests.rewa = rewa;
					singleQuests.obje = obje;
					singleQuests.reqs = reqs;

					string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + singleQuests.name + ".asset");
					AssetDatabase.CreateAsset(singleQuests, assetPathAndName);
				
				}
			
				Close();
			}
			

		}

		if(name == null)
			EditorGUILayout.HelpBox("You must fill the name field", MessageType.Warning);
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


		name = EditorGUILayout.TextField ("Quest name:", name);
		EditorGUILayout.Space();

		reqs = (QuestRequirement.requirementsType)EditorGUILayout.EnumFlagsField ("Requirements:", reqs);
		EditorGUILayout.Space();

		obje = (QuestObjective.objectiveTypes)EditorGUILayout.EnumFlagsField ("Objectives:", obje);
		EditorGUILayout.Space();

		rewa = (QuestReward.rewardType)EditorGUILayout.EnumFlagsField ("Rewards:", rewa);
		EditorGUILayout.Space();

		description = EditorGUILayout.TextField ("Description:", description);
		EditorGUILayout.Space();

		questOrigin = (QuestOrign)EditorGUILayout.ObjectField ("Quest origin:",singleQuest.originator, typeof(QuestOrign),true);
		EditorGUILayout.Space();

		if (GUILayout.Button("Save Changes"))
		{
			singleQuest.name = name;
			singleQuest.description = description;
			singleQuest.originator = questOrigin;
			singleQuest.rewa = rewa;
			singleQuest.obje = obje;
			singleQuest.reqs = reqs;
            //node
            node.name = singleQuest.name;
            node.description = description;
            chain.Repaint();
            this.Close();
		}	
	}
}
