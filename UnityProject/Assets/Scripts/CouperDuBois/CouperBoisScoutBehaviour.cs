using UnityEngine;
using System.Collections;

public class CouperBoisScoutBehaviour : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals("Axe"))
		{
			this.transform.parent.GetComponent<CouperBoisScoutParentBehaviour>().ScoutDied();
		}
	}
}
