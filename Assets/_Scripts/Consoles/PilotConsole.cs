using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;
public class PilotConsole : MonoBehaviour {
	
	public static PilotConsole instance;
	public JumpInfoHolder jumpInfos;

	public ShipRefuelInStarPath shipPath;
	public bool moduleActive = true;
	public bool isBroken;
	public bool isNearAStar;
	public cakeslice.Outline consoleOutline;
	bool alreadyOpened;
	bool questPartNotActivated = true;
	bool firstTimeHyperdriveIsCharged = true;
	public int maxEnergy;
	public int currentEnergy;
	public int energyRegen =1;

	public int hyperdriveTmp;
	[Range(0,101)]public int coolingRegen;
	[Range(0,99)]public int hyperdriveOverheatPrct;
	public int moteurTmp;
	public int hyperdriveBattery;
	public int moteurBattery;
	public int hyperdriveBatteryRegen;
	public bool hyperdriveFull;
	public int moteurBatteryRegen;
	public bool moteurActive= true;
	public bool hyperdriveActive = true;
	public bool coolingactive = true;

	public Button reloadInStarBtn;
	public Button hyperdriveActivationBtn;
	public Color turnedOffColor;
	public Color turnedOnColor;

	public int consommation;

	public Slider consoSlider;
	public Slider hyperdriveTmpSlider;
	public Slider moteurTmpSlider;
	public Slider hyperdriveBatterySlider;
	public Slider moteurBatterySlider;
	public Slider hyperdriveRegenSlider;
	public Slider moteurRegenSlider;
	public Slider coolingRegenSlider;

	[Range(0,5)]public int currentProc;


	public GameObject moteurPanel;
	public GameObject paramPanel;
	public GameObject TrajectoryPanel;
	public GameObject loadingPanel;
	public Slider energySlider;
	public Slider procSlider;
	public GameObject jumpBtn;
	public Text jumpDestination;

	public AudioClip rireErreurSnd;
	public AudioClip alertSnd;

	bool questOneDone;

	public Color goodStuffColor;
	public Color badStuffColor;


	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
	}
	void Start()
	{
		StartCoroutine (EnergyAutoRefill ());
		procSlider.value = currentProc;
		consoleOutline.enabled = false;
	}
		
	#region StarRefuel


	//dire a cette console qu'on est a proximité d'une étoile!
	//rendre le bouton reloadinstar interactif.
	public void YouAreNearAStart()
	{
		isNearAStar = true;
		reloadInStarBtn.interactable = true;
	}


	//met toi sur le path qui conduit a la recharge dans l'étoile.
	public void EngageRefuelInStarTrajectory()
	{
		if (isNearAStar) {
			shipPath.enabled = true;
			reloadInStarBtn.interactable = false;
			//ajouter ici le lancement des evenements liés a l'étoile.
		}
	}


	#endregion
	IEnumerator EnergyAutoRefill()
	{
		while (moduleActive) {
			yield return new WaitForSecondsRealtime (1f);
			if (currentEnergy < consommation - energyRegen) {
				NotEnoughPower ();
		
			} else {
				ChangeEnergy (energyRegen - consommation);
				if (hyperdriveActive) {
					ActuHyperdriveTmp ();
					if (hyperdriveFull && hyperdriveBattery != hyperdriveBatterySlider.maxValue) 
					{
						hyperdriveFull = false;
					}
					hyperdriveBattery += hyperdriveBatteryRegen;
					if (hyperdriveBattery > hyperdriveBatterySlider.maxValue) {
						hyperdriveBattery = (int)hyperdriveBatterySlider.maxValue;
					}
					if (hyperdriveBattery == hyperdriveBatterySlider.maxValue && !hyperdriveFull) 
					{
						if(firstTimeHyperdriveIsCharged){
							firstTimeHyperdriveIsCharged = false;
							QuestManager.QM.QuestFirstJumpStep2 ();
						}
						hyperdriveFull = true;
						StartCoroutine(ShowAlertWindow("Hyperdrive chargé.",1f, true));
					}
					hyperdriveBatterySlider.value = hyperdriveBattery;
				}
				if (moteurActive) {
					moteurBattery += moteurBatteryRegen;
					if (moteurBattery > moteurBatterySlider.maxValue) {
						moteurBattery = (int)moteurBatterySlider.maxValue;
					}
					moteurBatterySlider.value = moteurBattery;
				}
				if (coolingactive) 
				{
					if (hyperdriveTmp > coolingRegen) 
					{
						CoolDownHyperdrive (coolingRegen);
					}
					if (moteurTmp > coolingRegen) 
					{
						CoolDownMoteur (coolingRegen);
					}
				}
			}
		}

	}

	public void CoolDownHyperdrive(int tmpDown)
	{
		hyperdriveTmp -= tmpDown;
		hyperdriveTmpSlider.value = hyperdriveTmp;
	}
	public void CoolDownMoteur(int tmpDown)
	{
		moteurTmp -= tmpDown;
		moteurTmpSlider.value = moteurTmp;
	}

	public void ActuHyperdriveTmp()
	{
		
			hyperdriveTmp += (hyperdriveBattery / 10) + hyperdriveBatteryRegen;
			if (hyperdriveTmp > hyperdriveTmpSlider.maxValue * hyperdriveOverheatPrct / 100) {
				HyperdriveOverheating ();
			}
			if (hyperdriveTmp > hyperdriveTmpSlider.maxValue) {
				hyperdriveTmp = (int)hyperdriveTmpSlider.maxValue;

			}

		hyperdriveTmpSlider.value = hyperdriveTmp;
	}
	public void HyperdriveOverheating ()
	{
		int x = Random.Range (0, 101);
		if (x > hyperdriveOverheatPrct) 
		{
			ShutDownHyperdrive ();
		}
	}

	public void ShutDownHyperdrive()
	{
		StartCoroutine( ShowAlertWindow ("Arret forcé de l'hyperdrive!", 3f,false));
		hyperdriveRegenSlider.value = 0;
		hyperdriveBatteryRegen = 0;
		hyperdriveRegenSlider.interactable = false;
		hyperdriveActivationBtn.GetComponent<Image> ().color = turnedOffColor;
		hyperdriveActive = false;
		RecalculateConso ();
	}

	public void RestartHyperdrive()
	{
		if (hyperdriveActive) 
		{
			return;
		}
		if (hyperdriveTmp > hyperdriveTmpSlider.maxValue * hyperdriveOverheatPrct / 100) {
			StartCoroutine (ShowAlertWindow ("L'Hyperdrive doit refroidir.", 3f,false));

		} else 
		{
			StartCoroutine (ShowAlertWindow ("Démarrage de l'Hyperdrive...", 20f/currentProc,true));
			hyperdriveActive = true;
			hyperdriveActivationBtn.GetComponent<Image> ().color = turnedOnColor;

			hyperdriveRegenSlider.interactable = true;

		}
	}
	public void RepairTheModule()
	{
		isBroken = false;
		LightManager.LM.StopAlertLight ();
		QuestManager.QM.QuestFirstJumpStep4 ();
	}

	public void NotEnoughPower()
	{
		moteurBatteryRegen = 0;
		hyperdriveBatteryRegen = 0;
		coolingRegen = 0;
		moteurRegenSlider.value = 0;
		hyperdriveRegenSlider.value = 0;
		coolingRegenSlider.value = 0;
		StartCoroutine( ShowAlertWindow ("Pas assez d'énergie!", 1.5f,false));
//		ShowParamWindow ();
		RecalculateConso ();
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

	public void ChangeMoteurBatteryRegen(float newRegen)
	{
		moteurBatteryRegen = (int)newRegen;
		RecalculateConso ();
	}
	public void ChangeHyperdriveBatteryRegen(float newRegen)
	{
		if (!alreadyOpened) 
		{
			alreadyOpened = true;
			QuestManager.QM.StartQuestFirstJump ();

		}
		hyperdriveBatteryRegen = (int)newRegen;
		RecalculateConso ();

	}
	public void ChangeCoolingRegen(float newRegen)
	{
		coolingRegen = (int)newRegen*10;
		RecalculateConso ();

	}
	void RecalculateConso()
	{
		consommation = moteurBatteryRegen + hyperdriveBatteryRegen+ (coolingRegen/10);
		consoSlider.value = consommation;

	}
	public void ChangeProc(int newProc)
	{
		currentProc = newProc;
		procSlider.value = newProc;
	}

	public void MakeAJump()
	{
		if (!jumpInfos.gotJumpCoordonates) 
		{
			StartCoroutine (ShowAlertWindow ("Aucune destination fournie par la Navigation.", 2f,false));
			return;
		}
		
		if (hyperdriveBattery < jumpInfos.reqEnergyForJump) 
		{
			StartCoroutine (ShowAlertWindow ("L'hyperdrive n'a pas assez d'énergie en stock.", 2f,false));
//			GetComponent<AudioSource> ().PlayOneShot (rireErreurSnd);
			return;
		} else 
		{
			if (questPartNotActivated) 
			{
				QuestManager.QM.QuestFirstJumpStep3 ();
				GetComponent<AudioSource> ().PlayOneShot (alertSnd);
				hyperdriveBattery -= 30;
				hyperdriveBatteryRegen = 0;
				hyperdriveBatterySlider.value -= 30;
				StartCoroutine( ShowAlertWindow( "Dérivateur Défectueux. Baisse du niveau d'énergie de l'Hyperdrive.",3f,false));
				LightManager.LM.StartAlertLightProcess ();
				RecalculateConso ();
				isBroken = true;
				questPartNotActivated = false;
				return;
			}
			MakeTheFinalJump ();

		}
	}

	public void MakeTheFinalJump()
	{
		if (isBroken) {
			StartCoroutine( ShowAlertWindow ("Composant Défectueux. IMPOSSIBLE!", 2f,false));

		} else 
		{
			if (!QuestManager.QM.questOneCompleted && jumpInfos.jumpName != "Naine Rouge") 
			{
				StartCoroutine( ShowAlertWindow ("IMPOSSIBLE!Naine Rouge recquise.", 2f,false));
				return;
			}
			InGameManager.IGM.MakeWarpEffect ();
			hyperdriveBattery -= jumpInfos.reqEnergyForJump;
			jumpInfos.ClearJumpDestination ();
			NavigationConsole.instance.ReinitializeConsole ();
			if (!questOneDone) {
				QuestManager.QM.EndQuestFirstJump ();
				questOneDone = true;
			}
		}
	}


	IEnumerator ShowAlertWindow(string txt, float timer, bool isPositive)
	{
		if (isPositive) 
		{
			loadingPanel.GetComponent<Image> ().color = goodStuffColor;

		} else 
		{
			loadingPanel.GetComponent<Image> ().color = badStuffColor;

		}
		loadingPanel.GetComponentInChildren<Text> ().text = txt;
		loadingPanel.SetActive (true);
		yield return new WaitForSecondsRealtime (timer);
		loadingPanel.SetActive (false);

	}
	#region ShowHidePanels

	public void ShowMoteurWindow()
	{
		moteurPanel.SetActive (true);
		paramPanel.SetActive (false);
		TrajectoryPanel.SetActive (false);
	}
	public void ShowParamWindow()
	{

		moteurPanel.SetActive (false);
		paramPanel.SetActive (true);
		TrajectoryPanel.SetActive (false);

	}
	public void ShowTrajectoryWindow()
	{
		moteurPanel.SetActive (false);
		paramPanel.SetActive (false);
		TrajectoryPanel.SetActive (true);

	}
	public void HideScanWindows()
	{
		moteurPanel.SetActive (false);
		paramPanel.SetActive (false);
		TrajectoryPanel.SetActive (false);

	}
	#endregion


}
