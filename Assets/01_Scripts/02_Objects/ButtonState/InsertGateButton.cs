using UnityEngine;

public class InsertGateButton : MonoBehaviour {
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnInsertGateClick(); }
}
