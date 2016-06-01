using UnityEngine;
using System.Collections;

public class WoodFireTentBehaviour : MonoBehaviour {

	public int scoutsInTent;

	public AudioSource snoreSound;
	public Animator tentAnimator;

	public void PlaySnoreSound()
	{
		if (scoutsInTent > 0)
		{
			snoreSound.Pause();
			snoreSound.Play();
		}
	}

	void OnMouseDown() 
	{
		if (scoutsInTent > 0)
		{
			tentAnimator.SetTrigger ("move");
		}
	}
}
