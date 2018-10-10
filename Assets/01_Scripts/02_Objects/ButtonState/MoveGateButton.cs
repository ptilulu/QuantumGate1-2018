using UnityEngine;

public class MoveGateButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnMoveGateClick(); }
}
