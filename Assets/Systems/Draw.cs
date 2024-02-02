using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : ISystem
{
    public void UpdateSystem() { 

        var world = World.Instance;
        foreach (var entityID in world.entities)
        {
            if (!world.ShouldEntityBeUpdated(entityID))
            {
                continue;
            }
            var position = world.positionTab[entityID];
            ECSController.Instance.UpdateShapePosition(entityID, position.position);
        }

        foreach (var entityID in world.entities)
        {
            if (!world.ShouldEntityBeUpdated(entityID))
            {
                continue;
            }
            var taille = world.tailleTab[entityID];
            ECSController.Instance.UpdateShapeSize(entityID, taille.taille);
        }

        foreach (var entityID in world.entities)
        {
            if (!world.ShouldEntityBeUpdated(entityID))
            {
                continue;
            }
            var state = world.stateTab[entityID];
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
