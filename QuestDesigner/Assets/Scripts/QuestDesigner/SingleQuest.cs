using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Quest",menuName = "Quest")]
public class SingleQuest : ScriptableObject {

	public int questID;
	public new string name;
	public string description;
	public QuestOrign originator;
	public List<QuestRequirement> requirements;
	public List<QuestObjective> objectives;
	public List<QuestReward> rewards;

	//Estas listas de arriba lo mas probable es que las use pero todavia no se si son nececarias.

	public  QuestRequirement.requirementsType reqs = 0;
	public  QuestObjective.objectiveTypes obje = 0;
	public  QuestReward.rewardType rewa = 0;

	public static void SaveSingleQuest(SingleQuest q){
		q.questID = q.GetInstanceID();
		QuestSaveManager.CreateAsset<SingleQuest> ();
	}

}
