using UnityEngine;
using System.Collections;

public class MenuMainScoutTitleBehaviour : MonoBehaviour {
	
	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
	} 
}
