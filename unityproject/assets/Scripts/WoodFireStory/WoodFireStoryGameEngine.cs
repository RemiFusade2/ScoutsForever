using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

[System.Serializable]
public class WoodFireStoryLine
{
	public int minimumNumberOfScoutsToReadLine;
	public string line;
	public string choice1;
	public string choice2;
	public int scoutsDeadChoice1;
	public int scoutsSavedChoice1;
	public bool scaryAtmosphereChoice1;
	public bool boringAtmosphereChoice1;
	public int scoutsDeadChoice2;
	public int scoutsSavedChoice2;
	public bool scaryAtmosphereChoice2;
	public bool boringAtmosphereChoice2;
}

public class WoodFireStoryGameEngine : MonoBehaviour {

	public GameObject storyScoutPrefab;

	public GameObject scoutGroup;
	
	public GameObject mainBkgBox;
	public Text uiMainText;
	public Text uiMainTextWithChoices;
	public GameObject choiceBox1;
	public Text uiChoice1Text;
	public GameObject choiceBox2;
	public Text uiChoice2Text;
	public GameObject scountCounter;
	public Texture scoutCounterON;
	public Texture scoutCounterOFF;
	public Text uiScoutCountText;
	public GameObject nextButton;

	public GameObject mainCurtain;


	public List<string> introTexts;
	public List<WoodFireStoryLine> story;
	public List<string> endingFailTexts;
	public List<string> endingOKTexts;
	public List<string> endingPerfectTexts;

	private List<string> endingTexts;

	private bool introPlaying;
	private bool gameRunning;
	private bool endingPlaying;
	private int textIndex;

	private int remainingScoutsInGroup;
	private int remainingScouts;
	private int totalScouts;
	private int savedScouts;

	public AudioClip backgroundMusic;
	public AudioClip windMusic;
	public AudioClip cricketsMusic;

	public AudioSource wolfSound;
	public AudioSource owlSound;
	public AudioSource crunchSound;

	// UI state
	private bool uiForStory;

	public Transform scarfSpawner;
	public List<GameObject> scoutScarfs;


	public WoodFireTentBehaviour tent;

	// Use this for initialization
	void Start () 
	{		
		uiMainTextWithChoices.fontSize = ApplicationModel.idealFontSize;
		uiMainText.fontSize = ApplicationModel.idealFontSize;

		introPlaying = true;
		gameRunning = false;
		endingPlaying = false;
		textIndex = 0;
		
		scountCounter.SetActive (true);
		uiForStory = false;
		ShowUI ();
		uiMainText.text = introTexts[0];

		totalScouts = ApplicationModel.scoutsRemaining;
		remainingScouts = totalScouts;
		remainingScoutsInGroup = remainingScouts;
		savedScouts = 0;
		UpdateScoutCounter ();
		scoutGroup.GetComponent<ScoutGroupBehaviour>().UpdateScoutGroup (remainingScoutsInGroup);

		mainCurtain.GetComponent<Animator> ().SetBool ("Up", true);
		
		this.GetComponent<Animator> ().SetBool ("Active", true);
	}
	
	private void HideUITemporarily(float seconds)
	{
		StartCoroutine(WaitAndRestoreUIVisibility(seconds));
		uiMainText.enabled = false;
		uiMainTextWithChoices.enabled = false;
		choiceBox1.SetActive (false);
		choiceBox2.SetActive (false);
		nextButton.SetActive (false);
		mainBkgBox.SetActive (false);
	}
	
	IEnumerator WaitAndRestoreUIVisibility(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		mainBkgBox.SetActive (true);
		ShowUI ();
	}

	private void ShowUI()
	{
		if (uiForStory)
		{
			uiMainText.enabled = false;
			uiMainTextWithChoices.enabled = true;
			choiceBox1.SetActive (true);
			choiceBox2.SetActive (true);
			nextButton.SetActive (false);
		}
		else
		{
			uiMainText.enabled = true;
			uiMainTextWithChoices.enabled = false;
			choiceBox1.SetActive (false);
			choiceBox2.SetActive (false);
			nextButton.SetActive (true);
		}
	}

	private void UpdateScoutCounter()
	{
		uiScoutCountText.text = remainingScouts.ToString ();
		scountCounter.GetComponent<RawImage>().texture = scoutCounterOFF;
		StartCoroutine(WaitAndSwitchCounterOn(0.3f));
	}

	IEnumerator WaitAndSwitchCounterOn(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		scountCounter.GetComponent<RawImage>().texture = scoutCounterON;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			mainCurtain.GetComponent<Animator> ().SetBool ("Up", false);
			this.GetComponent<Animator> ().SetBool ("Active", false);
			StartCoroutine(WaitAndLoadMenu(2));
		}
	}

	private void ScoutsDie(int numberOfScouts, float timeOffset)
	{
		if (numberOfScouts > 0)
		{
			for (int i = 0 ; i < numberOfScouts ; i++)
			{
				if (i < remainingScoutsInGroup)
				{
					StartCoroutine(WaitAndSendScoutToDie(timeOffset+i));
					StartCoroutine(WaitAndCameraShake(timeOffset+i+2.0f));
					StartCoroutine(WaitAndPlaySound(timeOffset+i+1.8f, crunchSound));
					StartCoroutine(WaitAndSpawnScarf(timeOffset+i+2.5f));
				}
			}
		}
	}
	
	IEnumerator WaitAndSendScoutToDie(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		GameObject scout = (GameObject) Instantiate (storyScoutPrefab);
		scout.GetComponent<WoodFireScoutBehaviour> ().gameEngine = this;
		scout.GetComponent<WoodFireScoutBehaviour> ().RunAway ();
		remainingScoutsInGroup--;
		scoutGroup.GetComponent<ScoutGroupBehaviour>().UpdateScoutGroup (remainingScoutsInGroup);
		// update counter
		remainingScouts--;
		UpdateScoutCounter ();
	}
	
	private void ScoutsGoToBed(int numberOfScouts, float timeOffset)
	{
		if (numberOfScouts > 0) 
		{
			for (int i = 0; i < numberOfScouts; i++) 
			{
				if (i < remainingScoutsInGroup)
				{
					StartCoroutine(WaitAndSendScoutToBed(timeOffset+i));
					savedScouts++;
				}
			}
		}
	}

	IEnumerator WaitAndSendScoutToBed(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		GameObject scout = (GameObject) Instantiate (storyScoutPrefab);
		scout.GetComponent<WoodFireScoutBehaviour> ().gameEngine = this;
		scout.GetComponent<WoodFireScoutBehaviour> ().GoToSleep ();
		remainingScoutsInGroup--;
		scoutGroup.GetComponent<ScoutGroupBehaviour>().UpdateScoutGroup (remainingScoutsInGroup);
		tent.scoutsInTent++;
	}

	public void KillScout(GameObject scout)
	{
		Destroy (scout);
	}

	public void PressChoice(int choice)
	{
		WoodFireStoryLine storyLine = story[textIndex];
		int deadScouts = (choice == 1 ? storyLine.scoutsDeadChoice1 : storyLine.scoutsDeadChoice2);
		int savedScouts = (choice == 1 ? storyLine.scoutsSavedChoice1 : storyLine.scoutsSavedChoice2);
		bool scaryAtmosphere = (choice == 1 ? storyLine.scaryAtmosphereChoice1 : storyLine.scaryAtmosphereChoice2);
		bool boringAtmosphere = (choice == 1 ? storyLine.boringAtmosphereChoice1 : storyLine.boringAtmosphereChoice2);
		if (scaryAtmosphere)
		{
			LaunchScaryAtmosphere();
		}
		if (deadScouts > 0)
		{
			ScoutsDie(deadScouts, 2);
		}
		if (boringAtmosphere)
		{
			LaunchBoringAtmosphere();
		}
		if (savedScouts > 0)
		{
			ScoutsGoToBed(savedScouts, 2);
		}
		IncrementStoryText ();
		UpdateUI (deadScouts+savedScouts);
	}

	public void NextText()
	{
		textIndex++;
		UpdateUI (0);
	}

	private void IncrementStoryText()
	{
		textIndex++;
		if (textIndex < story.Count)
		{
			WoodFireStoryLine storyLine = story[textIndex];
			while (storyLine.minimumNumberOfScoutsToReadLine > totalScouts) 
			{
				textIndex++;
				if (textIndex >= story.Count)
				{
					break;
				}
				storyLine = story[textIndex];
			}
		}
	}

	private void UpdateUI(int scoutsAwayTmpValue)
	{
		if (introPlaying)
		{
			if (textIndex >= introTexts.Count)
			{
				// set up story
				introPlaying = false;
				gameRunning = true;
				textIndex = -1;
				IncrementStoryText();
				uiForStory = true;
				ShowUI ();
			}
			else
			{
				uiMainText.text = introTexts[textIndex];
			}
		}
		if (gameRunning) 
		{
			if (textIndex >= story.Count || (remainingScoutsInGroup-scoutsAwayTmpValue) <= 0)
			{
				// set up ending
				gameRunning = false;
				endingPlaying = true;
				textIndex = 0;
				uiForStory = false;

				// depends on player choices
				if (savedScouts <= 0)
				{
					endingTexts = endingFailTexts;
				} 
				else if (savedScouts < totalScouts)
				{
					endingTexts = endingOKTexts;
				}
				else
				{
					endingTexts = endingPerfectTexts;
				}
			}
			else
			{
				uiMainTextWithChoices.text = story[textIndex].line;
				uiChoice1Text.text = story[textIndex].choice1;
				uiChoice2Text.text = story[textIndex].choice2;
			}
		}
		if (endingPlaying)
		{
			if (textIndex >= endingTexts.Count)
			{
				// end game
				mainCurtain.GetComponent<Animator> ().SetBool ("Up", false);
				ApplicationModel.totalScoutsSaved = savedScouts;
				ApplicationModel.totalScoutsLostInForest = totalScouts - savedScouts;
				StartCoroutine(WaitAndLoadScoring(2.5f));
				this.GetComponent<Animator> ().SetBool ("Active", false);
			}
			else
			{
				uiMainText.text = endingTexts[textIndex];
			}
		}
	}
	
	IEnumerator WaitAndLoadMenu(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		Application.LoadLevelAsync ("menu");
	}
	
	IEnumerator WaitAndLoadScoring(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		Application.LoadLevelAsync ("scoring");
	}

	private void LaunchBoringAtmosphere()
	{
		// UI
		HideUITemporarily (4);
		// music
		this.GetComponent<AudioSource> ().Stop ();
		this.GetComponent<AudioSource> ().clip = cricketsMusic;
		this.GetComponent<AudioSource> ().Play ();
		// animals		
		StartCoroutine(WaitAndPlaySound (1, owlSound));

		// remake standard atmosphere
		StartCoroutine(WaitAndRestoreFunnyAtmosphere(4));
	}

	private void LaunchScaryAtmosphere()
	{
		// UI
		HideUITemporarily (4);
		// music
		this.GetComponent<AudioSource> ().Stop ();
		this.GetComponent<AudioSource> ().clip = windMusic;
		this.GetComponent<AudioSource> ().Play ();
		// animals
		StartCoroutine(WaitAndPlaySound (1, wolfSound));
		// stop objects
		scoutGroup.GetComponent<Animator> ().SetBool ("Stopped", true);

		// remake standard atmosphere
		StartCoroutine(WaitAndRestoreFunnyAtmosphere(4));
	}

	IEnumerator WaitAndPlaySound(float timeToWait, AudioSource sound)
	{
		yield return new WaitForSeconds(timeToWait);
		sound.Play ();
	}
	
	IEnumerator WaitAndRestoreFunnyAtmosphere(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		// music
		this.GetComponent<AudioSource> ().Stop ();
		this.GetComponent<AudioSource> ().clip = backgroundMusic;
		this.GetComponent<AudioSource> ().Play ();
		// animals
		wolfSound.Stop ();
		owlSound.Stop ();
		// move objects
		scoutGroup.GetComponent<Animator> ().SetBool ("Stopped", false);
	}

	IEnumerator WaitAndSpawnScarf(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);

		int index = Random.Range(0,scoutScarfs.Count);
		GameObject scarf = Instantiate(scoutScarfs[index], scarfSpawner.transform.position, Quaternion.Euler(90,180,0)) as GameObject;
		scarf.GetComponent<Rigidbody>().velocity = Vector3.right * 10 + Vector3.up * 10;
		scarf.GetComponent<Rigidbody> ().angularVelocity = Vector3.forward * 1;
	}



	IEnumerator WaitAndCameraShake(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		StartCoroutine(Shake(0.3f, 0.3f));
	}
	
	IEnumerator Shake(float duration, float magnitude) {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration) 
		{			
			elapsed += Time.deltaTime;          
			
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			Camera.main.transform.position = new Vector3(originalCamPos.x+x, originalCamPos.y+y, originalCamPos.z);
			
			yield return null;
		}
		
		Camera.main.transform.position = originalCamPos;
	}
}
