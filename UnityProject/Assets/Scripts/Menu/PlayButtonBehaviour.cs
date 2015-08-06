using UnityEngine;
using System.Collections;

public class PlayButtonBehaviour : MonoBehaviour {

	public MenuMainEngine gameEngine;

	void OnMouseDown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Clicked");
		gameEngine.PutCurtainDown ();
	}   
}
