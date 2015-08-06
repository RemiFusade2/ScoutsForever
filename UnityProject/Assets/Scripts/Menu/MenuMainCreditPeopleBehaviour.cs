using UnityEngine;
using System.Collections;

public class MenuMainCreditPeopleBehaviour : MonoBehaviour {
	
	public MenuMainEngine gameEngine;
	
	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		gameEngine.ShowCredits(false);
	} 
}
