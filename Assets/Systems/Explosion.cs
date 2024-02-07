using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : ISystem
{
    public void UpdateSystem()
    {
        int sizeMax = ECSController.Instance.Config.explosionSize;
        bool mouseDown = Input.GetMouseButtonDown(0);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Assurez-vous que la position Z est à 0 pour une correspondance 2D

        List<uint> entitiesToRemove = new List<uint>();
        List<uint> entitiesToAdd = new List<uint>();

        var world = World.Instance;


        foreach (var entityID in world.entities)
        {

            if (!world.ShouldEntityBeUpdated(entityID))
            {
                continue;
            }

            var position = world.positionTab[entityID];
            var size = world.sizeTab[entityID];
            var speed = world.speedTab[entityID];

            
            bool isMouseOver = mouseDown && IsMouseOverEntity(mouseWorldPosition, position, size);

            if (size.size <= 0f || (isMouseOver && size.size <= 4 ))
            {
                entitiesToRemove.Add(entityID);
            }
            else if (size.size >= sizeMax || isMouseOver)
            {

                int newSize = ((int)(size.size / 4.0));
                if (newSize < 1)
                {
                    newSize = 1;
                }

                Vector2[] diagonals = new Vector2[4];
                diagonals[0] = new Vector2(-1, 1);
                diagonals[1] = new Vector2(1, 1);
                diagonals[2] = new Vector2(-1, -1);
                diagonals[3] = new Vector2(1, -1);

                float thirdOfSize = size.size / 3;
                float speedValue = speed.speed.magnitude;

                // Create 4 new entities
                for (int i = 0; i < 4; i++)
                {
                    entitiesToAdd.Add(world.nbEntities);

                    world.positionTab[world.nbEntities] = new Position(position.position + diagonals[i] * thirdOfSize);
                    world.sizeTab[world.nbEntities] = new Size(newSize);
                    world.speedTab[world.nbEntities] = new Speed(diagonals[i] * speedValue);
                    world.stateTab[world.nbEntities] = new State(State.CircleState.Explosion);
                    world.collisionCountTab[world.nbEntities] = new CollisionCount();
                    world.protectionTimeTab[world.nbEntities] = new ProtectionTime();
                    world.cooldownTimeTab[world.nbEntities] = new CooldownTime();
                    world.inCollisionTab[world.nbEntities] = new InCollision(false);

                    ECSController.Instance.CreateShape(world.nbEntities, newSize);

                    world.nbEntities++;
                }

                entitiesToRemove.Add(entityID);                
            }
        }

        foreach (var entityID in entitiesToRemove)
        {
            world.entities.Remove(entityID);
            ECSController.Instance.DestroyShape(entityID);
        }

        foreach (var entityID in entitiesToAdd)
        {
            world.entities.Add(entityID);
        }

    }
    public string Name { get; } = "Explosion";

    private bool IsMouseOverEntity(Vector3 mousePosition, Position entityPosition, Size entitysize)
    {
        return (mousePosition - new Vector3(entityPosition.position.x, entityPosition.position.y, 0)).magnitude < entitysize.size;
    }


}

