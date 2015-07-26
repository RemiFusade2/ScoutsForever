using UnityEngine;
using System.Collections;

public class TransitionBusBehaviour : MonoBehaviour {

	public TransitionGameEngineBehaviour gameEngine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BusLeaves()
	{
		gameEngine.EndScene (this.gameObject);
	}

	public void PlayMotorSound()
	{
		this.GetComponent<AudioSource> ().Play ();
	}
}
