using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    /// <summary>
    /// Le contexte du jeu.
    /// </summary>
    protected T context;
    public State(T context) { this.context = context; }
}
