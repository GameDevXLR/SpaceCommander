using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpInfoHolder : MonoBehaviour {

	public bool gotJumpCoordonates;
	public string jumpCodeName;
	public string jumpName;
	public GameObject navJumpableInfoObj;
	public int reqEnergyForJump;
	public Color badColor;
	public Color goodColor;

	//code d'évenement, utiliser partout: 
	public int eventCode;

	public void SetNewDestination(string codeName, string name, int energy, int code, GameObject obj)
	{
		if (navJumpableInfoObj != null) {
			if (navJumpableInfoObj.GetComponent<SignaturePropThreeD> ()) {
				navJumpableInfoObj.GetComponent<SignaturePropThreeD> ().isTransfered = false;
				navJumpableInfoObj.GetComponent<MeshRenderer>().material = navJumpableInfoObj.GetComponent<SignaturePropThreeD> ().rdyForJumpMat;
			}
		}
		eventCode = code;
		jumpCodeName = codeName;
		jumpName = name;
		navJumpableInfoObj = obj;
		reqEnergyForJump = energy;
		gotJumpCoordonates = true;
		PilotConsole.pilotC.jumpDestination.text = jumpName;
		PilotConsole.pilotC.jumpBtn.GetComponent<Image> ().color = goodColor;

	}

	public void ClearJumpDestination()
	{
		PilotConsole.pilotC.jumpDestination.text = "";
		PilotConsole.pilotC.jumpBtn.GetComponent<Image> ().color = badColor;
		gotJumpCoordonates = false;
	}
}
