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
    private string prevName;

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
        window.name = "";
        window.prevName = window.name;     
        if(window.singleQuest != null)
        {
            window.name = window.singleQuest.name;
            window.prevName = window.singleQuest.name;
        }
        if (window.node != null)
        {
            window.name = window.node.name;
            window.prevName = window.node.name;
        }
        if ((QuestSaveManager)AssetDatabase.LoadAssetAtPath("Assets/Quests/Save/" + "SaveManager" + ".asset", typeof(object)) == null)
        {
            ScriptableObjectUtility.CreateAsset<QuestSaveManager>("Save", "SaveManager");
        }
        window.saveManager = (QuestSaveManager)AssetDatabase.LoadAssetAtPath("Assets/Quests/Save/" + "SaveManager" + ".asset", typeof(object));
    }
    public void Open()
    {
        OpenWindow();
    }

	private void OnGUI()
	{
		if (singleQuest == null)
			ToShow ();
		else
        {          
			EditQuest ();
        }
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

		if(name != "")
        {
            if (GUILayout.Button("Save"))
            {
                Save();
            }
            Repaint();
        }

		if(name == "")
        {
            EditorGUILayout.HelpBox("You must fill the name field", MessageType.Warning);
            Repaint();
        }
	}

    private void Save()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Quests"))
        {
            AssetDatabase.CreateFolder("Assets", "Quests");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Quests/Single"))
        {
            AssetDatabase.CreateFolder("Assets/Quests", "Single");
        }
        if (singleQuest == null)
        {
            SingleQuest singleQuests = ScriptableObject.CreateInstance<SingleQuest>();
            singleQuests.questID = saveManager.SINGLEID;
            singleQuests.name = name;
            singleQuests.description = description;
            singleQuests.originator = questOrigin;
            singleQuests.rewa = rewa;
            singleQuests.obje = obje;
            singleQuests.reqs = reqs;
            saveManager.SINGLEID++;

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/Quests/Single/" + singleQuests.name + "_" + singleQuests.questID + ".asset");
            AssetDatabase.CreateAsset(singleQuests, assetPathAndName);

            if (node != null)
            {
                node.name = name;
                node.description = description;
                chain.Repaint();
            }
            prevName = singleQuests.name;
            singleQuest = (SingleQuest)AssetDatabase.LoadAssetAtPath("Assets/Quests/Single/" + name + "_" + singleQuests.questID.ToString() + ".asset", typeof(object));
            Close();
        }
        else
        {
            if (name == prevName)
            {
                singleQuest.name = name;
                singleQuest.description = description;
                singleQuest.originator = questOrigin;
                singleQuest.rewa = rewa;
                singleQuest.obje = obje;
                singleQuest.reqs = reqs;

                if (node != null)
                {
                    node.name = name;
                    node.description = description;
                    chain.Repaint();
                }
            }
            else
            {
                AssetDatabase.RenameAsset("Assets/Quests/Single/" + prevName + "_" + singleQuest.questID + ".asset", name + "_" + singleQuest.questID);
                singleQuest.name = name;
                singleQuest.description = description;
                singleQuest.originator = questOrigin;
                singleQuest.rewa = rewa;
                singleQuest.obje = obje;
                singleQuest.reqs = reqs;

                if (node != null)
                {
                    node.name = name;
                    node.description = description;
                    chain.Repaint();
                }
                prevName = name;
            }
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
            Save();
		}	
	}
}
