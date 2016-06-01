using UnityEngine;
using System.Collections;

public class CreditsButtonBehaviour : MonoBehaviour {
	
	public MenuMainEngine gameEngine;

	public BackButtonBehaviour backButton;
	
	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		backButton.GetComponent<Animator> ().SetBool ("Visible", true);
		gameEngine.ShowCredits(true);
	} 
}
