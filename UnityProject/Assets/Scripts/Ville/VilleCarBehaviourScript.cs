using UnityEngine;
using System.Collections;

public class VilleCarBehaviourScript : MonoBehaviour {

	public VilleGameEngineBehaviour gameEngine;

	public void KillCar()
	{
		gameEngine.KillCar (this.gameObject);
	}
}
