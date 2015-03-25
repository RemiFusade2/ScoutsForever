using UnityEngine;
using System.Collections;

public class GameEngineBehaviour : MonoBehaviour {

	public Animator greenLightAnimator;
	public Animator redLightAnimator;

	public float greenLightTimer;
	public float redLightTimer;

	public bool greenLightIsOn;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		greenLightIsOn = Mathf.FloorToInt (Time.time) % (greenLightTimer + redLightTimer) < greenLightTimer;
		greenLightAnimator.SetBool("greenLightOn", greenLightIsOn);
		redLightAnimator.SetBool("greenLightOn", greenLightIsOn);
	}
}
