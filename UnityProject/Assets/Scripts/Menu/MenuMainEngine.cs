using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuMainEngine : MonoBehaviour {
	
	public GameObject mainCurtain;

	public List<GameObject> mainMenuObjects;
	public List<GameObject> creditsObjects;

	// Use this for initialization
	void Start ()
	{
		mainCurtain.GetComponent<Animator> ().SetBool ("Up", true);
		ShowCredits (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PutCurtainDown()
	{
		mainCurtain.GetComponent<Animator> ().SetBool ("Up", false);
	}

	public void StartGame()
	{
		Application.LoadLevel("level1");
	}

	public void ShowCredits(bool creditsVisible)
	{
		foreach (GameObject o in mainMenuObjects)
		{
			o.GetComponent<Animator>().SetBool("Visible", !creditsVisible);
		}
		foreach (GameObject o in creditsObjects)
		{
			o.GetComponent<Animator>().SetBool("Visible", creditsVisible);
		}
	}
}
