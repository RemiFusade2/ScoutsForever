using UnityEngine;
using System.Collections;

public class CouperBoisScoutParentBehaviour : MonoBehaviour {
	
	public CouperBoisGameEngine gameEngine;

	private bool scoutReturnedInGroup;

	private bool collisionHasBeenProcessed;
	
	public GameObject logPrefab;

	void Start()
	{
		scoutReturnedInGroup = false;
		collisionHasBeenProcessed = false;
	}
	
	public void KillScout()
	{
		if (scoutReturnedInGroup)
		{
			gameEngine.ScoutReturnedInGroup ();
		}
		gameEngine.KillScout (this.gameObject);
	}

	public void ScoutDied()
	{
		if (!collisionHasBeenProcessed)
		{
			collisionHasBeenProcessed = true;
			if (!scoutReturnedInGroup) 
			{
				gameEngine.ScoutDied ();
				this.GetComponent<Animator>().SetTrigger("ScoutHit");
			}
		}
	}

	public void WoodHasBeenCut(GameObject wood)
	{
		if (!collisionHasBeenProcessed) 
		{
			collisionHasBeenProcessed = true;
			scoutReturnedInGroup = true;
			this.GetComponent<Animator> ().SetTrigger ("WoodCut");
			wood.GetComponent<Renderer>().enabled = false;
			Instantiate(logPrefab, wood.transform.position + new Vector3(-1,-0.5f,Random.Range(0.01f,1.0f)), Quaternion.Euler(new Vector3(85,270,90)));
			Instantiate(logPrefab, wood.transform.position + new Vector3(1,0.5f,Random.Range(0.01f,1.0f)), Quaternion.Euler(new Vector3(95,270,90)));
			gameEngine.AddWoodLogs (2);
		}
	}

	public void TriggerSpawnScout()
	{
		gameEngine.TriggerSpawScout ();
	}

	public void PlaySound()
	{
		this.GetComponent<AudioSource> ().Play ();
	}
}
