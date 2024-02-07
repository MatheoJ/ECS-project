using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionTime : IComponent
{
    public float protectionTime;

    public ProtectionTime()
    {
        this.protectionTime = 0;
    }


    public ProtectionTime(ProtectionTime protectionTime)
    {
        this.protectionTime = protectionTime.protectionTime;
    }
}
