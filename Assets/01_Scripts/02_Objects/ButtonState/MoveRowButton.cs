using UnityEngine;

public class MoveRowButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnMoveRowClick(); }
}
