using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCircuitButton : MonoBehaviour {
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnSaveCircuitClick(); }
}
