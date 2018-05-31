using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Chain Quest", menuName = "Chain Quest")]
public class ChainQuest: ScriptableObject
{
	public int chainQuestID;
    public string myname;
    public string description;
	public QuestOrign originator;
	public List<QuestRequirement> requirements;
    //public List<> chainQuestList;
	public List<QuestReward> rewards;

}
