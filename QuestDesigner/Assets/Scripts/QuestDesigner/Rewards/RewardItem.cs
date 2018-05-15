using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardItem : QuestReward {

	public BaseItem item;
	public int quantitie;

	public RewardItem(){
		type = rewardType.ITEM;
	}

}
