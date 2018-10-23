using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : InputState
{
    private int _fingerId;

    public Move(InputController context, Touch touch) : base(context) { _fingerId = touch.fingerId; }
    public Move(InputController context, int fingerId) : base(context) { _fingerId = fingerId; }

    public override void down(Touch touch)
    {
        context.currentInputState = new Pinch(context, _fingerId, touch);
    }

    public override void up(Touch touch)
    {
        context.currentInputState = new DefaultInputState(context);
    }

    public override void move(Touch touch)
    {
        context.editor.CurrentState.OnMove(touch.deltaPosition);

    }
}
