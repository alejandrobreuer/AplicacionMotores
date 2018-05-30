using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRequirement : ScriptableObject {

	public enum requirementsType
	{
		ITEM = 1,
		SINGLE_QUEST = 2,
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
