using UnityEngine;
using System.Collections;

public class CreditsButtonBehaviour : MonoBehaviour {
	
	public MenuMainEngine gameEngine;
	
	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		gameEngine.ShowCredits(true);
	} 
}
