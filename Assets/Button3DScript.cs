using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3DScript : MonoBehaviour {

	public UnityEvent onClickAction;
	public UnityEvent onEnterAction;
	public UnityEvent onExitAction;

	public cakeslice.Outline outliner;

	void Start()
	{
		outliner.enabled = false;
		if (onClickAction == null) 
		{
			onClickAction = new UnityEvent ();
			onEnterAction = new UnityEvent ();
			onExitAction = new UnityEvent ();
		}
	}

	void OnMouseDown()
	{
		onClickAction.Invoke ();
	}

	void OnMouseEnter()
	{
		outliner.enabled = true;
		onEnterAction.Invoke ();
	}

	void OnMouseExit()
	{
		outliner.enabled = false;
		onExitAction.Invoke ();
	}
}
