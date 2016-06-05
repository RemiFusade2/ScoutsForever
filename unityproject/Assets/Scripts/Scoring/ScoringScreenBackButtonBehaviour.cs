using UnityEngine;
using System.Collections;

public class ScoringScreenBackButtonBehaviour : MonoBehaviour {

	public ScoringScreenEngine gameEngine;
	
	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		gameEngine.PutCurtainDownAndGoBackToMenu ();
	}  
}
