using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSessionButton : MonoBehaviour {

	[SerializeField]
	private Editor editor;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () {}

	public void OnClick() { editor.CurrentState.OnSaveSandBoxSession (); }
}
