using System.Collections.Generic;
using UnityEngine;

public class InputState : State<InputController>
{
    public InputState(InputController context) : base(context) { }
    
    public virtual void down(Touch touch) { }
    public virtual void up(Touch touch) { }
    public virtual void cancel(Touch touch) { }
    public virtual void move(Touch touch) { }
    public virtual void time() { }

    public virtual void Update(float deltaTime) { }
}
