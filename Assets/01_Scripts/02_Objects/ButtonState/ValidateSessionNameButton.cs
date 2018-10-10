using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateSessionNameButton : MonoBehaviour {

    [SerializeField]
    private Editor editor;

    public void OnClick()
    {
        editor.CurrentState.OnValidSessionNameClick();
    }

}
