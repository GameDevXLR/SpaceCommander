using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

	public static LightManager LM;
	public MeshRenderer[] elecLightsNormal;
	public MeshRenderer[] elecLightsAlert;
//	public Color alertColor;
//	public Color baseColor;

	void Awake()
	{
		if (LM == null) {
			LM = this;
		} else 
		{
			Destroy (this);
		}
	}
//	pour tester le mode alerte activer cette fonction:
	public void Start()
	{
		StartAlertLightProcess ();
		Invoke ("StopAlertLight", 10f);
	}

	public void StartAlertLightProcess()
	{
		StartCoroutine (AlertProcedure());
	}
	public void StopAlertLight()
	{
		StopAllCoroutines ();
//		MakeTheLightNormal ();
		TurnOnAllLights ();

	}

	IEnumerator AlertProcedure()
	{
//		MakeTheLightRed ();
		while (true) 
		{
			TurnOffAllLights ();
			yield return new WaitForSecondsRealtime (.5f);
			TurnOnAllLights ();
			yield return new WaitForSecondsRealtime (.5f);

		}

	}
//	public void MakeTheLightRed()
//	{
//		foreach (Light l in elecLightsNormal) 
//		{
//			l.color = alertColor;	
//		}
//	}
//	public void MakeTheLightNormal()
//	{
//		foreach (Light l in elecLightsNormal) 
//		{
//			l.color = baseColor;	
//		}
//	}
	public void TurnOffAllLights()
	{
		foreach (MeshRenderer l in elecLightsNormal) 
		{
			l.enabled = false;
		}
		foreach (MeshRenderer l in elecLightsAlert) 
		{
			l.enabled = true;
		}
	}
	public void TurnOnAllLights()
	{
		foreach (MeshRenderer l in elecLightsNormal) 
		{
			l.enabled = true;
		}
		foreach (MeshRenderer l in elecLightsAlert) 
		{
			l.enabled = false;
		}
	}

}
