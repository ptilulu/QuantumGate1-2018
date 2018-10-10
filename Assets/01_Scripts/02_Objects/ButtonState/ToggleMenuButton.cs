using UnityEngine;

public class ToggleMenuButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnToggleMenuClick(); }
}
