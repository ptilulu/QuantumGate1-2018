using UnityEngine;

public class PreviousCircuitButton : MonoBehaviour {

    public Editor editor;
    public void OnClick() { editor.CurrentState.OnPreviousCircuitClick(); }
}
