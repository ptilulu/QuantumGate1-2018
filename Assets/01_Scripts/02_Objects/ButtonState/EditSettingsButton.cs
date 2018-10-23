using UnityEngine;

public class EditSettingsButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnSettingsClick(); }
}
