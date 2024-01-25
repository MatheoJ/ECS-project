using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taille : IComponent
{
   public int taille;

    public Taille(int taille)
    {
        this.taille = taille;
    }
}
