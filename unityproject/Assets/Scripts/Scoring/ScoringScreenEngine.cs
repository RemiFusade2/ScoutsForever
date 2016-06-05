using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoringScreenEngine : MonoBehaviour {
	
	public GameObject mainCurtain;

	public TextMesh resultText;

	public ScoutGroupBehaviour scoutGroup;
	public GameObject car;
	public GameObject axe;
	public GameObject tent;

	public Animator MenuButton;

	public Text thankYouText;

	public AudioClip scoutGroupHappy;
	public AudioClip scoutHitByCar;
	public AudioClip scoutHitByAxe;
	public AudioClip scoutScared;

	// Use this for initialization
	void Start () 
	{
		thankYouText.fontSize = ApplicationModel.idealFontSize;

		resultText.text = "";
		if (ApplicationModel.totalScoutsHitByCar > 0)
		{
			resultText.text += ApplicationModel.totalScoutsHitByCar + " " + scout (ApplicationModel.totalScoutsHitByCar) + " hit by a car\n";
		}
		if (ApplicationModel.totalScoutsHurtByAxe > 0)
		{
			resultText.text += ApplicationModel.totalScoutsHurtByAxe + " " + scout (ApplicationModel.totalScoutsHurtByAxe) + " hurt by an axe\n";
		}
		if (ApplicationModel.totalScoutsLostInForest > 0)
		{
			resultText.text += ApplicationModel.totalScoutsLostInForest + " " + scout (ApplicationModel.totalScoutsLostInForest) + " lost in the forest\n";
		}

		resultText.text += "\n" + ApplicationModel.totalScoutsSaved + " " + scout (ApplicationModel.totalScoutsSaved) + " had an awesome week-end!";


		if (ApplicationModel.totalScoutsSaved <= 0)
		{
			thankYouText.text = "It looks like a Game Over\nI'm sure you can do better...";
		}
		else if (ApplicationModel.totalScoutsSaved <= 10)
		{
			thankYouText.text = "I was so close to serve Champagne!\nMaybe you could try again?";
		}
		else
		{
			thankYouText.text = "Your skills are amazing!\nThank you for playing!";
		}
		
		
		MenuButton.SetBool("Visible", true);

		if (ApplicationModel.totalScoutsSaved > 0) {
			scoutGroup.gameObject.SetActive (true);
			scoutGroup.GetComponent<Animator> ().SetBool ("Visible", true);
			scoutGroup.UpdateScoutGroup (ApplicationModel.totalScoutsSaved);

			if (ApplicationModel.totalScoutsSaved == 11)
			{
				scoutGroup.GetComponent<ScoringScreenClickablePanelBehaviour>().soundBank.Add(scoutGroupHappy);
			} else {
				if (ApplicationModel.totalScoutsHitByCar > 0)
				{
					scoutGroup.GetComponent<ScoringScreenClickablePanelBehaviour>().soundBank.Add(scoutHitByCar);
				}
				if (ApplicationModel.totalScoutsHurtByAxe > 0)
				{
					scoutGroup.GetComponent<ScoringScreenClickablePanelBehaviour>().soundBank.Add(scoutHitByAxe);
				}
				if (ApplicationModel.totalScoutsLostInForest > 0)
				{
					scoutGroup.GetComponent<ScoringScreenClickablePanelBehaviour>().soundBank.Add(scoutScared);
				}
			}
		} else if (ApplicationModel.totalScoutsLostInForest > 0) {
			tent.SetActive (true);
			tent.GetComponent<Animator> ().SetBool ("Visible", true);
		} else if (ApplicationModel.totalScoutsHurtByAxe > 0) {
			axe.SetActive (true);
			axe.GetComponent<Animator> ().SetBool ("Visible", true);
		} else {
			car.SetActive(true);
			car.GetComponent<Animator> ().SetBool ("Visible", true);
		}

		this.GetComponent<Animator> ().SetBool ("Active", true);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PutCurtainDownAndGoBackToMenu ();
		}
	}

	private string scout(int count)
	{
		if (count > 1) {
			return "scouts";
		}
		return "scout";
	}

	public void PutCurtainDownAndGoBackToMenu ()
	{
		mainCurtain.GetComponent<Animator> ().SetBool ("Up", false);		
		StartCoroutine (WaitAndGoBackToMenu (2.5f));

		this.GetComponent<Animator> ().SetBool ("Active", false);
	}
	
	IEnumerator WaitAndGoBackToMenu(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		Application.LoadLevelAsync("menu");
	}
}
