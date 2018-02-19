using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SignaturePropThreeD : MonoBehaviour {

	public Text infoTxt;
//	Button sigBtn;
	public string startInfo;
	public string detailedInfo = "Blue Giant";
	public int skyboxIndexForJump;
	public float scanTime= 2f;
	public int scanRequiredEnergy;
	public Color goodStuffColor;
	public Color badStuffColor;
	public AudioSource audioS;
	public AudioClip detailedScanSnd;
	public AudioClip sendCoordonatesSnd;
	public Transform JumpPanel;
	public bool giveQuestOne;
	public int eventCode;
	public UnityEvent myEvent;
	public bool isScanned;
	public bool isTransfered;
	public Vector3 OriginOfLine;
	public Material rdyForJumpMat;
	public Material lockedForJumpMat;
	public int indexDetectedPoint;

//	public LineRenderer lR;

	// Use this for initialization

	void Start () {
		Invoke ("Initialize", .1f);
	}

	void OnMouseDown()
	{
		if (!isScanned) {
			StartDetailedScan ();
			return;
		}
		if (!isTransfered) 
		{
			TransferTheJumpInfo ();
		}
	}

	void Initialize()
	{
		switch (eventCode) 
		{
		case 1:
			myEvent.AddListener (StarEvent);
			break;
		case 2:
			myEvent.AddListener (BlackHole);
			break;
		case 3:
			myEvent.AddListener (AsteroidField);
			break;
			
		default:
			break;
		}
		ReturnToYourPosition ();
		myEvent.Invoke ();
		infoTxt.text = startInfo;
		JumpPanel = NavigationConsole.instance.JumpListPanel;
//		lR.SetPosition(0, transform.position);
//		lR.SetPosition(1, OriginOfLine);
	}

	public void ReturnToYourPosition()
	{
		if (!isScanned) {
			transform.position = NavigationConsole.instance.detectedPtParents [indexDetectedPoint].position;
			return;
		}
		if (!isTransfered) 
		{
			transform.position = NavigationConsole.instance.transferablePtParents [indexDetectedPoint].position;
		}
	}

	public void StartDetailedScan()
	{
		if (NavigationConsole.instance.isScanning || NavigationConsole.instance.isTransfering) 
		{
			return;
		}
		if (NavigationConsole.instance.currentEnergy > scanRequiredEnergy) 
		{
			StartCoroutine (DetailedScanProcedure());
//			return; // vaut mieux return car quand ca baisse : ca peut activer le else.

		} else 
		{
			StartCoroutine(ShowAlertWindow("Not enough power.", false,1.5f));
		}
	}

	IEnumerator DetailedScanProcedure()
	{
		NavigationConsole.instance.isScanning = true;
		if (giveQuestOne) {
			QuestManager.QM.QuestFindStarStep4 ();
		}
		audioS.PlayOneShot (detailedScanSnd);
		NavigationConsole.instance.ChangeEnergy(- scanRequiredEnergy);
		StartCoroutine( ShowAlertWindow ("Detailed scan in progress...", true, scanTime));
		yield return new WaitForSecondsRealtime (scanTime);
		if (!NavigationConsole.instance.hasPlayedTransferCoorVoice) 
		{
			NavigationConsole.instance.PlayTransferCoorVoice ();
		}
//		sigBtn.interactable = true;
//		sigBtn.onClick.RemoveAllListeners ();
//		sigBtn.GetComponent<Image>().color = Color.green;
//		sigBtn.onClick.AddListener (TransferTheJumpInfo);
		transform.SetParent(JumpPanel,false);
		GetComponent<MeshRenderer> ().material = rdyForJumpMat;
		infoTxt.text = detailedInfo;
		isScanned = true;
		ReturnToYourPosition ();
		NavigationConsole.instance.isScanning = false;

	}

	public void TransferTheJumpInfo()
	{
		if (NavigationConsole.instance.isTransfering ||NavigationConsole.instance.isScanning) 
		{
			return;
		}
		if (NavigationConsole.instance.currentEnergy > scanRequiredEnergy*4) 
		{
			StartCoroutine (TransferInfoProcedure ());
			return;

		} else 
		{
			StartCoroutine(ShowAlertWindow("Not enough power.",false,1.5f));
		}
	}


	IEnumerator TransferInfoProcedure()
	{
		NavigationConsole.instance.isTransfering = true;
		NavigationConsole.instance.ChangeEnergy(- scanRequiredEnergy*4);
		StartCoroutine(ShowAlertWindow( "Transfering jump coordinates...",true,(scanTime * 10)/NavigationConsole.instance.currentProc));
		yield return new WaitForSecondsRealtime ((scanTime * 10)/NavigationConsole.instance.currentProc);
		StartCoroutine(ShowAlertWindow( "Data transfer completed.",true,3f));
		PilotConsole.instance.jumpInfos.SetNewDestination (startInfo, detailedInfo, scanRequiredEnergy * 4, eventCode,skyboxIndexForJump, gameObject);
		if (giveQuestOne) {
			giveQuestOne = false;
			QuestManager.QM.EndQuestFindStar ();
		}
		audioS.PlayOneShot (sendCoordonatesSnd);
		isTransfered = true;
		GetComponent<MeshRenderer> ().material = lockedForJumpMat;
		NavigationConsole.instance.isTransfering = false;
	}

	IEnumerator ShowAlertWindow(string txt, bool isPositive, float displayTime)
	{
		if (isPositive) 
		{
			NavigationConsole.instance.loadingPanel.GetComponent<Image> ().color = goodStuffColor;

		} else 
		{
			NavigationConsole.instance.loadingPanel.GetComponent<Image> ().color = badStuffColor;

		}
		NavigationConsole.instance.loadingPanel.GetComponentInChildren<Text> ().text = txt;
		NavigationConsole.instance.loadingPanel.SetActive (true);
		yield return new WaitForSeconds (displayTime);

		NavigationConsole.instance.loadingPanel.SetActive (false);

	}

	//eventcode 1
	public void StarEvent()
	{
		detailedInfo = "Blue Giant";
		scanRequiredEnergy = 20;
		scanTime = 5;
		skyboxIndexForJump = 0;

	}

	//eventcode 2
	public void BlackHole()
	{
		detailedInfo = "Black Hole";
		scanRequiredEnergy = 24;
		scanTime = 10;
		skyboxIndexForJump = Random.Range(0,3);

	}
	//eventcode 3
	public void AsteroidField()
	{
		detailedInfo = "Asteroids Field";
		scanRequiredEnergy = 10;
		scanTime = 2;
		skyboxIndexForJump = Random.Range(4,6);

	}
}
