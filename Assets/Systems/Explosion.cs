using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : ISystem
{
    public void UpdateSystem()
    {
        int tailleMax = ECSController.Instance.Config.explosionSize;

        foreach (var entityID in World.Instance.entities)
        {
            var position = World.Instance.positionTab[entityID];
            var taille = World.Instance.tailleTab[entityID];
            var speed = World.Instance.speedTab[entityID];

            if (taille.taille <= 0f)
            {
                World.Instance.entities.Remove(entityID);
                ECSController.Instance.DestroyShape(entityID);
            }

            if (taille.taille >= tailleMax)
            {
                int newTaille = ((int)(taille.taille / 4.0));
                if (newTaille < 1)
                {
                    newTaille = 1;
                }

                Vector2[] diagonals = new Vector2[4];
                diagonals[0] = new Vector2(-1, 1);
                diagonals[1] = new Vector2(1, 1);
                diagonals[2] = new Vector2(-1, -1);
                diagonals[3] = new Vector2(1, -1);

                float thirdOfTaille = taille.taille / 3;
                float speedValue = speed.speed.magnitude;

                // Create 4 new entities
                for (int i = 0; i < 4; i++)
                {
                    World.Instance.entities.Add(World.Instance.nbEntities);
                    World.Instance.positionTab[World.Instance.nbEntities] = new Position(position.position + diagonals[i] * thirdOfTaille);
                    World.Instance.tailleTab[World.Instance.nbEntities] = new Taille(newTaille);
                    World.Instance.speedTab[World.Instance.nbEntities] = new Speed(diagonals[i] * speedValue);
                    World.Instance.stateTab[World.Instance.nbEntities] = new State(State.CircleState.Dynamic);
                    World.Instance.collisionCountTab[World.Instance.nbEntities] = new CollisionCount();
                    World.Instance.protectionTimeTab[World.Instance.nbEntities] = new ProtectionTime();
                    World.Instance.cooldownTimeTab[World.Instance.nbEntities] = new CooldownTime();

                    ECSController.Instance.CreateShape(World.Instance.nbEntities, newTaille);

                    World.Instance.nbEntities++;
                }

                World.Instance.entities.Remove(entityID);
                ECSController.Instance.DestroyShape(entityID);
            }
        }
    }
    public string Name { get; } = "Explosion";
}

