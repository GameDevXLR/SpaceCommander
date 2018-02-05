using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownCinematic : MonoBehaviour {

	public Text cdTxt;
	public string baseTxt;
	public int sec;
	// Use this for initialization
	void Start () {
		cdTxt = GetComponent<Text> ();
		StartCoroutine (CountDown ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator CountDown()
	{
		while (true)
		{
			cdTxt.text = baseTxt + sec;
			sec--;
			yield return new WaitForSecondsRealtime (.95f);
			cdTxt.text = "";
			yield return new WaitForSecondsRealtime (0.05f);
		}
	}
//	void OnDisable()
//	{
//		StopCoroutine ("CountDown");
//	}
}
