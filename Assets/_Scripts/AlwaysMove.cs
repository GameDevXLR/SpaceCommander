using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysMove : MonoBehaviour {

	public int speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (transform.forward * Time.deltaTime * speed);
	}
}
