using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.UIElements.GraphView;

public class ChainQuest {

	public int chainQuestID;
	public new string name;
	public string description;
	public QuestOrign originator;
	public List<QuestRequirement> requirements;

	public List<Node> chainQuestList;
	public List<QuestReward> rewards;



}
