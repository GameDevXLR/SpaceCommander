using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour {

	public bool hasLooseTheGame;
	public static InGameManager IGM;
	public GameObject playerObj;
	public GameObject questCanvasObj;
	public GameObject controlRoomDoor;
	public GameObject lockedControlRoomDoor;
	public GameObject spaceShip;
	public GameObject starForRefuelObj;
	public PostProcessingProfile PPP;
	public Text mainTextDisplayer;

//	public AudioClip youLooseVoice;
//	public AudioSource effectsAudioS;

//	public Material newSkybox;
	bool isJumping;
	[Header("Reglage du PPP")]
	[SerializeField] float bloomValue;
	[SerializeField] float postExpoValue;
	[SerializeField] float chromaAbeValue;

	void Awake()
	{
		if (IGM == null) {
			IGM = this;
		} else if(IGM != null)
		{
			Destroy (this);
		}

	}
	void Start () {
		Cursor.visible = true;
		ConfigureThePPP ();
//		MakeEndOfWarpEffect (); //pour test...
//		ActivateStarEvent ();
	}

	public void ActivateTheQuestPanel()
	{
		questCanvasObj.SetActive (true);
		QuestManager.QM.StartQuestFindStar ();
	}

	public void OpenTheDoor()
	{
		controlRoomDoor.SetActive (true);
		lockedControlRoomDoor.SetActive (false);
	}

	public void MakeWarpEffect()
	{
		StartCoroutine (WarpEffectProcedure ());
		WarpManager.singleton.PlayWarpParticleEffect (10f);

	}

	public void MakeEndOfWarpEffect()
	{
		StartCoroutine (WarpEffectProcedure ());

	}

	IEnumerator WarpEffectProcedure ()
	{
		isJumping = true;
		yield return new WaitForEndOfFrame ();
		StartCoroutine (PPPProcedure());
		yield return new WaitForSecondsRealtime (3f);
		isJumping = false;
		WarpManager.singleton.ChangeTheSkybox (PilotConsole.instance.jumpInfos.skyboxIndex);
		if (PilotConsole.instance.jumpInfos.eventCode == 1) 
		{
			//si t'es une étoile...
			ActivateStarEvent();

		}
	}

	public void ActivateStarEvent()
	{
		//définir ici ce qu'il se passe quand on arrive dans un systeme ou ya une étoile...
		starForRefuelObj.SetActive(true); //active l'étoile (son visuel) : a optimiser / rendre plus random etc...
		PilotConsole.instance.YouAreNearAStart(); //dire a la console qu'on est proche d'une étoile...
	}

	IEnumerator PPPProcedure ()
	{
		float i = PPP.bloom.settings.bloom.intensity;
		float j = PPP.colorGrading.settings.basic.postExposure;
		float k = PPP.chromaticAberration.settings.intensity;
		var  tmpBloom = PPP.bloom.settings;
		var tmpColorG = PPP.colorGrading.settings;
		var tmpChromA = PPP.chromaticAberration.settings;
		while (isJumping) 
		{
			tmpBloom.bloom.intensity = PPP.bloom.settings.bloom.intensity + .20f;
			tmpColorG.basic.postExposure = PPP.colorGrading.settings.basic.postExposure + .15f;
			tmpChromA.intensity = PPP.chromaticAberration.settings.intensity + .25f;
			yield return new WaitForSecondsRealtime (.05f);
			PPP.bloom.settings = tmpBloom;
			PPP.colorGrading.settings = tmpColorG;
			PPP.chromaticAberration.settings = tmpChromA;
		}
		//change the ship position;
		spaceShip.transform.localPosition = new Vector3(-1000,220,-3850);

		StartCoroutine (PPPEndProcedure (i,j,k));

	}

	IEnumerator PPPEndProcedure(float i, float j, float k)
	{
		var tmpBloom = PPP.bloom.settings;
		var tmpColorG = PPP.colorGrading.settings;
		var tmpChromA = PPP.chromaticAberration.settings;
		while (PPP.bloom.settings.bloom.intensity > i) 
		{
			//multiplier les valeurs ci dessous par 2 par rapport a l'écran du dessus. Garder le mm facteur multiplicateur sinon BUG VISUEL
			tmpColorG.basic.postExposure = PPP.colorGrading.settings.basic.postExposure - .3f;
			tmpChromA.intensity = PPP.chromaticAberration.settings.intensity - .5f;
			tmpBloom.bloom.intensity = PPP.bloom.settings.bloom.intensity - .40f;
			yield return new WaitForSecondsRealtime (.05f);
			PPP.bloom.settings = tmpBloom;
			PPP.colorGrading.settings = tmpColorG;
			PPP.chromaticAberration.settings = tmpChromA;
		}
				tmpBloom.bloom.intensity = i;
				tmpColorG.basic.postExposure = j;
				tmpChromA.intensity = k;
				PPP.bloom.settings = tmpBloom;
				PPP.colorGrading.settings = tmpColorG;
				PPP.chromaticAberration.settings = tmpChromA;
	}

	void ConfigureThePPP()
	{
		var  tmpBloom = PPP.bloom.settings;
		var tmpColorG = PPP.colorGrading.settings;
		var tmpChromA = PPP.chromaticAberration.settings;
		tmpBloom.bloom.intensity = bloomValue;
		tmpColorG.basic.postExposure = postExpoValue;
		tmpChromA.intensity = chromaAbeValue;
		PPP.bloom.settings = tmpBloom;
		PPP.colorGrading.settings = tmpColorG;
		PPP.chromaticAberration.settings = tmpChromA;
	}

	public void EndTheGame()
	{
		//you LOOSE:
		//a completer.
		Debug.Log("you lose");
		hasLooseTheGame = true;
		StartCoroutine (LooseProcedure ());
	}

	IEnumerator LooseProcedure()
	{
		StartCoroutine (PPPProcedure());

		mainTextDisplayer.GetComponent<TextTyper> ().enabled = false;
		mainTextDisplayer.text = "YOU LOSE...You'll do better in another life";
//		effectsAudioS.PlayOneShot (youLooseVoice);
		mainTextDisplayer.enabled = true;
		mainTextDisplayer.GetComponent<TextTyper> ().enabled = true;

		yield return new WaitForSecondsRealtime (3f);
		SceneManager.LoadScene (0);
	}
}
