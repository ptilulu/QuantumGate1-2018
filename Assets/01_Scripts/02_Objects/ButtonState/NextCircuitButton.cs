using UnityEngine;

public class NextCircuitButton : MonoBehaviour {
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnNextCircuitClick(); }
}
