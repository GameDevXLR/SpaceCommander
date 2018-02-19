using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour {

	public static WarpManager singleton;
	public Material[] skyboxList;
	public GameObject warpParticleEffectObj;

	void Awake()
	{
		if (singleton != null) {
			Destroy (this);
		} else 
		{
			singleton = this;
		}
	}
	public void ChangeTheSkybox(int index)
	{
		RenderSettings.skybox = skyboxList [index];
	}
	public void ChangeTheSkybox()
	{
//		Debug.Log (Random.Range(0, skyboxList.Length));
		RenderSettings.skybox = skyboxList[Random.Range(0, skyboxList.Length)];
	}

	public void ClearTheConsoles()
	{
		NavigationConsole.instance.ReinitializeConsole ();
	}

	public void PlayWarpParticleEffect(float timer)
	{
		StartCoroutine(ParticleProcedure(timer));
	}
	IEnumerator ParticleProcedure(float time)
	{
		warpParticleEffectObj.SetActive (true);
		yield return new WaitForSecondsRealtime (time);
		InGameManager.IGM.MakeEndOfWarpEffect ();
		foreach (ParticleSystem i in warpParticleEffectObj.GetComponentsInChildren<ParticleSystem>()) 
		{
			i.Stop (true, ParticleSystemStopBehavior.StopEmitting);
		}
		yield return new WaitForSeconds (2f);
		warpParticleEffectObj.SetActive (false);



	}
}
