using UnityEngine;
using System.Collections;

public class VilleScoutBehaviourScript : MonoBehaviour 
{
	public VilleGameEngineBehaviour gameEngine;

	public bool wantCandies;

	// Use this for initialization
	void Start () 
	{
		if (wantCandies) 
		{
			this.GetComponent<Animator> ().SetBool ("GoToShop", wantCandies);
		}
	}

	public void UpdatePicturePosition(int index, int maxIndex)
	{
		float minY = -0.5f;
		float maxY = 0.5f;
		this.transform.FindChild ("Picture").transform.Translate(new Vector3(0,minY + ((maxY-minY) * index) / maxIndex,0));
	}

	public void UpdateUserInput(int stopOrCross)
	{
		this.GetComponent<Animator> ().SetInteger ("StopOrCross", stopOrCross);
	}

	public void UpdateGreenLight(bool greenLightOn)
	{
		this.GetComponent<Animator> ().SetBool ("GreenLightIsOn", greenLightOn);
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals("Car"))
		{
			Debug.Log ("SCOUT HIT !");
			this.GetComponent<Animator> ().SetTrigger ("HitByCar");
		}
	}

	public void KillScout()
	{
		gameEngine.KillScout (this.gameObject);
	}

	public void ScoutIsOnBus()
	{
		gameEngine.ScoutGoesOnBus (this.gameObject);
	}

	public void GotCandies()
	{
		wantCandies = false;
		this.GetComponent<Animator> ().SetBool ("GoToShop", false);
	}
}
