using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

            // Circle in collision
            if (world.inCollisionTab[entityID].inCollision)
            {
                color = Color.green;
                world.inCollisionTab[entityID].inCollision = false;
            } 
            // Circle about to explode next collision
            else if (world.tailleTab[entityID].taille == (ECSController.Instance.Config.explosionSize - 1) && state.state == State.CircleState.Dynamic)
            {
                color = new Color(1, 0.5f, 0);
            }
            // Circle can be protected
            else if (world.tailleTab[entityID].taille == ECSController.Instance.Config.protectionSize && state.state == State.CircleState.Dynamic)
            {
                color = new Color(0.5f, 0.5f, 1);
            }
            // Dynamic circle
            else if (state.state == State.CircleState.Dynamic)
            {
                color = Color.blue;
            }
            // Protected circle
            else if (state.state == State.CircleState.Protected)
            {
                color = Color.white;
            }
            // Circle in cooldown
            else if (state.state == State.CircleState.Cooldown)
            {
                color = Color.yellow;
            }
            // Static circle
            else if (state.state == State.CircleState.Static)
            {
                color = Color.red;
            }
            // Circle created by an explosiion
            else if (state.state == State.CircleState.Explosion)
            {
                color = Color.magenta;
                state.state = State.CircleState.Dynamic;
            }

            ECSController.Instance.UpdateShapeColor(entityID, color);
        }
         

    }
    public string Name { get; } = "Draw";
}
