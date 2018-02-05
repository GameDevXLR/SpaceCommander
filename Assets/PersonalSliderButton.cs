using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersonalSliderButton : MonoBehaviour {


	public UnityEvent pressedEvent;
	public float limitDistance;
	// Use this for initialization
	void OnMouseDown()
	{
		if (Vector3.Distance (InGameManager.IGM.playerObj.transform.position, this.transform.position) < limitDistance) 
		{
			pressedEvent.Invoke ();
		}
	}
}
