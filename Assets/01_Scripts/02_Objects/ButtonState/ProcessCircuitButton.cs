using UnityEngine;

public class ProcessCircuitButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnProcessCircuitClick(); }
}
