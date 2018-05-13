using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward {


	public enum rewardType
	{
		ITEM,
		EXP,
		COINS
	}

	public int rewardID; //Is this necesary? (Thinking what to do with this...)
	public rewardType type;
	public GameObject target; //Temporary: Used for Item Rewards.
	public int Quantities; //Temporary: Used for Exp and Coin rewards.


}
