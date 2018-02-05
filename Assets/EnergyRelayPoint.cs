using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRelayPoint : MonoBehaviour {

//	public LineRenderer LineR;
//	public Transform originPoint;
	public int maxConnections;
	public int activeConnections;
	public Material normalMat;
	public Material fullMat;
	public MeshRenderer MeshR;


	void Start()
	{
//		LineR.SetPosition (0, transform.position);
//		LineR.SetPosition (1, transform.position);

	}

	void OnMouseOver()
	{
		if (Input.GetMouseButton (0)) {
			if (EnergyConsole.singleton.isCreatingLink) 
			{
				if (activeConnections < maxConnections) 
				{
//					LineR.SetPosition (1, originPoint.position);
					EnergyConsole.singleton.EnergyLP.AddRelayPoint(this);
				}
			}
		}
	}
}
