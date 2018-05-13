using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SingleQuest : ScriptableObject {

	public int questID;
	public new string name;
	public string description;
	public QuestOrign originator;
	public List<QuestRequirements> requirements;
	public List<QuestObjective> objectives;
	public List<QuestReward> rewards;



}
