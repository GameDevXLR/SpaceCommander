using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpInfoHolder : MonoBehaviour {
	public int skyboxIndex;
	public bool gotJumpCoordonates;
	public string jumpCodeName;
	public string jumpName;
	public GameObject navJumpableInfoObj;
	public int reqEnergyForJump;
	public Color badColor;
	public Color goodColor;

	//code d'évenement, utiliser partout: 
	public int eventCode;

	public void SetNewDestination(string codeName, string name, int energy, int code,int skyboxIdx, GameObject obj)
	{
		if (navJumpableInfoObj != null) {
			if (navJumpableInfoObj.GetComponent<SignaturePropThreeD> ()) {
				navJumpableInfoObj.GetComponent<SignaturePropThreeD> ().isTransfered = false;
				navJumpableInfoObj.GetComponent<MeshRenderer>().material = navJumpableInfoObj.GetComponent<SignaturePropThreeD> ().rdyForJumpMat;
			}
		}
		skyboxIndex = skyboxIdx;
		eventCode = code;
		jumpCodeName = codeName;
		jumpName = name;
		navJumpableInfoObj = obj;
		reqEnergyForJump = energy;
		gotJumpCoordonates = true;
		PilotConsole.instance.jumpDestination.text = jumpName;
		PilotConsole.instance.jumpBtn.GetComponent<Image> ().color = goodColor;

	}

	public void ClearJumpDestination()
	{
		PilotConsole.instance.jumpDestination.text = "";
		PilotConsole.instance.jumpBtn.GetComponent<Image> ().color = badColor;
		gotJumpCoordonates = false;
	}
}
