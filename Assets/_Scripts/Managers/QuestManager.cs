using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour {

	public static QuestManager QM;
	public Transform questPanel;
	public GameObject questItem;
	GameObject quest1Item;
	GameObject quest2Item;
	public AudioClip questFindStar2;
	public BrokenFixablePart quest2Part;
	public AudioSource audioS;
	public AudioClip derivateurDefectueuxSnd;
	public AudioClip retourneConsoleSnd;
	public AudioClip finPremiereMissionSnd;

	public bool questOneCompleted;
	public bool questTwoCompleted;

	void Awake()
	{
		if (QM == null) {
			QM = this;
		} else 
		{
			Destroy (this);
		}
	}
	// Use this for initialization
	void Start () 
	{
		audioS = GetComponent<AudioSource> ();
	}

	#region FindYourFirstStar

	public void StartQuestFindStar()
	{
		quest1Item =  Instantiate (questItem, questPanel);
		NavigationConsole.instance.ChangeOutline ();

	}
	public void QuestFindStarStep2()
	{
		NavigationConsole.instance.ChangeOutline ();
		quest1Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Trouvez la naine rouge pour y mettre le cap.";
		GetComponent<AudioSource> ().PlayOneShot (questFindStar2);
	}
	public void QuestFindStarStep3()
	{
		quest1Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Analysez votre découverte dans Signature pour obtenir plus de détails.";
	}
	public void QuestFindStarStep4()
	{
		quest1Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Vous avez trouver la naine rouge. Transférez les coordonnées au poste de pilotage.";
	}
	public void EndQuestFindStar()
	{
		questOneCompleted = true;
		Destroy (quest1Item);
	}
	#endregion
	#region MakeYourFirstJump

	public void StartQuestFirstJump()
	{
		quest2Item = Instantiate (questItem, questPanel);
		quest2Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Charge la batterie de l'hyperdrive.";
//		PilotConsole.pilotC.jumpBtn.SetActive (true);
	}
	public void QuestFirstJumpStep2()
	{
		
		quest2Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Initialise le jump.";
	}
	public void QuestFirstJumpStep3()
	{
		quest2Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Trouve le module défectueux et répare le.";
		audioS.PlayOneShot (derivateurDefectueuxSnd);
		quest2Part.BrakeThePart ();
//		InGameManager.IGM.OpenTheDoor ();
	}

	public void QuestFirstJumpStep4()
	{
		quest2Item.transform.GetChild (0).GetComponentInChildren<Text> ().text = "Réactive le jump.";
		audioS.PlayOneShot (retourneConsoleSnd);
		PilotConsole.instance.consoleOutline.enabled = true;
	}
	public void EndQuestFirstJump()
	{
		questTwoCompleted = true;
		audioS.PlayOneShot (finPremiereMissionSnd);
		Destroy (quest2Item);
		PilotConsole.instance.consoleOutline.enabled = false;

	}
	#endregion
}
