using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTime : IComponent
{
    public float cooldownTime;

    public CooldownTime()
    {
        this.cooldownTime = 0;
    }


    public CooldownTime(CooldownTime cooldownTime)
    {
        this.cooldownTime = cooldownTime.cooldownTime;
    }
}
