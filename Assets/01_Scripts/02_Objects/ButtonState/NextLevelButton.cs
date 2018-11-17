using UnityEngine;

public class NextLevelButton : MonoBehaviour {
    public Editor editor;
    public void OnClick() { editor.CurrentState.OnNextLevelClick(); }
}
