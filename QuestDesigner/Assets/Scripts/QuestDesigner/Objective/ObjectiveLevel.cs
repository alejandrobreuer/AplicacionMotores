using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveLevel : QuestObjective {

	public int requiredLevel;

	public ObjectiveLevel(){
		type = objectiveTypes.LEVEL;
	}


}
