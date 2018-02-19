using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnergyLinkProcess : MonoBehaviour {
	public EnergySlotBehaviour.ConsoleName consoleType;
	public int currentLinkPower; // combien d'énergie ce link transmet
	[SerializeField] Text powerTxt;
	public GameObject TrailObj;
	public List<EnergyRelayPoint> relays;
	public LineRenderer relaysLine;
	bool isBeingDrag;
	private Vector3 screenPoint;
	private Vector3 offset;
	Vector3 trailInitPos;


	public bool desiredLinkComplete;

	void Start()
	{
		trailInitPos = TrailObj.transform.localPosition;
	}

	void Update()
	{
		if (isBeingDrag) 
		{
			if(!Input.GetMouseButton(0))
			{
				EnergyConsole.instance.isCreatingLink = false;
				if (!desiredLinkComplete) 
				{
					ReinitializeTrail ();
					relaysLine.positionCount = 1;
				}
				isBeingDrag = false;
			}
		}
	}

//	void LateUpdate()
//	{
//		if (desiredLinkComplete) 
//		{
//			if (relays.Count == 0) 
//			{
//				return;
//			}
//			Vector3[] oldPos = new Vector3[ relays.Count+1];
//
//			for (int i = 0; i < relays.Count; i++) 
//			{
//
//				if (i == 0) 
//				{
//					Debug.Log ("to");
//					oldPos.SetValue (transform.position, i);
//				} else if(i>0)
//				{
//					oldPos.SetValue (relays [i].transform.localPosition, i);
//				}
//			}
//			oldPos.SetValue (RelayTransformator.singleton.transform.localPosition,relays.Count);
//			relaysLine.positionCount = oldPos.Length;
//			relaysLine.SetPositions (oldPos);
//		}
//	}

	void OnMouseDown()
	{
		StartNewLinkCreation ();
		isBeingDrag = true;
		EnergyConsole.instance.EnergyLP = this;
		EnergyConsole.instance.isCreatingLink = true;
		TrailObj.GetComponent<TrailRenderer> ().enabled = true;
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag()
	{
		if (desiredLinkComplete == false) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			TrailObj.transform.position = curPosition;

			//si le joueur lache
			if (!Input.GetMouseButton (0)) {
				EnergyConsole.instance.isCreatingLink = false;
				if (!desiredLinkComplete) {
					relaysLine.positionCount = 1;
					ReinitializeTrail ();
				}
			}
		}
	}
	public void AddRelayPoint(EnergyRelayPoint rPt)
	{
		if(relays.Contains(rPt))
			{
				return;
			}

		relays.Add (rPt);
		AddANewPoint (rPt.transform.localPosition);

	}

	public void AddANewPoint(Vector3 newPos)
	{
		Vector3[] oldPos = new Vector3[ relaysLine.positionCount+1];
		for (int i = 0; i < relaysLine.positionCount; i++) 
		{
			if (i == 0) 
			{
				oldPos.SetValue (transform.localPosition, i);
			} else 
			{
				oldPos.SetValue (relaysLine.GetPosition(i),i);
			}
		}
		oldPos.SetValue (newPos,relaysLine.positionCount);
		relaysLine.positionCount = oldPos.Length;
		relaysLine.SetPositions (oldPos);
	}

	public void StartNewLinkCreation()
	{
		foreach (EnergyRelayPoint item in relays) 
		{
//			item.LineR.SetPosition (1, item.transform.position);
			if (item.activeConnections == item.maxConnections) 
			{
				//faire ici ce qui se passe si le relai redescend en dessous de son nombre max de co.
				item.MeshR.material = item.normalMat;
			}
			item.activeConnections--;
		}

		relaysLine.positionCount = 1;
		desiredLinkComplete = false;
		relays.Clear();

		currentLinkPower = 0;
		switch (consoleType) {

		case EnergySlotBehaviour.ConsoleName.navigation:
			EnergyConsole.instance.NavSlot.ChangeRegenRate (currentLinkPower);
			break;

		case EnergySlotBehaviour.ConsoleName.pilot:
			EnergyConsole.instance.PilotSlot.ChangeRegenRate (currentLinkPower);
			break;
		case EnergySlotBehaviour.ConsoleName.shield:
			EnergyConsole.instance.shieldSlot.ChangeRegenRate (currentLinkPower);
			break;
		default:
			Debug.Log("Le link n'est associé a aucune console!Etonnant!");
			break;
		}
		ActualizeMyVisualLinkPower ();

	}

	public void FinishLinkCreation(Vector3 endPos)
	{
		ReinitializeTrail ();
		isBeingDrag = false;
		EnergyConsole.instance.isCreatingLink = false;
		desiredLinkComplete = true;
		AddANewPoint (endPos);
		foreach (EnergyRelayPoint item in relays) 
		{
			currentLinkPower += item.maxConnections;
			item.activeConnections++;
			if (item.activeConnections == item.maxConnections) 
			{
				item.MeshR.material = item.fullMat;
			}
		}
		switch (consoleType) {

		case EnergySlotBehaviour.ConsoleName.navigation:
			EnergyConsole.instance.NavSlot.ChangeRegenRate (currentLinkPower);
			break;
		
		case EnergySlotBehaviour.ConsoleName.pilot:
			EnergyConsole.instance.PilotSlot.ChangeRegenRate (currentLinkPower);
			break;
		case EnergySlotBehaviour.ConsoleName.shield:
			EnergyConsole.instance.shieldSlot.ChangeRegenRate (currentLinkPower);
			break;
		default:
			break;
		}
		ActualizeMyVisualLinkPower ();
//		StartCoroutine (ActuTheLink ());
	}

	//Bugger a mort :(
	IEnumerator ActuTheLink()
	{
		yield return new WaitForSeconds (.2f);
		while (desiredLinkComplete) 
		{
			Debug.Log ("truite");
			if (relays.Count == 0) 
			{
				break;
			}
			Vector3[] oldPos = new Vector3[ relays.Count+1];

			for (int i = 0; i < relays.Count; i++) 
			{
				
				if (i == 0) {
					oldPos.SetValue (transform.position, i);
				} else {
					oldPos.SetValue (relays [i].transform.position, i);
				}
			}
			oldPos.SetValue (RelayTransformator.singleton.transform.position,relays.Count);
			relaysLine.positionCount = oldPos.Length;
			relaysLine.SetPositions (oldPos);

			yield return new WaitForSeconds (.05f);
//			break;
		}
	}

	public void ReinitializeTrail()
	{
		TrailObj.GetComponent<TrailRenderer> ().Clear();
		TrailObj.GetComponent<TrailRenderer> ().enabled = false;
		TrailObj.transform.localPosition = trailInitPos;
	}

	public void ActualizeMyVisualLinkPower()
	{
		powerTxt.text = currentLinkPower.ToString();	
	}


}
