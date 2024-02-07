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
            var size = world.sizeTab[entityID];

            float cameraHeight = 2f * Camera.main.orthographicSize;
            float cameraWidth = cameraHeight * Camera.main.aspect;

            float xMax = cameraWidth / 2;
            float xMin = -cameraWidth / 2;
            float yMax = cameraHeight / 2;
            float yMin = -cameraHeight / 2;

            float halfSize = size.size / 2;

            if (position.position.x + halfSize > xMax)
            {
                position.position.x = xMax - halfSize;
                world.speedTab[entityID].speed.x *= -1;
            }
            else if (position.position.x - halfSize < xMin)
            {
                position.position.x = xMin + halfSize;
                world.speedTab[entityID].speed.x *= -1;
            }

            if (position.position.y + halfSize > yMax)
            {
                position.position.y = yMax - halfSize;
                world.speedTab[entityID].speed.y *= -1;
            }
            else if (position.position.y - halfSize < yMin)
            {
                position.position.y = yMin + halfSize;
                world.speedTab[entityID].speed.y *= -1;
            }

        }

        // Colision between entities
        foreach (var entityID in world.entities)
        {
            var position = world.positionTab[entityID];
            var size = world.sizeTab[entityID];
            var speed = world.speedTab[entityID];
            var state = world.stateTab[entityID];

            int indexOfentity = world.entities.IndexOf(entityID);

            for (int i = indexOfentity + 1; i < world.entities.Count; i++)
            {
                var position2 = world.positionTab[world.entities[i]];
                var size2 = world.sizeTab[world.entities[i]];
                var speed2 = world.speedTab[world.entities[i]];

                CollisionResult collisionResult = CollisionUtility.CalculateCollision(position.position, speed.speed, size.size, position2.position, speed2.speed, size2.size);

                if (collisionResult != null)
                {
                    position.position = collisionResult.position1;
                    position2.position = collisionResult.position2;
                    speed.speed = collisionResult.velocity1;
                    speed2.speed = collisionResult.velocity2;

                    int sizeProtection = ECSController.Instance.Config.protectionSize;
                    var state2 = world.stateTab[world.entities[i]];
                    var inCollision = world.inCollisionTab[entityID];
                    var inCollision2 = world.inCollisionTab[world.entities[i]];

                    // set in Collision to true if not static (for the green color)
                    if (state.state != State.CircleState.Static)
                    {
                        inCollision.inCollision = true;

                    }
                    if (state2.state != State.CircleState.Static)
                    {
                        inCollision2.inCollision = true;
                    }

                    if (state.state == State.CircleState.Explosion){
                        state.state = State.CircleState.Dynamic;
                    }
    
                    if (state2.state == State.CircleState.Explosion) {
                        state2.state = State.CircleState.Dynamic;
                    }

                    if (state.state != State.CircleState.Static && state2.state != State.CircleState.Static) {
                        HandleDynamicCollision(entityID, world.entities[i], size, size2, state, state2, sizeProtection);
                    }
                }
            }
        }
    }

    private static void HandleDynamicCollision(uint entity1, uint entity2, Size size1, Size size2, State state1, State state2, float protectionSize)
    {
        var world = World.Instance;

        // Increment the collision counter for dynamic circles of the same size
        if (size1.size == size2.size && size1.size == protectionSize)
        {
            if (state1.state == State.CircleState.Dynamic) 
            {
                world.collisionCountTab[entity1].collisionCount++;
            } if (state2.state == State.CircleState.Dynamic)
            {
                world.collisionCountTab[entity2].collisionCount++;
            }
            //debug collision count
            Debug.Log("Collision count : " + world.collisionCountTab[entity1].collisionCount);
            Debug.Log("Collision count : " + world.collisionCountTab[entity2].collisionCount);
        }

        //  Change the size for the bigger circle in collision with a smaller circle
        if (state1.state != State.CircleState.Protected && state2.state != State.CircleState.Protected)
        {
            if (size1.size > size2.size)
            {
                size1.size++;
                size2.size--;
            }
            else if (size1.size < size2.size)
            {
                size1.size--;
                size2.size++;
            }
        }
        else if (state1.state == State.CircleState.Protected || state2.state == State.CircleState.Protected)
        {
            HandleProtectedCollision(entity1, entity2, size1, size2, state1, state2);
        }
    }

    private static void HandleProtectedCollision(uint entity1, uint entity2, Size size1, Size size2, State state1, State state2)
    {
        // Decrease the size for the bigger circle in collision with a protected circle
        if (state1.state == State.CircleState.Protected && size1.size < size2.size)
        {
            size2.size--;
        }
        else if (state2.state == State.CircleState.Protected && size2.size < size1.size)
        {
            size1.size--;
        }
    }
    public string Name { get; } = "Collision";
}
