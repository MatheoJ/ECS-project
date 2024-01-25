using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : ISystem
{
    public void UpdateSystem() { 
        foreach (var entityID in World.Instance.entities)
        {
            var position = World.Instance.positionTab[entityID];
            ECSController.Instance.UpdateShapePosition(entityID, position.position);
            Debug.Log("Position " + position.position);
        }

        foreach (var entityID in World.Instance.entities)
        {
            var taille = World.Instance.tailleTab[entityID];
            ECSController.Instance.UpdateShapeSize(entityID, taille.taille);
            Debug.Log("Taille " + taille.taille);
        }

        foreach (var entityID in World.Instance.entities)
        {
            Color red = new Color(1, 0, 0);
            ECSController.Instance.UpdateShapeColor(entityID, red);
        }
         

    }
    public string Name { get; } = "Draw";
}
