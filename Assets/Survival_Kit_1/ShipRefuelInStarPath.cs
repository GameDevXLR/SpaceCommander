using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipRefuelInStarPath : MonoBehaviour {

	public string pathName;
	public float time;
	public AudioSource audioS;
	public AudioClip refuelTrajVoice;
	public AudioClip endOfRefuel;


	void OnEnable()
	{
		audioS.PlayOneShot (refuelTrajVoice);
		iTween.MoveTo(gameObject, iTween.Hash("path",iTweenPath.GetPath(pathName),"easetype", iTween.EaseType.easeInOutSine, "time",time));
	}
	void OnDisable()
	{
		audioS.PlayOneShot (endOfRefuel);
		Destroy (GetComponent<iTween> ());
	}
}
