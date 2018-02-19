using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySlotBehaviour{

	public ConsoleName name; // c'est quel console ca ? 

	public enum ConsoleName
	{
		navigation,
		pilot,
		shield,
		weapon,

	}

	public int priorityLevel; //ordre de prio des modules pour la distribution d'énergie. pas utilisé pour le moment
	public int regenRate; //recharge rate of the console.

	//builder:
	public void EnergySlotConfig(ConsoleName consoleName, int priority, int regen)
	{
		ActuGlobalPowerConsumption (0, regen);

		name = consoleName;
		priorityLevel = priority;
		regenRate = regen;
//		Debug.Log (regenRate.ToString () + name);
		ActualizeTheAssociatedConsole ();

	}
	public void ChangeRegenRate(int regenR)
	{
		ActuGlobalPowerConsumption (regenRate, regenR);
		regenRate = regenR;
		ActualizeTheAssociatedConsole ();
	}

	void ActuGlobalPowerConsumption(int oldRegen, int newRegen)
	{
		EnergyConsole.instance.transfoEnergyRate -= oldRegen;
		EnergyConsole.instance.transfoEnergyRate += newRegen;
		EnergyConsole.instance.consoEnergyDisplayTxt.text = EnergyConsole.instance.transfoEnergyRate.ToString();
	}

	void ActualizeTheAssociatedConsole()
	{
		if (name == ConsoleName.navigation) 
		{
			NavigationConsole.instance.energyRegen = regenRate;
		}
		if (name == ConsoleName.pilot) 
		{
			PilotConsole.instance.energyRegen = regenRate;
		}
		if (name == ConsoleName.shield) 
		{
//			PilotConsole.pilotC.energyRegen = regenRate;
		}
		if (name == ConsoleName.weapon) 
		{
//			PilotConsole.pilotC.energyRegen = regenRate;
		}
	}
}
