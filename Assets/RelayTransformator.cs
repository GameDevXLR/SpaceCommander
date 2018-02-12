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
			if (EnergyConsole.instance.isCreatingLink) 
			{
				if (EnergyConsole.instance.EnergyLP.relays.Count != 0) 
				{
					meshR.material = highlightMat;
					EnergyConsole.instance.EnergyLP.FinishLinkCreation(transform.localPosition);


				}
			}
		}
	}

	void OnMouseUp()
	{
			if (EnergyConsole.instance.isCreatingLink) 
			{
				EnergyConsole.instance.EnergyLP.FinishLinkCreation(transform.localPosition);
			}

	}

	void OnMouseExit()
	{
		meshR.material = normalMat;

	}

	
}
