using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward : ScriptableObject {


	public enum rewardType
	{
		ITEM = 2,
		EXP = 3,
		COINS = 4
	}

	public int rewardID; //Is this necesary? (Thinking what to do with this...)
	public rewardType type;



	public rewardType getType(){
		return type;
	}

	public int getID(){
		return rewardID;
	}


}
