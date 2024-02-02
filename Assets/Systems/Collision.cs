using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : ISystem
{
    public void UpdateSystem()
    {
        var world = World.Instance;

        // Colision with the border of the screen
        foreach (var entityID in world.entities)
        {

            if (!world.ShouldEntityBeUpdated(entityID))
            {
                continue;
            }

            var position = world.positionTab[entityID];
            var taille = world.tailleTab[entityID];

            float cameraHeight = 2f * Camera.main.orthographicSize;
            float cameraWidth = cameraHeight * Camera.main.aspect;

            float xMax = cameraWidth / 2;
            float xMin = -cameraWidth / 2;
            float yMax = cameraHeight / 2;
            float yMin = -cameraHeight / 2;

            float halfTaille = taille.taille / 2;

            if (position.position.x + halfTaille > xMax)
            {
                position.position.x = xMax - halfTaille;
                world.speedTab[entityID].speed.x *= -1;
            }
            else if (position.position.x - halfTaille < xMin)
            {
                position.position.x = xMin + halfTaille;
                world.speedTab[entityID].speed.x *= -1;
            }

            if (position.position.y + halfTaille > yMax)
            {
                position.position.y = yMax - halfTaille;
                world.speedTab[entityID].speed.y *= -1;
            }
            else if (position.position.y - halfTaille < yMin)
            {
                position.position.y = yMin + halfTaille;
                world.speedTab[entityID].speed.y *= -1;
            }

        }

        // Colision between entities
        foreach (var entityID in world.entities)
        {
            var position = world.positionTab[entityID];
            var taille = world.tailleTab[entityID];
            var speed = world.speedTab[entityID];
            var state = world.stateTab[entityID];

            int indexOfentity = world.entities.IndexOf(entityID);

            for (int i = indexOfentity+ 1; i < world.entities.Count; i++)
            {
                var position2 = world.positionTab[world.entities[i]];
                var taille2 = world.tailleTab[world.entities[i]];
                var speed2 = world.speedTab[world.entities[i]];

                CollisionResult collisionResult = CollisionUtility.CalculateCollision(position.position, speed.speed, taille.taille, position2.position, speed2.speed, taille2.taille);
                
                if (collisionResult != null)
                {
                    position.position = collisionResult.position1;
                    position2.position = collisionResult.position2;
                    speed.speed = collisionResult.velocity1;
                    speed2.speed = collisionResult.velocity2;

                    int tailleProtection = ECSController.Instance.Config.protectionSize;
                    var collisionCount = world.collisionCountTab[entityID].collisionCount;
                    var collisionCount2 = world.collisionCountTab[world.entities[i]].collisionCount;
                    var state2 = world.stateTab[world.entities[i]];

                    if (state.state != State.CircleState.Static && state2.state != State.CircleState.Static) {
                        HandleDynamicCollision(entityID, world.entities[i], taille, taille2, state, state2, tailleProtection);
                    }
                }
            }
        }
    }

    private static void HandleDynamicCollision(uint entity1, uint entity2, Taille taille1, Taille taille2, State state1, State state2, float protectionSize)
    {
        var world = World.Instance;

        // Increment the collision counter for dynamic circles of the same size
        if (taille1.taille == taille2.taille && taille1.taille == protectionSize)
        {
            world.collisionCountTab[entity1].collisionCount++;
            world.collisionCountTab[entity2].collisionCount++;
            //debug collision count
            Debug.Log("Collision count : " + world.collisionCountTab[entity1].collisionCount);
            Debug.Log("Collision count : " + world.collisionCountTab[entity2].collisionCount);
        }

        //  Change the size for the bigger circle in collision with a smaller circle
        if (state1.state != State.CircleState.Protected && state2.state != State.CircleState.Protected)
        {
            if (taille1.taille > taille2.taille)
            {
                taille1.taille++;
                taille2.taille--;
            }
            else if (taille1.taille < taille2.taille)
            {
                taille1.taille--;
                taille2.taille++;
            }
        }
        else if (state1.state == State.CircleState.Protected || state2.state == State.CircleState.Protected)
        {
            HandleProtectedCollision(entity1, entity2, taille1, taille2, state1, state2);
        }
    }

    private static void HandleProtectedCollision(uint entity1, uint entity2, Taille taille1, Taille taille2, State state1, State state2)
    {
        // Decrease the size for the bigger circle in collision with a protected circle
        if (state1.state == State.CircleState.Protected && taille1.taille < taille2.taille)
        {
            taille2.taille--;
        }
        else if (state2.state == State.CircleState.Protected && taille2.taille < taille1.taille)
        {
            taille1.taille--;
        }
    }
    public string Name { get; } = "Collision";
}
