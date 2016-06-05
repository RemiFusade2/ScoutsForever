using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoringScreenClickablePanelBehaviour : MonoBehaviour {

	public List<AudioClip> soundBank;

	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		this.GetComponent<AudioSource> ().Stop ();

		this.GetComponent<AudioSource> ().clip = soundBank[Random.Range(0, soundBank.Count)];

		this.GetComponent<AudioSource> ().Play ();
	} 
}
