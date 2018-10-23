using UnityEngine;

public class InsertRowButton : MonoBehaviour {
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnInsertRowClick(); }
}
