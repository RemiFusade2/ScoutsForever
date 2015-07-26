using UnityEngine;
using System.Collections;

public class CouperBoisScoutGroupBehaviour : ScoutGroupBehaviour {

	public CouperBoisGameEngine gameEngine;

	public void GroupInTheForest()
	{
		gameEngine.GroupReadyToBringBackWood ();
	}

	public void GroupReadyForGame()
	{
		gameEngine.GroupReadyForGame ();
		gameEngine.TriggerSpawScout ();
	}
}
