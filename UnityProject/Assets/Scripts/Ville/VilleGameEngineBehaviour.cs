using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;

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
	public List<string> endingPerfectTexts;
	public List<string> endingOKTexts;
	public List<string> endingFailTexts;
	private List<string> endingTexts;

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
		Debug.Log (ApplicationModel.idealFontSize);

		uiMainTextWithChoices.fontSize = ApplicationModel.idealFontSize;
		uiMainText.fontSize = ApplicationModel.idealFontSize;

		activeScouts = new List<GameObject> ();
		
		playIntro = true;
		playEnding = false;
		playGame = false;
		sceneEnded = false;
		textIndex = 0;
		savedScouts = 0;
		killedScouts = 0;
		totalScouts = 11; //ApplicationModel.scoutsRemaining;
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
			if (killedScouts == 0)
			{
				endingTexts = endingPerfectTexts;
			} 
			else if (savedScouts == 0)
			{				
				endingTexts = endingFailTexts;
			}
			else
			{				
				endingTexts = endingOKTexts;
			}
		}
	}
	
	public void UpdateSceneEndingCondition()
	{
		if (sceneEnded && bus == null)
		{
			topCurtain.GetComponent<Animator> ().SetBool ("Up", false);
			if (savedScouts > 0)
			{
				ApplicationModel.scoutsRemaining = savedScouts;
				StartCoroutine(WaitAndLoadNextLevel(2.5f, "transitionForest"));
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
		int indexGroup = (remainingScouts > 0) ? ( (remainingScouts - 1) > 10 ? 10 : (remainingScouts - 1) ) : 0;
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
			TellChoiceToAllScouts(1, true);
		}
		if (choice == 2)
		{
			// Wait...
			TellChoiceToAllScouts(-1, true);
		}
		lastChoiceTime = Time.time;
	}

	public void UpdateChoice()
	{
		if (Time.time - lastChoiceTime > choiceDuration)
		{
			// choice is no more up to date
			TellChoiceToAllScouts(0, false);
		}
	}

	public GameObject supriseFXPrefab;

	private void TellChoiceToAllScouts(int stopOrCross, bool showSurpriseFX)
	{
		float timeToWait = 0;
		foreach (GameObject scout in activeScouts)
		{
			if (showSurpriseFX)
			{	
				StartCoroutine(WaitAndInstantiateSurpriseFX(0.1f, scout.transform.position + Vector3.up * 10));
			}
			//scout.GetComponent<ScoutBehaviourScript>().UpdateUserInput(stopOrCross);
			StartCoroutine(WaitAndUpdateScout(timeToWait, scout, stopOrCross));
			timeToWait += 0.05f;
		}
	}

	IEnumerator WaitAndInstantiateSurpriseFX (float timeToWait, Vector3 position)
	{
		yield return new WaitForSeconds(timeToWait);
		GameObject surpriseFXGameObject = Instantiate (supriseFXPrefab, position, Quaternion.Euler(90, 270, 90)) as GameObject;
		StartCoroutine(WaitAndKillGameObject( 1.5f, surpriseFXGameObject));
	}

	IEnumerator WaitAndUpdateScout(float timeToWait, GameObject scout, int stopOrCross)
	{
		yield return new WaitForSeconds(timeToWait);
		if (scout != null)
		{
			scout.GetComponent<VilleScoutBehaviourScript>().UpdateUserInput(stopOrCross);
		}
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
