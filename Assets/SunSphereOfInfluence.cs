using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSphereOfInfluence : MonoBehaviour {

	bool playerInside;
	int rechargeRate = 5;
	public AudioClip backgroundMusic;
//	public AudioClip actionMusic;
	public AudioSource audioS;
//	public ParticleSystem shieldImpact; //pour faire des burst un de c 4.

	public void Start()
	{
//		audioS = GetComponent<AudioSource> (); // nan en fait..
		audioS = InGameManager.IGM.GetComponent<AudioSource>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ship") 
		{
			playerInside = true;
			ShipManager.instance.EnableShieldImpactEffect ();
			Debug.Log ("entering refuel area");

//			audioS.enabled = true;
//			InGameManager.IGM.GetComponent<AudioSource> ().enabled = false;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ship") 
		{
			playerInside = false;
			ShipManager.instance.DisableShieldImpactEffect ();
			audioS.clip = backgroundMusic;
			audioS.Play ();

			Debug.Log ("exiting refuel area");
//			audioS.enabled = false;
//			InGameManager.IGM.GetComponent<AudioSource> ().enabled = true;

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
