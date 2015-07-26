using UnityEngine;
using System.Collections;

public class CouperBoisBusBehaviour : MonoBehaviour {

	public CouperBoisGameEngine gameEngine;

	public void KillBus()
	{
		gameEngine.KillBus ();
	}

	public void BusIsArrived()
	{
		gameEngine.BusIsArrived ();
	}
}
