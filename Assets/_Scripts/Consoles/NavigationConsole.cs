using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

public class NavigationConsole : MonoBehaviour {

	public static NavigationConsole instance;

	public bool moduleActive = true;
	public bool isScanning;
	public bool isTransfering;
	public cakeslice.Outline consoleOutline;
	public GameObject UIAnchorObj;

	bool alreadyOpened;
	bool isScalingUp;

	public int maxEnergy;
	public int currentEnergy;
	public int energyRegen =1;
	public Transform JumpListPanel;
	public bool hasGivenQuestOne;

	[Range(0,5)]public int currentProc;


	public GameObject scanPanel;
	public GameObject interestPointObj;

	public GameObject scanConfigPanel;
	public GameObject scanSignaturePanel;
	public GameObject loadingPanel;
	public Slider energySlider;
	public Slider procSlider;
	public List<GameObject> listOfSignatures;

	public int indexDetectedPt;
	public Transform[] detectedPtParents;
	public Transform[] transferablePtParents;

	AudioSource audioS;
	public AudioClip deepScanVoice;
	public bool hasPlayedDeepScanVoice;

	public AudioClip transferCoordinatesVoice;
	public bool hasPlayedTransferCoorVoice;

	public AudioClip lowPowerVoice;
	public bool hasPlayedLowPowerVoice;
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
	}
	void Start()
	{
		audioS = GetComponent<AudioSource> ();
		StartCoroutine (EnergyAutoRefill ());
		procSlider.value = currentProc;
		ChangeOutline ();
		ReinitializeConsole ();
	}

	public void ReinitializeConsole()
	{
		if (listOfSignatures != null) {
			foreach (GameObject obj in listOfSignatures) {
				Destroy (obj);	
			}
		}
			listOfSignatures = new List<GameObject> ();
			int j = Random.Range (5, 10);
			for (int i = 0; i < j; i++) {
				i++;
				AddInterestPointDetectable ();	
			}
		indexDetectedPt = 0;
	}

	public void PlayDeepScanVoice()
	{
		if (!hasPlayedDeepScanVoice) {
			audioS.PlayOneShot (deepScanVoice);
			hasPlayedDeepScanVoice = true;
		}
	}
	public void PlayTransferCoorVoice()
	{
		if (!hasPlayedTransferCoorVoice) {
			audioS.PlayOneShot (transferCoordinatesVoice);
			hasPlayedTransferCoorVoice = true;
		}
	}
	public void PlayLowPowerVoice()
	{
		if (!hasPlayedLowPowerVoice) {
			audioS.PlayOneShot (lowPowerVoice);
			hasPlayedLowPowerVoice = true;
		}
	}

	IEnumerator EnergyAutoRefill()
	{
		while (moduleActive) 
		{
			yield return new WaitForSecondsRealtime (1f);
			ChangeEnergy (energyRegen);
		}
	}

	public void ChangeEnergy(int changement)
	{
		currentEnergy += changement;
		if (currentEnergy > maxEnergy) 
		{
			currentEnergy = maxEnergy;
		}
		if (currentEnergy < 0) 
		{
			currentEnergy = 0;
		}
		energySlider.value = currentEnergy;
	}
	public void ChangeProc(int newProc)
	{
		currentProc = newProc;
		procSlider.value = newProc;
	}

	public void AddInterestPointDetectable()
	{
		GameObject go = Instantiate (interestPointObj, scanPanel.transform);
		go.GetComponent<ScanInterestPoint> ().DetectionStrenghtReq = Random.Range (.1f, 10f);

		if (!hasGivenQuestOne) 
		{
			go.GetComponent<ScanInterestPoint> ().giveFirstQuest = true;
			go.GetComponent<ScanInterestPoint> ().predefinedEventCode = 1; // naine rouge : necessaire pour la quete 1
			hasGivenQuestOne = true;
		}
		listOfSignatures.Add (go);

	}

	//si c'est zero : ca fera rien en gros...
	public void AddInterestPointDetectable(int i)
	{
		GameObject go = Instantiate (interestPointObj, scanPanel.transform);
		go.GetComponent<ScanInterestPoint> ().DetectionStrenghtReq = Random.Range (.1f, 10f);
		go.GetComponent<ScanInterestPoint> ().predefinedEventCode = i;
		listOfSignatures.Add (go);

	}
	public void ChangeOutline()
	{
		consoleOutline.enabled = !consoleOutline.enabled;
		////		consoleOutline.eraseRenderer = !consoleOutline.eraseRenderer;
		//		StartCoroutine (ResetOutlineEffect());
	}

	public void ShowOrHideTheInterface(bool show)
	{
		if (show) {
			StartCoroutine (ShowProcedure());
		} else {

			StartCoroutine (HideProcedure());
		}
	}

	IEnumerator ShowProcedure()
	{
		StopCoroutine ("HideProcedure");
		isScalingUp = true;
		Transform tr = UIAnchorObj.transform;
		Vector3 dif = new Vector3 (.05f, .05f, .05f);
		while (tr.localScale.x<=1f) 
		{
			tr.localScale += dif;
			yield return new WaitForSeconds (.08f);
			
		}
		isScalingUp = false;
	}
	IEnumerator HideProcedure()
	{
		StopCoroutine ("ShowProcedure");
		if (!isScalingUp) {
			Transform tr = UIAnchorObj.transform;
			Vector3 dif = new Vector3 (.05f, .05f, .05f);
			while (tr.localScale.x > .1f) {
				tr.localScale -= dif;
				yield return new WaitForSeconds (.05f);

			}
		}
	}
	//
	//	IEnumerator ResetOutlineEffect()
	//	{
	////		consoleOutline.enabled = false;
	//		yield return new WaitForEndOfFrame ();
	////		consoleOutline.enabled = true;
	//
	//	}
//
//	#region ShowHidePanels
//
//	public void ShowScanWindow()
//	{
//		if (!alreadyOpened) 
//		{
//			alreadyOpened = true;
//			QuestManager.QM.QuestFindStarStep2 ();
//		}
//		scanPanel.SetActive (true);
//		scanConfigPanel.SetActive (false);
//		scanSignaturePanel.SetActive (false);
//	}
//	public void ShowScanConfigWindow()
//	{
//		scanPanel.SetActive (false);
//		scanConfigPanel.SetActive (true);
//		scanSignaturePanel.SetActive (false);
//
//	}
//	public void ShowSignatureWindow()
//	{
//		scanPanel.SetActive (false);
//		scanConfigPanel.SetActive (false);
//		scanSignaturePanel.SetActive (true);
//
//	}
//	public void HideScanWindows()
//	{
//		scanPanel.SetActive (false);
//		scanConfigPanel.SetActive (false);
//		scanSignaturePanel.SetActive (false);
//		
//	}
//	#endregion


}
