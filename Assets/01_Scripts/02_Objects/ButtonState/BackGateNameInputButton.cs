using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGateNameInputButton : MonoBehaviour
{
    public Editor editor;
	public void OnClick() { editor.CurrentState.OnBackGateNameClick(); }
}
