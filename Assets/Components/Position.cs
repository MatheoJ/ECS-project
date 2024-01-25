using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : IComponent
{
   public Vector2 position;

    public Position(Vector2 position)
    {
        this.position = position;
    }
}
