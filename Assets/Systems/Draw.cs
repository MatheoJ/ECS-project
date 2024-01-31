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
            //Debug.Log("Position " + position.position);
        }

        foreach (var entityID in World.Instance.entities)
        {
            var taille = World.Instance.tailleTab[entityID];
            ECSController.Instance.UpdateShapeSize(entityID, taille.taille);
            //Debug.Log("Taille " + taille.taille);
        }

        foreach (var entityID in World.Instance.entities)
        {
            var state = World.Instance.stateTab[entityID];
            Color color = new Color();
            if (state.state == State.CircleState.Dynamic)
            {
                color = Color.green;
            }
            else if (state.state == State.CircleState.Protected)
            {
                color = Color.blue;
            }
            else if (state.state == State.CircleState.Cooldown)
            {
                color = Color.yellow;
            }
            else if (state.state == State.CircleState.Static)
            {
                color = Color.red;
            }
            ECSController.Instance.UpdateShapeColor(entityID, color);
        }
         

    }
    public string Name { get; } = "Draw";
}
