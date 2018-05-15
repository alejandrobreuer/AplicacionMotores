using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestOrign : ScriptableObject {

	public enum originTypes
	{
		NPC,
		ITEM,
		ZONE,
		LEVEL
	}

	public int originID; //Is this necesary? (Thinking what to do with this...)
	public originTypes type;
	public GameObject target; //Temporary: this is a game object, depending on the OriginType, this might be an NPC, an item, a zone or a player's level, each one should be treated differently.



}
