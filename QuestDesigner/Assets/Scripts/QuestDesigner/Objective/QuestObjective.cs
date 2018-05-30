using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour {

	public enum objectiveTypes
	{
		ITEM = 2,
		NPC = 3,
		ENEMY = 4,
		LEVEL = 5
	}

	public int objectivetID; //Is this necesary? (Thinking what to do with this...)
	public objectiveTypes type;


	public objectiveTypes getType(){
		return type;
	}

	public int getID(){
		return objectivetID;
	}

}
