using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : InputState
{
    /* time after which we consider a press */
    private static readonly float deltaTimePress = 0.150f;

    private static readonly float distanceMove = 2f;

    private Vector2 _startPosition;
    private float _totalTime;
    private int _fingerId;

    public Click(InputController context, Touch touch) : base(context)
    {
        _totalTime = 0;
        _startPosition = touch.position;
        _fingerId = touch.fingerId;
    }
    
    public override void down(Touch touch)
    {
        context.currentInputState = new Pinch(context, _fingerId, touch);
    }
    public override void up(Touch touch)
    {
        context.editor.CurrentState.OnClick(touch.position);

        context.currentInputState = new DefaultInputState(context);
    }
    public override void cancel(Touch touch)
    {
        context.currentInputState = new DefaultInputState(context);
    }
    public override void move(Touch touch)
    {
        if ((touch.position - _startPosition).SqrMagnitude() < distanceMove)
            return;

        context.currentInputState = new Move(context, _fingerId);
    }
    public override void time()
    {
        context.editor.CurrentState.OnDragStart(_startPosition);

        context.currentInputState = new Drag(context, _startPosition);
    }

    public override void Update(float deltaTime)
    {
        _totalTime += deltaTime;
        if (_totalTime < deltaTimePress)
            return;
        time();
    }
}
