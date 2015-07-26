using UnityEngine;
using System.Collections;

public class CouperBoisWoodBehaviour : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals("Axe"))
		{
			this.transform.parent.GetComponent<CouperBoisScoutParentBehaviour>().WoodHasBeenCut(this.gameObject);
		}
	}
}
