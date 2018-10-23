using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinch : InputState
{
    private int _fingerId1;
    private int _fingerId2;

    private bool p1;
    private bool p2;

    private Vector2 oldPos1;
    private Vector2 oldPos2;

    private Vector2 newPos1;
    private Vector2 newPos2;

    public Pinch(InputController context, int fingerId, Touch touch) : base(context) {
        _fingerId1 = fingerId;
        _fingerId2 = touch.fingerId;

        oldPos1 = Input.GetTouch(fingerId).position;
        oldPos2 = touch.position;

        p1 = p2 = false;
    }

    public override void down(Touch touch)
    {
        context.currentInputState = new DefaultInputState(context);
    }

    public override void up(Touch touch)
    {
        context.currentInputState = new Move(context, (touch.fingerId == _fingerId1 ? _fingerId2 : _fingerId1));
    }

    public override void move(Touch touch)
    {
        if(touch.fingerId == _fingerId1)
        {
            p1 = true;
            newPos1 = touch.position;
        }
        if (touch.fingerId == _fingerId2)
        {
            p2 = true;
            newPos2 = touch.position;
        }
    }

    public override void Update(float deltaTime)
    {
        if (!p1)
            newPos1 = oldPos1;
        if (!p2)
            newPos2 = oldPos2;

        float oldDist = (oldPos2 - oldPos1).sqrMagnitude;
        float newDist = (newPos2 - newPos1).sqrMagnitude;
        
        context.editor.CurrentState.OnPinch(oldDist - newDist);

        oldPos1 = newPos1;
        oldPos2 = newPos2;
        p1 = p2 = false;
    }
}
