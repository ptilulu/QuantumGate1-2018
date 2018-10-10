using UnityEngine;

public class BackResultButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnBackResultClick(); }
}
