using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets;
using UnityEngine.PostProcessing;


public class InGameManager : MonoBehaviour {


	public static InGameManager IGM;
	public GameObject playerObj;
	public GameObject questCanvasObj;
	public GameObject controlRoomDoor;
	public GameObject lockedControlRoomDoor;
	public PostProcessingProfile PPP;
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
		Invoke( "MakeWarpEffect",2f);
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
		WarpManager.singleton.ChangeTheSkybox ();
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
}
