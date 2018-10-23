using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : InputState
{
    public Drag(InputController context, Vector2 position) : base(context) { }
    
    public override void up(Touch touch)
    {
        context.editor.CurrentState.OnDragEnd(touch.position);

        context.currentInputState = new DefaultInputState(context);
    }

    public override void down(Touch touch)
    {
        context.editor.CurrentState.OnDragCancelled(touch.position);

        context.currentInputState = new DefaultInputState(context);
    }

    public override void move(Touch touch)
    {
        context.editor.CurrentState.OnDrag(touch.position);
    }
}
