using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScanInterestPoint : MonoBehaviour {


	[Range(.1f,10f)]public float DetectionStrenghtReq;
	float actualStrenght;
	bool isDetected = false;
	float maxStrReach;
	public GameObject SignatureInfoPrefab;
	GameObject sigInfoObj;
	public AudioClip pointDetectedSnd;
	public AudioClip pointLockedSnd;
	public bool giveFirstQuest;
	public int predefinedEventCode;

	string[] allLetters = new string[26] {"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
	string codeName;
	void Start()
	{
		GetComponent<RectTransform>().anchoredPosition = new Vector3 (Random.Range (-200f, 200f), Random.Range (-200f, 200f), 0f);
		codeName = GenerateNames ();
//		GenerateNames ();
	}

//	public GameObject pulseObj;
	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
		if (isDetected) 
		{
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer ("UI")) 
		{
			GetComponent<AudioSource> ().PlayOneShot (pointDetectedSnd);
			Color c = GetComponent<Image> ().color;
			c.a += maxStrReach;
			c.a += other.GetComponent<ScanPulseProperties> ().strenght;
			actualStrenght = c.a;
			maxStrReach = actualStrenght;
//			Debug.Log (c.a);
			GetComponent<Image> ().color= c;
			if (actualStrenght >= DetectionStrenghtReq) {
				GetComponent<Image> ().color = Color.green;
				DetectionComplete ();
			} else 
			{
				FadeToNotDetected ();
			}
		}
	}
	public void DetectionComplete()
	{
		GetComponent<Button> ().enabled = true;
		GetComponent<AudioSource> ().PlayOneShot (pointLockedSnd);
		isDetected = true;
		sigInfoObj = Instantiate (SignatureInfoPrefab, NavigationConsole.navC.scanSignaturePanel.transform);
		NavigationConsole.navC.listOfSignatures.Add (sigInfoObj);
		sigInfoObj.GetComponent<SignaturePropThreeD> ().startInfo = codeName;
		sigInfoObj.GetComponent<SignaturePropThreeD> ().OriginOfLine = transform.position;
		sigInfoObj.GetComponent<SignaturePropThreeD> ().indexDetectedPoint = NavigationConsole.navC.indexDetectedPt;
		NavigationConsole.navC.indexDetectedPt++;
		if (giveFirstQuest) {
			QuestManager.QM.QuestFindStarStep3 ();
			sigInfoObj.GetComponent<SignaturePropThreeD> ().eventCode = 1;//code pour naine rouge
			sigInfoObj.GetComponent<SignaturePropThreeD> ().giveQuestOne = true;
			return;
		}
		if (predefinedEventCode == 0) { //en gros, est ce encore un point random ? ou bien l'event souhaité a été prédéfini a la création?

			if (!QuestManager.QM.questOneCompleted) 
			{
				sigInfoObj.GetComponent<SignaturePropThreeD> ().eventCode = Random.Range (2, 4);
				return;
			}
			sigInfoObj.GetComponent<SignaturePropThreeD> ().eventCode = Random.Range (1, 4);
		} else 
		{
			sigInfoObj.GetComponent<SignaturePropThreeD> ().eventCode = predefinedEventCode;

		}
	}
	string GenerateNames ()
	{
		string c = allLetters [Random.Range (0, allLetters.Length)];
		string d = allLetters [Random.Range (0, allLetters.Length)];
		string e = allLetters [Random.Range (0, allLetters.Length)];
		string f = allLetters [Random.Range (0, allLetters.Length)];

		string st = c + d + e + f+"-"+Random.Range(1,999);
		return st;


	}

	public void FadeToNotDetected()
	{
		StartCoroutine (FadingProcedure ());
	}

	IEnumerator FadingProcedure()
	{
		Image img = GetComponent<Image> ();
		Color c = GetComponent<Image> ().color;
		yield return new WaitForSeconds (1f);
		while (c.a >0) 
		{
			if (isDetected) 
			{
				break;
			}
			c.a -= .02f;
			img.color = c;
//			if (c.a < 0) 
//			{
//				Debug.Log ("done");
//			}
			yield return new WaitForEndOfFrame ();

		}
	}

}
