using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCount : IComponent
{
    public int collisionCount;

    public CollisionCount()
    {
        this.collisionCount = 0;
    }
}
