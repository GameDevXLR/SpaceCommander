using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipManager : MonoBehaviour {


	public static ShipManager instance;

	public GameObject shieldImpactParticles;
	public GameObject mainShieldParticles;


	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		} else 
		{
			Destroy (gameObject);
		}
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}


	//c'est fait comme ca pour penser a plus tard: 
	//on pourra plus facilement ajouter de la perte de vie ou autre / linké des sons de jéro etc....
	public void EnableShield()
	{
		mainShieldParticles.SetActive (true);
	}

	public void DisableShield()
	{
		mainShieldParticles.SetActive (false);

	}

	public void EnableShieldImpactEffect()
	{
		shieldImpactParticles.SetActive (true);
	}

	public void DisableShieldImpactEffect()
	{
		shieldImpactParticles.SetActive (false);

	}
}
