using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour {

	public enum objectiveTypes
	{
		ITEM,
		NPC,
		ENEMY,
		LEVEL
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
