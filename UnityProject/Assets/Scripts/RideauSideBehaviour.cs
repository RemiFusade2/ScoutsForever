using UnityEngine;
using System.Collections;

public class RideauSideBehaviour : MonoBehaviour {

	public Camera currentCamera;

	public bool isRight;

	private float pixelsInsideRatio;

	// Use this for initialization
	void Start () 
	{
		pixelsInsideRatio = 0.09f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isRight)
		{

			// droite
			this.transform.position = currentCamera.ScreenToWorldPoint(new Vector3(currentCamera.pixelWidth*(1-pixelsInsideRatio), currentCamera.pixelHeight/2, 25));
		}
		else
		{
			// gauche
			this.transform.position = currentCamera.ScreenToWorldPoint(new Vector3(currentCamera.pixelWidth*pixelsInsideRatio, currentCamera.pixelHeight/2, 25));
		}
	}
}
