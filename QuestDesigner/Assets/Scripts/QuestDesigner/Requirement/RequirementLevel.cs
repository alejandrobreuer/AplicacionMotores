using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementLevel : QuestRequirement {

	public int requiredLevel;

	public RequirementLevel(){
		this.type = requirementsType.LEVEL;
	}


}
