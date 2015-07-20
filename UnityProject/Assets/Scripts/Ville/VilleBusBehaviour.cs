using UnityEngine;
using System.Collections;

public class VilleBusBehaviour : MonoBehaviour {

	public VilleGameEngineBehaviour gameEngine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EndScene()
	{
		gameEngine.KillBus ();
		gameEngine.PlayEnding ();
	}
}
