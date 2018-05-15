using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveEnemy : QuestObjective {

	public BaseEnemy enemy;
	public int quiantitie;

	public ObjectiveEnemy(){
		type = objectiveTypes.ENEMY;
	}



}
