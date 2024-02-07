using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : IComponent
{
    public Vector2 speed;

    public Speed(Vector2 speed)
    {
        this.speed = speed;
    }

    public Speed(Speed speed)
    {
        this.speed = speed.speed;
    }
}
