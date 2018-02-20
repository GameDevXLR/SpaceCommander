using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

//un ptit systeme de jauge 3D.

public class PersonalSlider : MonoBehaviour {

	public float sliderValue;
	public float sliderMaxValue;
//	public UnityEvent sliderEvent;
	public MeshRenderer[] renderers;
	float tmpValue;
	public Transform sliderBtn3DObj;
	public bool got3DBtnObj;

	public void ChangeTheValue(float i)
	{
		sliderValue = i;
		ActuVisual ();
	}

	void ActuVisual ()
	{
		for (int i = 0; i < renderers.Length; i++) 
		{
			renderers [i].enabled = false;
			tmpValue += (sliderMaxValue / 10f);
			if (tmpValue <= sliderValue) 
			{
				renderers [i].enabled = true;
			}
			if (got3DBtnObj && tmpValue == sliderValue) 
			{
				sliderBtn3DObj.localPosition = renderers [i].transform.localPosition;
			}
			
		}
		tmpValue = 0;
	}

}
