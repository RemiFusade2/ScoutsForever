using UnityEngine;
using System.Collections;

public class CreditsLinksBehaviour : MonoBehaviour {

	public string url;

	void OnMouseDown() 
	{
		Application.OpenURL(url);
	}
}
