using UnityEngine;

public class InsertColButton : MonoBehaviour {
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnInsertColClick(); }
}
