using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class BlackScreenManager : MonoBehaviour {

	public Text mainTxt;
	public Image backScreenImg;
	public CharacterController CC;
	public AudioSource playerAudioSource;
	public AudioClip testTuring;
	public AudioClip checkupSysteme;
	public AudioClip initialisationModule;
	public AudioClip BienvenueUnit;
	public AudioClip avenirHuma;
	public AudioClip UTGTransmission;
	public AudioClip reserveFaible;
	public AudioClip premiereTache;
	public bool quickStart;
	void Start()
	{
		GetComponentInParent<Canvas> ().enabled = true;
		StartCoroutine (StartingProcedure ());
	}

	IEnumerator StartingProcedure()
	{
		if (!quickStart) {
			yield return new WaitForSecondsRealtime (1f);
			mainTxt.enabled = true;
			playerAudioSource.PlayOneShot (testTuring);
			yield return new WaitForSecondsRealtime (2.8f);
			mainTxt.text = "Checkup systemes terminés...";
			playerAudioSource.PlayOneShot (checkupSysteme);
			yield return new WaitForSecondsRealtime (2.5f);
			mainTxt.text = "Initialisation du module de pensée complexe.";
			playerAudioSource.PlayOneShot (initialisationModule);
			yield return new WaitForSecondsRealtime (3.3f);

			mainTxt.text = "Bienvenue unité de colonisation XPTDR3!";
			playerAudioSource.PlayOneShot (BienvenueUnit);

			yield return new WaitForSecondsRealtime (4f);
			mainTxt.text = "Tu as été initialisé afin d’assurer l’avenir de l’humanité dans cette galaxy.";
			playerAudioSource.PlayOneShot (avenirHuma);

			yield return new WaitForSecondsRealtime (5f);
			mainTxt.text = "L’UTG te transmettra le controle des commandes au fur et a mesure.";
			playerAudioSource.PlayOneShot (UTGTransmission);

			yield return new WaitForSecondsRealtime (4f);
			mainTxt.text = "Les réserves d’énergie du vaisseau sont critiques.";
			playerAudioSource.PlayOneShot (reserveFaible);
		yield return new WaitForSecondsRealtime (3f);
		}
		playerAudioSource.PlayOneShot (premiereTache);
		CC.enabled = true;
		CC.gameObject.GetComponent<FirstPersonController> ().enabled = true;
		mainTxt.text =  "Ta première tache consiste a trouver une étoile ou nous pourrions nous approvisionner.";
		backScreenImg.CrossFadeAlpha (0, 5f, false);

		yield return new WaitForSecondsRealtime (2.5f);
		InGameManager.IGM.ActivateTheQuestPanel ();
		yield return new WaitForSecondsRealtime (2.5f);
		transform.parent.gameObject.SetActive (false);
	}
}
