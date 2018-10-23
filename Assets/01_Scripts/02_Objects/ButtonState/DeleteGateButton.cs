using UnityEngine;

public class DeleteGateButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnDeleteGateClick(); }
}
