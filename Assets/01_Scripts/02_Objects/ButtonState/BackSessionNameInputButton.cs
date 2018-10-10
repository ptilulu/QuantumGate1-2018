using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSessionNameInputButton : MonoBehaviour {

    [SerializeField]
    private Editor editor;

    public void OnClick()
    {
        editor.CurrentState.OnBackSessionNameClick();
    }
}
