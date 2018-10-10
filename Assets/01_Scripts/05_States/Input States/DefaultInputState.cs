using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultInputState : InputState
{
    public DefaultInputState(InputController context) : base(context) { }

    public override void down(Touch touch)
    {
        context.currentInputState = new Click(context, touch);
    }
}
