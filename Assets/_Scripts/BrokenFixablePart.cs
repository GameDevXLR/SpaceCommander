using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class BrokenFixablePart : MonoBehaviour {

	public cakeslice.Outline partOutline;
	bool isBroken;
	public ParticleSystem[] particles;
	public MeshRenderer normalPartMesh;
	public MeshRenderer brokenPartMesh;


	//pour tester le module:
//	public void Start(){
//
//		Invoke("FixThePart",2f);
//		Invoke("BrakeThePart",5f);
//
//	}

	public void BrakeThePart()
	{
		normalPartMesh.enabled = false;
		brokenPartMesh.enabled = true;
		partOutline.eraseRenderer = false;
		isBroken = true;
		foreach (var p in particles) 
		{
			p.Play(true);
		}
	}

	public void FixThePart()
	{
//		partMesh.materials.
		normalPartMesh.enabled = true;
		brokenPartMesh.enabled = false;
		partOutline.eraseRenderer = true;
		isBroken = false;
		foreach (var p in particles) 
		{
			p.Stop (true);

		}
	}
	void OnMouseDown()
	{
		if (isBroken) {
			PilotConsole.instance.RepairTheModule ();
			FixThePart ();
		}
	}
}
