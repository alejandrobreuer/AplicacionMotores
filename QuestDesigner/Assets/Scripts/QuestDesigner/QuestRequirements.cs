using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRequirements {

	public enum requirementsType
	{
		ITEM,
		SINGLE_QUEST,
		LEVEL
	}

	public int requirementID; //Is this necesary? (Thinking what to do with this...)
	public requirementsType type;
	public GameObject target; //Temporary: Used for Item or Quest requirements.
	public int Quantities; //Used for Level requirements.




}
