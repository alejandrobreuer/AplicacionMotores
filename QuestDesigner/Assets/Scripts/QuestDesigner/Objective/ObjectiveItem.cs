using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveItem : QuestObjective {

	public BaseItem item;
	public int quanitite;

	public ObjectiveItem(){
		type = objectiveTypes.ITEM;
	}


}
