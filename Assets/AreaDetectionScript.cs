using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetectionScript : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			NavigationConsole.navC.ShowOrHideTheInterface (true);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			NavigationConsole.navC.ShowOrHideTheInterface (false);
		}
	}
}
