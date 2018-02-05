using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannerControl : MonoBehaviour {

	public GameObject pulseObj;
	public float rangeLimit;
	public float sigStr;
	public float strLoss;
//	public Slider strLossSlider;
	public PersonalSlider strLossSlider3D;
	public int pulseEnergy = 5;
	bool alreadyGaveQuest;
	public LayerMask targetable;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast (ray, out hit, 5, targetable))
			{
				if(NavigationConsole.navC.currentEnergy < pulseEnergy)
				{
					return;
				}
				if (!alreadyGaveQuest) 
				{
					alreadyGaveQuest = true;
					QuestManager.QM.QuestFindStarStep2 ();
				}
				NavigationConsole.navC.ChangeEnergy (-pulseEnergy);
				GameObject go = Instantiate (pulseObj);
				go.transform.position = hit.point;
				go.transform.parent = transform;
				ScanPulseProperties SPP = go.GetComponent<ScanPulseProperties> ();
				SPP.rangeLimit = rangeLimit;
				SPP.strenght = sigStr;
				SPP.strenghtLoss = strLoss;
			}

		}
	}

	public void ChangeRange(float r)
	{
		rangeLimit = r;
		ActualizeStrLoss ();
	}
	public void ChangeStr( float str)
	{
		sigStr = str;
		ActualizeStrLoss ();

	}
	public void ActualizeStrLoss()
	{
		strLoss = sigStr / (301f-(rangeLimit * 1000));
//		strLossSlider.value = strLoss;
		strLossSlider3D.ChangeTheValue (strLoss);
	}
}
