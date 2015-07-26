using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoutGroupBehaviour : MonoBehaviour {
	
	public List<Texture> scoutGroupTextures;
	public List<Texture> scoutGroupReverseTextures;

	private bool reverse;

	private int indexGroup;

	public void UpdateScoutGroup(int remainingScouts)
	{		
		// Update group
		if (remainingScouts <= 0)
		{
			this.GetComponent<Renderer>().enabled = false;
		}
		else
		{
			this.GetComponent<Renderer>().enabled = true;
			indexGroup = (remainingScouts - 1) > (scoutGroupTextures.Count-1) ? (scoutGroupTextures.Count-1) : (remainingScouts - 1);
			UpdateGroupTextureFromIndex();
		}
	}

	private void UpdateGroupTextureFromIndex()
	{			
		if (reverse)
		{
			this.GetComponent<Renderer> ().material.mainTexture = scoutGroupReverseTextures [indexGroup];
		}
		else
		{
			this.GetComponent<Renderer> ().material.mainTexture = scoutGroupTextures [indexGroup];
		}
	}

	public void SetReverse()
	{
		reverse = true;
		UpdateGroupTextureFromIndex ();
	}

	public void SetNotReverse()
	{
		reverse = false;
		UpdateGroupTextureFromIndex ();
	}
}
