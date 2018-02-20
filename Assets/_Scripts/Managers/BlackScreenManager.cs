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
//	public AudioClip UTGTransmission;
	public AudioClip reserveFaible;
	public AudioClip premiereTache;
	public TextTyper typerScript;

	public bool quickStart;
	public bool isPlayingIntro;
	void Start()
	{
		GetComponentInParent<Canvas> ().enabled = true;
		StartCoroutine (StartingProcedure ());
	}

	IEnumerator StartingProcedure()
	{
		isPlayingIntro = true;
//		ChechForStopIntroDialog ();
		if (!quickStart) {
			yield return new WaitForSecondsRealtime (1f);
			mainTxt.enabled = true;
			playerAudioSource.PlayOneShot (testTuring);
			yield return new WaitForSecondsRealtime (2.8f);
			typerScript.enabled = false;
			mainTxt.text = "Checkup system over…";
			typerScript.enabled = true;
			playerAudioSource.PlayOneShot (checkupSysteme);
			yield return new WaitForSecondsRealtime (2.5f);
			typerScript.enabled = false;
			mainTxt.text = "Initializing complex thoughts software...";
			typerScript.enabled = true;
			playerAudioSource.PlayOneShot (initialisationModule);
			yield return new WaitForSecondsRealtime (3.3f);
			typerScript.enabled = false;
			mainTxt.text = "Welcome Unit XPTDR-3.";
			typerScript.enabled = true;
			playerAudioSource.PlayOneShot (BienvenueUnit);

			yield return new WaitForSecondsRealtime (4f);
			typerScript.enabled = false;
			mainTxt.text = "You’ve been activated to secure the future of humanity in this galaxy.";
			typerScript.enabled = true;
			playerAudioSource.PlayOneShot (avenirHuma);

			//			yield return new WaitForSecondsRealtime (5f);
			//			mainTxt.text = "L’UTG te transmettra le controle des commandes au fur et a mesure.";
			//			playerAudioSource.PlayOneShot (UTGTransmission);

			yield return new WaitForSecondsRealtime (5f);
			typerScript.enabled = false;
			mainTxt.text = "Our power reserves are very low...Your first mission is to find a Blue Giant in which we could refuel.";
			typerScript.enabled = true;
			playerAudioSource.PlayOneShot (reserveFaible);
			yield return new WaitForSecondsRealtime (7f);
			typerScript.enabled = false;
		}
		isPlayingIntro = false;
		playerAudioSource.PlayOneShot (premiereTache);
		CC.enabled = true;
		CC.gameObject.GetComponent<FirstPersonController> ().enabled = true;
		typerScript.enabled = false;
		mainTxt.text =  "Go to the navigation console to select a destination.";
		typerScript.enabled = true;
		backScreenImg.CrossFadeAlpha (0, 5f, false);

		yield return new WaitForSecondsRealtime (2.5f);
		InGameManager.IGM.ActivateTheQuestPanel ();
		yield return new WaitForSecondsRealtime (2.5f);
		transform.parent.gameObject.SetActive (false);
	}

	public void Update()
	{
		if (isPlayingIntro) 
		{
			if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return)) 
			{
				StopAllCoroutines ();
				quickStart = true;
				StartCoroutine (StartingProcedure());
			}
		}
	}
	//OLD FR
//		if (!quickStart) {
//			yield return new WaitForSecondsRealtime (1f);
//			mainTxt.enabled = true;
//			playerAudioSource.PlayOneShot (testTuring);
//			yield return new WaitForSecondsRealtime (2.8f);
//			mainTxt.text = "Checkup systemes terminés...";
//			playerAudioSource.PlayOneShot (checkupSysteme);
//			yield return new WaitForSecondsRealtime (2.5f);
//			mainTxt.text = "Initialisation du module de pensée complexe.";
//			playerAudioSource.PlayOneShot (initialisationModule);
//			yield return new WaitForSecondsRealtime (3.3f);
//
//			mainTxt.text = "Bienvenue unité de colonisation XPTDR3!";
//			playerAudioSource.PlayOneShot (BienvenueUnit);
//
//			yield return new WaitForSecondsRealtime (4f);
//			mainTxt.text = "Tu as été initialisé afin d’assurer l’avenir de l’humanité dans cette galaxy.";
//			playerAudioSource.PlayOneShot (avenirHuma);
//
////			yield return new WaitForSecondsRealtime (5f);
////			mainTxt.text = "L’UTG te transmettra le controle des commandes au fur et a mesure.";
////			playerAudioSource.PlayOneShot (UTGTransmission);
//
//			yield return new WaitForSecondsRealtime (4f);
//			mainTxt.text = "Les réserves d’énergie du vaisseau sont critiques.";
//			playerAudioSource.PlayOneShot (reserveFaible);
//		yield return new WaitForSecondsRealtime (3f);
//		}
//		playerAudioSource.PlayOneShot (premiereTache);
//		CC.enabled = true;
//		CC.gameObject.GetComponent<FirstPersonController> ().enabled = true;
//		mainTxt.text =  "Ta première tache consiste a trouver une étoile ou nous pourrions nous approvisionner.";
//		backScreenImg.CrossFadeAlpha (0, 5f, false);
//
//		yield return new WaitForSecondsRealtime (2.5f);
//		InGameManager.IGM.ActivateTheQuestPanel ();
//		yield return new WaitForSecondsRealtime (2.5f);
//		transform.parent.gameObject.SetActive (false);
//	}
}
