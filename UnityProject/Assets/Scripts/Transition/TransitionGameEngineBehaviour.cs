using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TransitionGameEngineBehaviour : MonoBehaviour {

	public GameObject bus;

	public Text uiMainText;
	public GameObject nextButton;

	public List<string> texts;

	private bool playIntro;
	private bool sceneEnded;
	private int textIndex;

	// Use this for initialization
	void Start () 
	{
		playIntro = true;
		sceneEnded = false;
		textIndex = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playIntro && textIndex < texts.Count) 
		{
			nextButton.SetActive(true);
			uiMainText.text = texts[textIndex];
		}
	}

	public void NextText()
	{
		textIndex++;
		if (playIntro && textIndex >= texts.Count)
		{
			nextButton.SetActive(false);
			uiMainText.text = "";
			playIntro = false;
			bus.GetComponent<Animator>().SetTrigger("BusLeaves");
		}
	}

	public void EndScene(GameObject busObject)
	{
		Destroy (busObject);
		sceneEnded = true;
		// load next scene
		Application.LoadLevel ("level1");
	}
}
