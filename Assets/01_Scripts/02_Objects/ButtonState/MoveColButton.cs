using UnityEngine;

public class MoveColButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnMoveColClick(); }
}
