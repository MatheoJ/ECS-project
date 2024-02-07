using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCollision : IComponent
{
    public bool inCollision;

    public InCollision(bool inCollision)
    {
        this.inCollision = inCollision;
    }
}
