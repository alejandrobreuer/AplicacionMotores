using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chain Quest", menuName = "Chain Quest")]
public class ChainQuest: ScriptableObject
{
	public int chainQuestID;
    public string myname;
    public string description;
	public QuestOrign originator;
	public List<QuestRequirement> requirements;
    public List<SingleQuest> chainQuestList;
	public List<QuestReward> rewards;
    public List<Rect> mySingleQuestRects = new List<Rect>();
    public List<SingleQuest> quests = new List<SingleQuest>();

}
