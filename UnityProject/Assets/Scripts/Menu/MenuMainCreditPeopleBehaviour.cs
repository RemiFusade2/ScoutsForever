using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuMainCreditPeopleBehaviour : MonoBehaviour {
	
	public MenuMainEngine gameEngine;
	
	public GameObject titleGameObject;
	public CreditsLinksBehaviour linkGameObject;

	public GameObject thisCreditPeopleGameObjectParent;
	public List<GameObject> otherCreditPeopleGameObjects;
	
	public bool detailsShown;

	void OnMouseDown()
	{
		ToggleDetails ();
	}

	public void ToggleDetails()
	{
		detailsShown = !detailsShown;
		
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		
		linkGameObject.GetComponent<Animator> ().SetBool ("Visible", detailsShown); 
		titleGameObject.GetComponent<Animator> ().SetBool ("Visible", detailsShown); 
		
		thisCreditPeopleGameObjectParent.GetComponent<Animator> ().SetBool ("Left", detailsShown); 
		
		foreach (GameObject otherPeople in otherCreditPeopleGameObjects)
		{
			otherPeople.GetComponent<Animator> ().SetBool ("Visible", !detailsShown); 
		}
	}
}
