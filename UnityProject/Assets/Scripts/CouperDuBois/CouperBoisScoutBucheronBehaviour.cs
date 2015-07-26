using UnityEngine;
using System.Collections;

public class CouperBoisScoutBucheronBehaviour : MonoBehaviour {

	private float lastOrderTimer;

	// Use this for initialization
	void Start () 
	{
		lastOrderTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.GetComponent<Animator> ().SetFloat ("LastChoice", Time.time - lastOrderTimer);
		if (Time.time - lastOrderTimer > 2)
		{
			this.GetComponent<Animator>().SetInteger("WaitOrCut", 0); // no current order
		}
	}

	public void GiveOrder(int waitOrCut)
	{		
		this.GetComponent<Animator>().SetInteger("WaitOrCut", waitOrCut);
		lastOrderTimer = Time.time;
	}

	public void ResetLastChoiceTimer()
	{
		lastOrderTimer = Time.time;
		this.GetComponent<Animator>().SetInteger("WaitOrCut", 0); // no current order
	}

	public void PlaySound()
	{
		this.GetComponent<AudioSource> ().Play ();
	}
}
