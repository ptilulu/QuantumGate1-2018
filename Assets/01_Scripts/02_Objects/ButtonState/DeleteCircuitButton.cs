using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCircuitButton : MonoBehaviour
{
	public Editor editor;
	public void OnClick() { editor.CurrentState.OnDeleteCircuitClick(); }
}
