using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEventsManager : MonoBehaviour {


	public static JumpEventsManager singleton;

	void Awake()
	{
		if (singleton == null) {
			singleton = this;
		} else 
		{
			Destroy (this);
		}
	}

	public void StarEvent()
	{
//		detailedInfo = "Naine Rouge";
		//faire spawn ici l'étoile par exemple.
	}

	public void AsteroidField()
	{
		
	}
}
