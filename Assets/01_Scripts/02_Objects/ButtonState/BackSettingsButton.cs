using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSettingsButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnBackSettingsClick(); }
}
