using UnityEngine;

public class DeleteRowButton : MonoBehaviour
{
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnDeleteRowClick(); }
}
