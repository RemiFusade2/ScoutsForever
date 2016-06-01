using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AssemblyCSharp;

public class CouperBoisGameEngine : MonoBehaviour 
{
	private int totalScouts;
	private int remainingScouts;
	private int remainingScoutsInGroup;
	private int killedScouts;
	
	private List<GameObject> activeScouts;

	private bool groupIsReady;

	public GameObject scoutGroup;
	public GameObject scoutBucheron;

	public GameObject scoutPrefab;

	public float scoutSpawnTimer;
	private float lastScoutSpawn;

	public GameObject mainCurtain;

	public GameObject bus;

	public GameObject tasDeBois;
	
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
	
	private float lastScoutCounterChange;
	private bool scoutCounterState;
	
	public List<string> introTexts;
	public string choicesMainText;
	public string choice1Text;
	public string choice2Text;
	public List<string> endingFailTexts;
	public List<string> endingOKTexts;
	public List<string> endingPerfectTexts;
	private List<string> endingTexts;

	private int textIndex;

	private bool playIntro;
	private bool playGame;
	private bool playEnding;
	private bool sceneEnded;

	private int logsCount;

	// Use this for initialization
	void Start () 
	{
		uiMainTextWithChoices.fontSize = ApplicationModel.idealFontSize;
		uiMainText.fontSize = ApplicationModel.idealFontSize;

		logsCount = 0;

		mainCurtain.GetComponent<Animator> ().SetBool ("Up", true);

		activeScouts = new List<GameObject> ();

		totalScouts = ApplicationModel.scoutsRemaining;
		remainingScouts = totalScouts;
		remainingScoutsInGroup = remainingScouts - 1;
		killedScouts = 0;
		scoutGroup.GetComponent<ScoutGroupBehaviour>().UpdateScoutGroup(remainingScoutsInGroup);
		ChangeScoutCounter ();

		int maxWoodCount = 6;
		int minWoodCount = 4;
		int woodCount = Mathf.Max (minWoodCount, Mathf.Min(maxWoodCount, remainingScoutsInGroup));
		tasDeBois.GetComponent<CouperBoisTasDeBoisBehaviour> ().InitializeWithWoodCount (woodCount);

		playIntro = true;
		playGame = false;
		playEnding = false;
		sceneEnded = false;

		groupIsReady = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!sceneEnded)
		{
			if (!playGame && playIntro && textIndex < introTexts.Count) 
			{
				choiceBox1.SetActive(false);
				choiceBox2.SetActive(false);
				scountCounter.SetActive(false);
				nextButton.SetActive(true);
				uiMainText.text = introTexts[textIndex];
				uiMainTextWithChoices.text = "";
				if (textIndex == 1)
				{
					// scout group goes in the forest
					scoutGroup.GetComponent<Animator> ().SetTrigger ("Begins");
				}
				if (textIndex == 4)
				{
					// scout bucheron goes out
					scoutBucheron.GetComponent<Animator> ().SetTrigger ("Begins");
				}
				if (textIndex == 5)
				{
					// bus goes away
					if (bus != null)
					{
						bus.GetComponent<Animator> ().SetTrigger ("Leaves");
					}
				}
				if (textIndex == 8)
				{
					// scout group comes back
					scoutGroup.GetComponent<Animator> ().SetTrigger ("ComesBack");
					tasDeBois.GetComponent<Animator> ().SetTrigger ("Begins");
				}
			} 
			else if (!playGame && playEnding && textIndex < endingTexts.Count) 
			{
				choiceBox1.SetActive(false);
				choiceBox2.SetActive(false);
				scountCounter.SetActive(false);
				nextButton.SetActive(true);
				uiMainText.text = endingTexts[textIndex];
				uiMainTextWithChoices.text = "";
			}
			else if (!playGame)
			{
				uiMainText.text = "";
				uiMainTextWithChoices.text = choicesMainText;
				uiChoice1Text.text = choice1Text;
				uiChoice2Text.text = choice2Text;
				choiceBox1.SetActive(true);
				choiceBox2.SetActive(true);
				scountCounter.SetActive(true);
				nextButton.SetActive(false);
				playGame = true;
				lastScoutSpawn = Time.time-8;
				TriggerSpawScout();
			}
			
			if (playGame && groupIsReady)
			{
				// Spawn scout ?
				if (remainingScouts > 0 && Time.time-lastScoutSpawn > scoutSpawnTimer && activeScouts.Count == 0)
				{
					lastScoutSpawn = Time.time;
					GameObject newScout = SpawnScout();
					if (newScout != null)
					{
						activeScouts.Add(newScout);
					}
				}
				
				UpdateGameEndingCondition ();
			}
			
			UpdateScoutCounter();
		}
	}

	private void UpdateGameEndingCondition()
	{
		bool gameEnd = false;
		if ( (activeScouts.Count == 0 && remainingScoutsInGroup == 0) || (activeScouts.Count == 0 && !tasDeBois.GetComponent<CouperBoisTasDeBoisBehaviour> ().HasWoodLeft() && logsCount <= 0) )
		{
			gameEnd = true;
			endingTexts = endingFailTexts;
			scoutBucheron.GetComponent<Animator>().SetTrigger("GameOver");
		}
		else if (!tasDeBois.GetComponent<CouperBoisTasDeBoisBehaviour> ().HasWoodLeft() && activeScouts.Count == 0)
		{
			gameEnd = true;
			if (killedScouts == 0)
			{
				endingTexts = endingPerfectTexts;
			}
			else
			{
				endingTexts = endingOKTexts;
			}
			scoutBucheron.GetComponent<Animator>().SetTrigger("Ends");
		}

		if (gameEnd)
		{
			playGame = false;
			playEnding = true;
			textIndex = 0;
		}
	}

	public void AddWoodLogs(int numberOfLogs)
	{
		logsCount += numberOfLogs;
	}

	public void TriggerSpawScout()
	{
		if (playGame && groupIsReady)
		{
			// Spawn scout ?
			if (remainingScouts > 0 && Time.time-lastScoutSpawn > scoutSpawnTimer && activeScouts.Count < 3)
			{
				lastScoutSpawn = Time.time;
				GameObject newScout = SpawnScout();
				if (newScout != null)
				{
					activeScouts.Add(newScout);
				}
			}
		}
	}
	
	public void BusIsArrived()
	{
		scoutGroup.GetComponent<Animator> ().SetTrigger ("BusIsArrived");
		scoutBucheron.GetComponent<Animator> ().SetTrigger ("BusIsArrived");
	}

	public void TasDeBoisIsInitialized()
	{
		scoutGroup.GetComponent<Animator> ().SetTrigger ("TasDeBoisIsReady");
	}
	
	private GameObject SpawnScout()
	{
		GameObject newScout = null;
		if (remainingScoutsInGroup > 0 && tasDeBois.GetComponent<CouperBoisTasDeBoisBehaviour> ().HasWoodLeft())
		{
			// spawn new scout
			newScout = (GameObject)Instantiate(scoutPrefab, Vector3.zero, Quaternion.identity);
			newScout.GetComponent<CouperBoisScoutParentBehaviour> ().gameEngine = this;
			
			// Update group
			remainingScoutsInGroup--;
			scoutGroup.GetComponent<ScoutGroupBehaviour>().UpdateScoutGroup(remainingScoutsInGroup);
			
			// Update tas de bois
			tasDeBois.GetComponent<CouperBoisTasDeBoisBehaviour> ().RemoveWood ();
		}

		return newScout;
	}

	public void GroupReadyToBringBackWood ()
	{
		tasDeBois.GetComponent<Animator> ().SetTrigger ("GroupReady");
	}
	
	private void ChangeScoutCounter()
	{
		lastScoutCounterChange = Time.time;
		uiScoutCountText.text = "" + (remainingScouts);
		scountCounter.GetComponent<RawImage>().texture = scoutCounterOFF;
		scoutCounterState = false;
	}
	
	private void UpdateScoutCounter()
	{
		if (!scoutCounterState && Time.time - lastScoutCounterChange > 0.3f)
		{
			scountCounter.GetComponent<RawImage>().texture = scoutCounterON;
			scoutCounterState = true;
		}
	}
	
	public void ScoutDied()
	{
		remainingScouts--;
		killedScouts++;
		// Update scout counter
		ChangeScoutCounter ();
		CameraShake ();
	}

	public void ScoutReturnedInGroup()
	{
		remainingScoutsInGroup++;
		scoutGroup.GetComponent<ScoutGroupBehaviour>().UpdateScoutGroup(remainingScoutsInGroup);
	}

	public void KillBus()
	{
		StartCoroutine(WaitAndKillGameObject(0, bus));
		bus = null;
	}

	IEnumerator WaitAndKillGameObject(float timeToWait, GameObject go)
	{
		yield return new WaitForSeconds(timeToWait);
		if (go != null)
		{
			Destroy (go);
		}
	}

	public void NextText()
	{
		textIndex++;
		if (playIntro && textIndex >= introTexts.Count)
		{
			playIntro = false;
		}
		if (playEnding && textIndex >= endingTexts.Count)
		{
			playEnding = false;
			sceneEnded = true;
			mainCurtain.GetComponent<Animator>().SetBool("Up", false);
			
			if (endingTexts != endingFailTexts)
			{
				ApplicationModel.scoutsRemaining = remainingScouts;
				StartCoroutine(WaitAndLoadNextLevel(2.5f, "levelWoodFire"));
			}
			else
			{
				StartCoroutine(WaitAndLoadMenu(2.5f));
			}

		}
	}
	
	IEnumerator WaitAndLoadMenu(float timeToWait)
	{
		yield return new WaitForSeconds (timeToWait);
		Application.LoadLevelAsync ("menu");
	}
	
	IEnumerator WaitAndLoadNextLevel(float timeToWait, string nextLevelName)
	{
		yield return new WaitForSeconds(timeToWait);
		Application.LoadLevelAsync(nextLevelName);
	}

	public void PressChoice(int choice)
	{
		// choice 1 : down the axe !
		if (choice == 1)
		{
			scoutBucheron.GetComponent<CouperBoisScoutBucheronBehaviour>().GiveOrder(1);
		}
		// choice 2 : just relax
		if (choice == 2)
		{
			scoutBucheron.GetComponent<CouperBoisScoutBucheronBehaviour>().GiveOrder(-1);
		}
	}

	public void KillScout(GameObject scout)
	{
		activeScouts.Remove (scout);
		Destroy (scout);
	}

	public void GroupReadyForGame()
	{
		groupIsReady = true;
	}

	public void CameraShake()
	{
		StartCoroutine(Shake(0.5f, 0.5f));
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
