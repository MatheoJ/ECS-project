using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : ISystem
{
    public void UpdateSystem()
    {
        // Colision with the border of the screen
        foreach (var entityID in World.Instance.entities)
        {
            var position = World.Instance.positionTab[entityID];
            var taille = World.Instance.tailleTab[entityID];

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
                World.Instance.speedTab[entityID].speed.x *= -1;
            }
            else if (position.position.x - halfTaille < xMin)
            {
                position.position.x = xMin + halfTaille;
                World.Instance.speedTab[entityID].speed.x *= -1;
            }

            if (position.position.y + halfTaille > yMax)
            {
                position.position.y = yMax - halfTaille;
                World.Instance.speedTab[entityID].speed.y *= -1;
            }
            else if (position.position.y - halfTaille < yMin)
            {
                position.position.y = yMin + halfTaille;
                World.Instance.speedTab[entityID].speed.y *= -1;
            }

        }

        // Colision between entities
        foreach (var entityID in World.Instance.entities)
        {
            var position = World.Instance.positionTab[entityID];
            var taille = World.Instance.tailleTab[entityID];
            var speed = World.Instance.speedTab[entityID];
            var state = World.Instance.stateTab[entityID];

            int indexOfentity = World.Instance.entities.IndexOf(entityID);

            for (int i = indexOfentity+ 1; i < World.Instance.entities.Count; i++)
            {
                var position2 = World.Instance.positionTab[World.Instance.entities[i]];
                var taille2 = World.Instance.tailleTab[World.Instance.entities[i]];
                var speed2 = World.Instance.speedTab[World.Instance.entities[i]];

                CollisionResult collisionResult = CollisionUtility.CalculateCollision(position.position, speed.speed, taille.taille, position2.position, speed2.speed, taille2.taille);
                
                if (collisionResult != null)
                {
                    position.position = collisionResult.position1;
                    position2.position = collisionResult.position2;
                    speed.speed = collisionResult.velocity1;
                    speed2.speed = collisionResult.velocity2;

                    int tailleProtection = ECSController.Instance.Config.protectionSize;
                    var collisionCount = World.Instance.collisionCountTab[entityID].collisionCount;
                    var collisionCount2 = World.Instance.collisionCountTab[World.Instance.entities[i]].collisionCount;
                    var state2 = World.Instance.stateTab[World.Instance.entities[i]];

                    if (state.state != State.CircleState.Static && state2.state != State.CircleState.Static) {
                        HandleDynamicCollision(entityID, World.Instance.entities[i], taille, taille2, state, state2, tailleProtection);
                    }


                }

            }
        }
    }

    private static void HandleDynamicCollision(uint entity1, uint entity2, Taille taille1, Taille taille2, State state1, State state2, float protectionSize)
    {
        // Increment the collision counter for dynamic circles of the same size
        if (taille1.taille == taille2.taille && taille1.taille == protectionSize)
        {
            World.Instance.collisionCountTab[entity1].collisionCount++;
            World.Instance.collisionCountTab[entity2].collisionCount++;
            //debug collision count
            Debug.Log("Collision count : " + World.Instance.collisionCountTab[entity1].collisionCount);
            Debug.Log("Collision count : " + World.Instance.collisionCountTab[entity2].collisionCount);
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
