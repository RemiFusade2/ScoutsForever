using UnityEngine;
using System.Collections;

public class WoodFireScoutBehaviour : MonoBehaviour {

	public WoodFireStoryGameEngine gameEngine;

	public AudioClip scream;
	public AudioClip sigh;

	public void RunAway()
	{
		this.GetComponent<Animator> ().SetTrigger ("RunAway");
	}
	
	public void GoToSleep()
	{
		this.GetComponent<Animator> ().SetTrigger ("GoToSleep");
	}

	public void Scream()
	{
		this.GetComponent<AudioSource> ().clip = scream;
		this.GetComponent<AudioSource> ().Play ();
	}

	public void Sigh()
	{
		this.GetComponent<AudioSource> ().clip = sigh;
		this.GetComponent<AudioSource> ().Play ();
	}

	public void KillScout()
	{
		gameEngine.KillScout (this.gameObject);
	}
}
