using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SignatureProperties : MonoBehaviour {

	public Text infoTxt;
//	Button sigBtn;
	public string startInfo;
	public string detailedInfo = "Naine Rouge";
	public float scanTime;
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


	// Use this for initialization

	void Start () {
		Initialize ();
	}

	void OnMouseDown()
	{
		StartDetailedScan ();
	}

	void Initialize()
	{
//		sigBtn = GetComponent<Button> ();
//		sigBtn.onClick.AddListener (StartDetailedScan);
		infoTxt.text = startInfo;
		JumpPanel = NavigationConsole.navC.JumpListPanel;
	}

	public void StartDetailedScan()
	{
		if (NavigationConsole.navC.isScanning || NavigationConsole.navC.isTransfering) 
		{
			return;
		}
		if (NavigationConsole.navC.currentEnergy > scanRequiredEnergy) 
		{
			StartCoroutine (DetailedScanProcedure());
			return; // vaut mieux return car quand ca baisse : ca peut activer le else.
			
		} else 
		{
			StartCoroutine(ShowAlertWindow("pas assez d'energie dans le module de navigation.", false,1.5f));
		}
	}

	IEnumerator DetailedScanProcedure()
	{
		NavigationConsole.navC.isScanning = true;
		if (giveQuestOne) {
			QuestManager.QM.QuestFindStarStep4 ();
		}
		audioS.PlayOneShot (detailedScanSnd);
		NavigationConsole.navC.ChangeEnergy(- scanRequiredEnergy);
		StartCoroutine( ShowAlertWindow ("Analyse détaillée de la signature énergétique en cours...", true, scanTime));
//		sigBtn.interactable = false;
		yield return new WaitForSecondsRealtime (scanTime);
//		sigBtn.interactable = true;
//		sigBtn.onClick.RemoveAllListeners ();
//		sigBtn.GetComponent<Image>().color = Color.green;
//		sigBtn.onClick.AddListener (TransferTheJumpInfo);
		transform.SetParent(JumpPanel,false);
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
		myEvent.Invoke ();
		infoTxt.text = detailedInfo;
		NavigationConsole.navC.isScanning = false;

	}

	public void TransferTheJumpInfo()
	{
		if (NavigationConsole.navC.isTransfering ||NavigationConsole.navC.isScanning) 
		{
			return;
		}
		if (NavigationConsole.navC.currentEnergy > scanRequiredEnergy*4) 
		{
			StartCoroutine (TransferInfoProcedure ());
			return;

		} else 
		{
			StartCoroutine(ShowAlertWindow("pas assez d'energie dans le module de navigation.",false,1.5f));
		}
	}


	IEnumerator TransferInfoProcedure()
	{
		NavigationConsole.navC.isTransfering = true;
//		sigBtn.interactable = false;
		NavigationConsole.navC.ChangeEnergy(- scanRequiredEnergy*4);

		StartCoroutine(ShowAlertWindow( "Transfert des coordonnées de bond en cours...",true,(scanTime * 10)/NavigationConsole.navC.currentProc));
		yield return new WaitForSecondsRealtime ((scanTime * 10)/NavigationConsole.navC.currentProc);
		StartCoroutine(ShowAlertWindow( "Destination transmise au module de pilotage.",true,3f));
		PilotConsole.pilotC.jumpInfos.SetNewDestination (startInfo, detailedInfo, scanRequiredEnergy * 4, eventCode, gameObject);
		if (giveQuestOne) {
			giveQuestOne = false;
			QuestManager.QM.EndQuestFindStar ();
		}
		audioS.PlayOneShot (sendCoordonatesSnd);
		NavigationConsole.navC.isTransfering = false;
	}

	IEnumerator ShowAlertWindow(string txt, bool isPositive, float displayTime)
	{
		if (isPositive) 
		{
			NavigationConsole.navC.loadingPanel.GetComponent<Image> ().color = goodStuffColor;
			
		} else 
		{
			NavigationConsole.navC.loadingPanel.GetComponent<Image> ().color = badStuffColor;
			
		}
		NavigationConsole.navC.loadingPanel.GetComponentInChildren<Text> ().text = txt;
		NavigationConsole.navC.loadingPanel.SetActive (true);
		yield return new WaitForSeconds (displayTime);
		NavigationConsole.navC.loadingPanel.SetActive (false);
		
	}

	//eventcode 1
	public void StarEvent()
	{
		detailedInfo = "Naine Rouge";
		scanRequiredEnergy = 20;
		scanTime = 5;

	}

	//eventcode 2
	public void BlackHole()
	{
		detailedInfo = "Trou noir";
		scanRequiredEnergy = 24;
		scanTime = 10;

	}
	//eventcode 3
	public void AsteroidField()
	{
		detailedInfo = "Champs d'asteroides";
		scanRequiredEnergy = 10;
		scanTime = 2;

	}
}

