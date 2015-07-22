using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VilleCarBehaviourScript : MonoBehaviour {

	public VilleGameEngineBehaviour gameEngine;

	public List<AudioClip> nutnuts;

	public void KillCar()
	{
		gameEngine.KillCar (this.gameObject);
	}

	public void PlayNutNutSound()
	{
		int r = Random.Range (0, nutnuts.Count);
		this.GetComponent<AudioSource> ().clip = nutnuts[r];
		this.GetComponent<AudioSource> ().Play ();
	}
}
