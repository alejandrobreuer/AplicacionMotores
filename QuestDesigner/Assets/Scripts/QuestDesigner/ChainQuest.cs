using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEditor.Graphs;

//[CreateAssetMenu(fileName = "New Chain Quest", menuName = "Chain Quest")]
public class ChainQuest: ScriptableObject
{
	public int chainQuestID;
	public new string name;
	public string description;
	public QuestOrign originator;
	public List<QuestRequirement> requirements;
	public List<Node> chainQuestList;
	public List<QuestReward> rewards;

}
