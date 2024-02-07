using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTimeCode : IComponent
{
    public float time;

    public  SaveTimeCode()
    {
            time = Time.time;
    }

    public SaveTimeCode(SaveTimeCode time)
    {
        this.time = time.time;
    }
}
