using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSphereOfInfluence : MonoBehaviour {

	bool playerInside;
	int rechargeRate = 5;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ship") 
		{
			playerInside = true;
			ShipManager.instance.EnableShieldImpactEffect ();
			Debug.Log ("entering refuel area");
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ship") 
		{
			playerInside = false;
			ShipManager.instance.DisableShieldImpactEffect ();

			Debug.Log ("exiting refuel area");
		}
	}

	public void FixedUpdate()
	{
		if (playerInside) 
		{
			EnergyConsole.instance.stockedEnergy += rechargeRate;
		}
	}
}
