using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taille : IComponent
{
   public float taille;

    public Taille(float taille)
    {
        this.taille = taille;
    }

    public Taille(Taille taille)
    {
        this.taille = taille.taille;
    }
}
