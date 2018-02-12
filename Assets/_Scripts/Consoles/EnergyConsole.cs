using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnergyConsole : MonoBehaviour {
	public bool isActive = true;
	public static EnergyConsole instance;
	public int stockedEnergy = 1000;
	public int maxStockableEnergy = 2000;
	public int transfoEnergyRate; // déduit des lignes d'énergies créées par le joueur : dans EnergySlotBehaviour
	public Text stockedEnergyDisplayTxt; 
	public Text consoEnergyDisplayTxt; 

	public EnergyLinkProcess EnergyLP;
	public bool isCreatingLink;
	public EnergySlotBehaviour NavSlot;
	public EnergySlotBehaviour PilotSlot;
	public EnergySlotBehaviour shieldSlot;
	public EnergySlotBehaviour weaponSlot;




	void Awake()
	{
		if (instance != null) {
			Destroy (this);
		} else 
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		Initialize ();
		RestartTheTransfo ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//créer ici les différents "energySlot" pour une gestion plus simple et que ce soit plus clair a extend.
	public void Initialize()
	{
		NavSlot = new EnergySlotBehaviour ();
		NavSlot.EnergySlotConfig (EnergySlotBehaviour.ConsoleName.navigation, 1, 3);
		PilotSlot = new EnergySlotBehaviour ();
		PilotSlot.EnergySlotConfig (EnergySlotBehaviour.ConsoleName.pilot, 2, 5);
		shieldSlot = new EnergySlotBehaviour ();
		shieldSlot.EnergySlotConfig (EnergySlotBehaviour.ConsoleName.shield, 3, 2);
		weaponSlot = new EnergySlotBehaviour ();
		weaponSlot.EnergySlotConfig (EnergySlotBehaviour.ConsoleName.weapon, 4, 1);
	}

	void RestartTheTransfo()
	{
		isActive = true;
		StartCoroutine (ActuTheMainBatteryPower());
	}

	IEnumerator ActuTheMainBatteryPower()
	{
		while (isActive) 
		{
			stockedEnergy -= transfoEnergyRate;
			stockedEnergyDisplayTxt.text = stockedEnergy.ToString ();
			yield return new WaitForSecondsRealtime (1f);
		}
	}
}
