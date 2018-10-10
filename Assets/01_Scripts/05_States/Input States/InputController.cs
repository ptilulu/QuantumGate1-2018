using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController {
    public Editor editor;
    public InputState currentInputState;

    public InputController(Editor editor)
    {
        Input.simulateMouseWithTouches = false;
        this.editor = editor;
        currentInputState = new DefaultInputState(this);
    }

    public void Update()
    {
        /* Actually for PC */
        editor.CurrentState.Update();
        if (Input.GetMouseButtonDown(0))
            editor.CurrentState.OnClick(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
            editor.CurrentState.OnPress(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Escape))
            editor.CurrentState.OnBackButton();
        
        
        foreach (Touch touch in Input.touches)
        {
            switch(touch.phase) {
                case TouchPhase.Began:
                    currentInputState.down(touch);
                    break;
                case TouchPhase.Moved:
                    currentInputState.move(touch);
                    break;
                case TouchPhase.Ended:
                    currentInputState.up(touch);
                    break;
                case TouchPhase.Canceled:
                    currentInputState.cancel(touch);
                    break;
            }
        }

        currentInputState.Update(Time.deltaTime);
    }
}
