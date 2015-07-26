using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CouperBoisTasDeBoisBehaviour : MonoBehaviour {

	public CouperBoisGameEngine gameEngine;

	public GameObject woodPrefab;
	private int woodCount;

	private List<GameObject> morceauxDeBois;

	private bool initialized;

	// Use this for initialization
	void Start () 
	{
		morceauxDeBois = new List<GameObject> ();
		initialized = false;
	}

	public void InitializeWithWoodCount(int count)
	{
		woodCount = count;
		float timeBetweenPop = 0.5f;
		for (int i = 0 ; i < woodCount-1 ; i++)
		{
			StartCoroutine(WaitAndAddWood(i*timeBetweenPop));
		}
		StartCoroutine(WaitAndAddWood(woodCount*timeBetweenPop + 1));
	}

	void Update()
	{
		if (!initialized && morceauxDeBois.Count == woodCount)
		{
			initialized = true;
			this.GetComponent<Animator>().SetTrigger("InitializationOK");
			gameEngine.TasDeBoisIsInitialized();
		}
	}

	public bool IsInit()
	{
		return initialized;
	}

	private void AddWood()
	{
		GameObject wood = (GameObject)Instantiate(woodPrefab, this.transform.position + new Vector3(Random.Range(-5.0f,5.0f),Random.Range(0.0f,10.0f),Random.Range(0.01f,1.0f)), Quaternion.Euler(new Vector3(85,270,90)));
		wood.transform.parent = this.transform;
		morceauxDeBois.Add (wood);
	}
	
	IEnumerator WaitAndAddWood(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		AddWood();
	}

	public bool HasWoodLeft()
	{
		return morceauxDeBois.Count > 0;
	}

	public void RemoveWood()
	{
		if (HasWoodLeft()) 
		{
			Destroy (morceauxDeBois[0]);
			morceauxDeBois.RemoveAt (0);
		}
	}

	public void StartMoving()
	{
		this.GetComponent<Animator> ().SetTrigger ("Begins");
	}
}
