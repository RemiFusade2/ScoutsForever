using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;

public class MenuMainEngine : MonoBehaviour {
	
	public GameObject mainCurtain;

	public List<GameObject> mainMenuObjects;
	public List<GameObject> creditsObjects;

	public Text copyrightLabel;

	// Use this for initialization
	void Start ()
	{
		mainCurtain.GetComponent<Animator> ().SetBool ("Up", true);
		ShowCredits (false);

		this.GetComponent<Animator> ().SetBool ("Active", true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void PutCurtainDownAndStartGame()
	{
		mainCurtain.GetComponent<Animator> ().SetBool ("Up", false);

		ApplicationModel.idealFontSize = copyrightLabel.cachedTextGenerator.fontSizeUsedForBestFit;
		Debug.Log (ApplicationModel.idealFontSize);

		StartCoroutine (WaitAndStartGame (2.5f));
		this.GetComponent<Animator> ().SetBool ("Active", false);
	}

	IEnumerator WaitAndStartGame(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		ApplicationModel.scoutsRemaining = 11;
		ApplicationModel.totalScoutsHitByCar = 0;
		ApplicationModel.totalScoutsHurtByAxe = 0;
		ApplicationModel.totalScoutsLostInForest = 0;
		Application.LoadLevelAsync("level1");
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
