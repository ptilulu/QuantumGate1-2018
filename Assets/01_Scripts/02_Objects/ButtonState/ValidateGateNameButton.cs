using UnityEngine;

public class ValidateGateNameButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnValidGateNameClick(); }
}
