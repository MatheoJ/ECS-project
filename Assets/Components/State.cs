using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : IComponent
{
    public enum CircleState
    {
        Dynamic,
        Static,
        Protected,
        Cooldown
    }

    public CircleState state;

    public State(CircleState state)
    {
        this.state = state;
    }

    public State(State state)
    {
        this.state = state.state;
    }
}