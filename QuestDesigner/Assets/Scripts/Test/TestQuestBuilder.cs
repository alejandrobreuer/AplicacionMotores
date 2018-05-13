using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Testing Class should be deleted when we are done.
public class TestQuestBuilder : MonoBehaviour {


	public SingleQuest newQuest;

	public GameObject npc;
	public GameObject enemy;
	public GameObject hero;
	public GameObject item;
	public GameObject oldQuest;

	void Start () {

		newQuest = new SingleQuest ();
		newQuest.questID = 1;
		newQuest.name = "Testing Quest";
		newQuest.description = "This is a testing quest for testing.";

		//Create Origin
		QuestOrign origin = new QuestOrign();
		origin.originID = 1;
		origin.type = QuestOrign.originTypes.NPC;
		origin.target = npc;
		//Assign the Origin
		newQuest.originator = origin;

		//Create Rewards in order to grant Exp and and Item.
		List<QuestReward> rewards = new List<QuestReward>();
		QuestReward reward = new QuestReward();
		reward.rewardID = 1;
		reward.type = QuestReward.rewardType.EXP;
		reward.Quantities = 150;
		rewards.Add (reward);
		reward.rewardID = 1;
		reward.type = QuestReward.rewardType.ITEM;
		reward.target = item;
		rewards.Add (reward);
		newQuest.rewards = rewards;

		//Create Requirement for the quest
		List<QuestRequirements> requirements = new List<QuestRequirements>();
		QuestRequirements requirement = new QuestRequirements();
		requirement.requirementID = 1;
		requirement.type = QuestRequirements.requirementsType.LEVEL;
		requirement.Quantities = 12;
		requirements.Add (requirement);
		requirement.requirementID = 1;
		requirement.type = QuestRequirements.requirementsType.SINGLE_QUEST;
		requirement.target = oldQuest;
		requirements.Add (requirement);
		newQuest.requirements = requirements;

		ShowQuest (newQuest);
	}

	public void ShowQuest(SingleQuest quest){
		Debug.Log ("Quest ID: " + quest.questID);
		Debug.Log ("Quest Name: " + quest.name);
		Debug.Log ("Quest Description: " + quest.description);
		Debug.Log ("Rewards: ");
		int i = 1;
		foreach (QuestReward reward in quest.rewards) {
			Debug.Log ("Reward number " + i);
			Debug.Log ("Reward ID: " + reward.rewardID);
			Debug.Log ("Reward Type: " + reward.type);
			Debug.Log ("Reward Quiantities: " + reward.Quantities);
			Debug.Log ("Reward Target: " + reward.target);
			i++;
		}
		i = 1;
		Debug.Log ("Requirements: ");
		foreach (QuestRequirements requirement in quest.requirements) {
			Debug.Log ("Requirement number " + i);
			Debug.Log ("Req ID: " + requirement.requirementID);
			Debug.Log ("Req Type: " + requirement.type);
			Debug.Log ("Req Quantities: " + requirement.Quantities);
			Debug.Log ("Req Target: " + requirement.target);
			i++;
		}
		Debug.Log ("Originator ID: " + quest.originator.originID);
		Debug.Log ("Originator Type: " + quest.originator.type);
		Debug.Log ("Originator Target: " + quest.originator.target);

	}


}
