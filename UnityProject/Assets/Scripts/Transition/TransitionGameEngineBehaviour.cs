﻿using UnityEngine;
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

	public GameObject mainCurtain;

	// Use this for initialization
	void Start () 
	{
		playIntro = true;
		sceneEnded = false;
		textIndex = 0;
		mainCurtain.GetComponent<Animator> ().SetBool ("Up", true);
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
			mainCurtain.GetComponent<Animator> ().SetBool ("Up", false);
		}
	}

	public void EndScene(GameObject busObject)
	{
		StartCoroutine(WaitAndKillGameObject(1.5f, busObject));
		sceneEnded = true;
		// load next scene
		StartCoroutine(WaitAndLoadNextLevel(1.5f, "levelCutWood"));
	}

	IEnumerator WaitAndLoadNextLevel(float timeToWait, string nextLevelName)
	{
		yield return new WaitForSeconds(timeToWait);
		Application.LoadLevel(nextLevelName);
	}
	
	IEnumerator WaitAndKillGameObject(float timeToWait, GameObject go)
	{
		yield return new WaitForSeconds(timeToWait);
		if (go != null)
		{
			Destroy (go);
		}
	}

}
