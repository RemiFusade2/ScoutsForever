using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackButtonBehaviour : MonoBehaviour {
	
	public MenuMainEngine gameEngine;

	public List<MenuMainCreditPeopleBehaviour> creditPeople;
	
	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		StartCoroutine (WaitAndResolveBackBehaviour (0.05f));
	}

	IEnumerator WaitAndResolveBackBehaviour(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		
		bool detailsWereShown = false;
		foreach (MenuMainCreditPeopleBehaviour people in creditPeople)
		{
			if (people.detailsShown)
			{
				people.ToggleDetails();
				detailsWereShown = true;
				break;
			}
		}
		
		if (!detailsWereShown)
		{
			gameEngine.ShowCredits(false);
			this.GetComponent<Animator> ().SetBool ("Visible", false);
		}
	}
}
