using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRequirement : ScriptableObject {

	public enum requirementsType
	{
		ITEM = 2,
		SINGLE_QUEST = 3,
		LEVEL = 4
	}

	public int requirementID; //Is this necesary? (Thinking what to do with this...)
	public requirementsType type;


	public requirementsType getType(){
		return type;
	}

	public int getID(){
		return requirementID;
	}

}
