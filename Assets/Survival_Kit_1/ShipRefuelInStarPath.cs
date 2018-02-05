using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipRefuelInStarPath : MonoBehaviour {

	public string pathName;
	public float time;
	void OnEnable()
	{
		iTween.MoveTo(gameObject, iTween.Hash("path",iTweenPath.GetPath(pathName),"easetype", iTween.EaseType.easeInOutSine, "time",time));
	}
	void OnDisable()
	{
		Destroy (GetComponent<iTween> ());
	}
}
