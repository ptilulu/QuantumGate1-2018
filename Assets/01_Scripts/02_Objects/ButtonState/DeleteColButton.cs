using UnityEngine;

public class DeleteColButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnDeleteColClick(); }
}
