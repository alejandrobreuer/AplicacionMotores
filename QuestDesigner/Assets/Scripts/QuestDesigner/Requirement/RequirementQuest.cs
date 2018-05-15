using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementQuest : QuestRequirement {

	public SingleQuest previousQuest;

	public RequirementQuest(){
		this.type = requirementsType.SINGLE_QUEST;
	}

}
