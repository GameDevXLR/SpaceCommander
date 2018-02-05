using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayTransformator : MonoBehaviour {

	public static RelayTransformator singleton;

	public Material normalMat;
	public Material highlightMat;
	public MeshRenderer meshR;

	void Awake()
	{
		if (singleton != null) {
			Destroy (this);
		} else 
		{
			singleton = this;
		}
	}

	void OnMouseEnter()
	{
		if (Input.GetMouseButton (0)) 
		{
			if (EnergyConsole.singleton.isCreatingLink) 
			{
				if (EnergyConsole.singleton.EnergyLP.relays.Count != 0) 
				{
					meshR.material = highlightMat;
					EnergyConsole.singleton.EnergyLP.FinishLinkCreation(transform.localPosition);


				}
			}
		}
	}

	void OnMouseUp()
	{
			if (EnergyConsole.singleton.isCreatingLink) 
			{
				EnergyConsole.singleton.EnergyLP.FinishLinkCreation(transform.localPosition);
			}

	}

	void OnMouseExit()
	{
		meshR.material = normalMat;

	}

	
}
