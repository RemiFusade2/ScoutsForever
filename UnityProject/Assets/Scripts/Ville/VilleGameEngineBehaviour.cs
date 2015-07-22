using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class VilleGameEngineBehaviour : MonoBehaviour {

	public float scoutSpawnTimer;
	private float lastScoutSpawn;
	public int totalScouts;
	private int remainingScouts;
	private int savedScouts;
	private int killedScouts;
	public GameObject scoutGroup;
	public List<Texture> scoutGroupTextures;
	private List<GameObject> activeScouts;

	public Animator greenLightAnimator;
	public Animator redLightAnimator;

	public float greenLightTimer;
	public float redLightTimer;

	private bool greenLightIsOn;

	private GameObject activeCar;


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

	public GameObject scoutPrefab;
	public GameObject carPrefab;


	public List<string> introTexts;
	public string choicesMainText;
	public string choice1Text;
	public string choice2Text;
	public List<string> endingTexts;

	private bool playIntro;
	private bool playEnding;
	private bool playGame;
	private bool sceneEnded;
	private int textIndex;

	public GameObject bus;

	public float choiceDuration;
	private float lastChoiceTime;

	public AudioClip backgroundMusic;
	public AudioClip backgroundMusicWithVoices;

	public GameObject topCurtain;

	// Use this for initialization
	void Start () 
	{
		activeScouts = new List<GameObject> ();
		
		playIntro = true;
		playEnding = false;
		playGame = false;
		sceneEnded = false;
		textIndex = 0;
		savedScouts = 0;
		killedScouts = 0;
		remainingScouts = totalScouts;

		ChangeScoutCounter ();

		greenLightAnimator.SetBool("greenLightOn", false);
		redLightAnimator.SetBool("greenLightOn", false);

		this.GetComponent<AudioSource> ().clip = backgroundMusicWithVoices;
		this.GetComponent<AudioSource> ().Play ();
		
		UpdateScoutGroup();

		topCurtain.GetComponent<Animator> ().SetBool ("Up", true);
	}

	// Update is called once per frame
	void Update () 
	{
		greenLightIsOn = ! (Mathf.FloorToInt (Time.time) % (greenLightTimer + redLightTimer) < redLightTimer);
		UpdateGreenLight ();

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
			}
			
			if (playGame)
			{
				// Spawn scout ?
				if (remainingScouts > 0 && Time.time-lastScoutSpawn > scoutSpawnTimer)
				{
					lastScoutSpawn = Time.time;
					GameObject newScout = SpawnScout();
					activeScouts.Add(newScout);
				}
				
				// Spawn car ?
				if (activeCar == null && !greenLightIsOn && (Mathf.FloorToInt (Time.time) % (greenLightTimer + redLightTimer) > 0.2f) && (Mathf.FloorToInt (Time.time) % (greenLightTimer + redLightTimer) < redLightTimer - 0.5f))
				{
					activeCar = SpawnCar();
				}
				
				UpdateGameEndingCondition ();
			}
			
			UpdateScoutCounter ();
			UpdateChoice ();
		}
		UpdateSceneEndingCondition ();
	}

	public void UpdateGameEndingCondition()
	{
		if (playGame && !sceneEnded && (savedScouts + killedScouts) == totalScouts && bus != null)
		{
			bus.GetComponent<Animator>().SetTrigger("BusLeaves");
			playEnding = true;
			playGame = false;
			textIndex = 0;
		}
	}
	
	public void UpdateSceneEndingCondition()
	{
		if (sceneEnded && bus == null)
		{
			topCurtain.GetComponent<Animator> ().SetBool ("Up", false);
			StartCoroutine(WaitAndLoadNextLevel(2.5f, "transitionForest"));
		}
	}
	
	IEnumerator WaitAndLoadNextLevel(float timeToWait, string nextLevelName)
	{
		yield return new WaitForSeconds(timeToWait);
		Application.LoadLevel(nextLevelName);
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
		}
	}

	private void UpdateGreenLight()
	{
		greenLightAnimator.SetBool("greenLightOn", greenLightIsOn);
		redLightAnimator.SetBool("greenLightOn", greenLightIsOn);
		foreach (GameObject scout in activeScouts)
		{
			scout.GetComponent<VilleScoutBehaviourScript>().UpdateGreenLight(greenLightIsOn);
		}
	}

	private GameObject SpawnCar()
	{
		GameObject newCar = (GameObject)Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		newCar.GetComponent<VilleCarBehaviourScript> ().gameEngine = this;
		return newCar;
	}

	private GameObject SpawnScout()
	{
		// spawn new scout
		GameObject newScout = (GameObject)Instantiate(scoutPrefab, new Vector3(-101.67f, -17.2f, 16.8f), Quaternion.identity);
		newScout.GetComponent<VilleScoutBehaviourScript> ().gameEngine = this;
		newScout.GetComponent<VilleScoutBehaviourScript> ().wantCandies = Random.Range(0,2) == 0;
		newScout.GetComponent<VilleScoutBehaviourScript> ().UpdatePicturePosition (remainingScouts, totalScouts);

		// Update group
		remainingScouts--;
		if (remainingScouts <= 0)
		{
			scoutGroup.SetActive(false);
		}
		else
		{
			UpdateScoutGroup();
		}

		return newScout;
	}

	private void UpdateScoutGroup()
	{
		int indexGroup = (remainingScouts - 1) > 10 ? 10 : (remainingScouts - 1);
		scoutGroup.GetComponent<Renderer> ().material.mainTexture = scoutGroupTextures [indexGroup];
	}

	private void ChangeScoutCounter()
	{
		lastScoutCounterChange = Time.time;
		uiScoutCountText.text = "" + (totalScouts - killedScouts);
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

	public void KillScout(GameObject scout)
	{
		killedScouts++;
		ChangeScoutCounter ();
		activeScouts.Remove (scout);
		StartCoroutine(WaitAndKillGameObject(2, scout));
	}
	
	IEnumerator WaitAndKillGameObject(float timeToWait, GameObject go)
	{
		yield return new WaitForSeconds(timeToWait);
		if (go != null)
		{
			Destroy (go);
		}
	}

	public void ScoutGoesOnBus(GameObject scout)
	{
		bus.GetComponent<Animator> ().SetTrigger ("ScoutHopsOn");
		activeScouts.Remove (scout);
		savedScouts++;
		Destroy (scout);
	}

	public void KillBus()
	{
		StartCoroutine(WaitAndKillGameObject(2, bus));
		bus = null;
	}

	public void KillCar(GameObject car)
	{
		activeCar = null;
		Destroy (car);
	}

	public void PlayEnding()
	{
		playGame = false;
		playEnding = true;
	}

	public void PressChoice(int choice)
	{
		if (choice == 1)
		{
			// Cross
			TellChoiceToAllScouts(1);
		}
		if (choice == 2)
		{
			// Wait...
			TellChoiceToAllScouts(-1);
		}
		lastChoiceTime = Time.time;
	}

	public void UpdateChoice()
	{
		if (Time.time - lastChoiceTime > choiceDuration)
		{
			// choice is no more up to date
			TellChoiceToAllScouts(0);
		}
	}

	private void TellChoiceToAllScouts(int stopOrCross)
	{
		float timeToWait = 0;
		foreach (GameObject scout in activeScouts)
		{
			//scout.GetComponent<ScoutBehaviourScript>().UpdateUserInput(stopOrCross);
			StartCoroutine(WaitAndUpdateScout(timeToWait, scout, stopOrCross));
			timeToWait += 0.05f;
		}
	}

	IEnumerator WaitAndUpdateScout(float timeToWait, GameObject scout, int stopOrCross)
	{
		yield return new WaitForSeconds(timeToWait);
		if (scout != null)
		{
			scout.GetComponent<VilleScoutBehaviourScript>().UpdateUserInput(stopOrCross);
		}
	}
}
