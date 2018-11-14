using UnityEngine;

public class CompareCircuitButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnCompareCircuitClick(); }
}
