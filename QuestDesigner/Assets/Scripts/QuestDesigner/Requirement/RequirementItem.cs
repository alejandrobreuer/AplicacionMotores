using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementItem : QuestRequirement {

	public BaseItem item;

	public RequirementItem(){
		this.type = requirementsType.ITEM;
	}


}
