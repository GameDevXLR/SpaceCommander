using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanPulseProperties : MonoBehaviour {

	[Range(0.02f,0.3f)]public float rangeLimit;
	[Range(.1f,.99f)]public float strenght;
	[Range(.001f,.01f)]public float strenghtLoss;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		strenght -= strenghtLoss;
		if (strenght <= .1f) 
		{
			strenght = .1f;
		}
		transform.localScale *= 1.02f;
		if (transform.localScale.x > rangeLimit) 
		{
			Destroy (gameObject);
		}
			
	}
}
