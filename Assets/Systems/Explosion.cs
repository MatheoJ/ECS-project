using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : ISystem
{
    public void UpdateSystem()
    {
        int tailleMax = ECSController.Instance.Config.explosionSize;
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
            var taille = world.tailleTab[entityID];
            var speed = world.speedTab[entityID];

            
            bool isMouseOver = mouseDown && IsMouseOverEntity(mouseWorldPosition, position, taille);

            if (taille.taille <= 0f || (isMouseOver && taille.taille <= 4 ))
            {
                entitiesToRemove.Add(entityID);
            }
            else if (taille.taille >= tailleMax || isMouseOver)
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
                    entitiesToAdd.Add(world.nbEntities);

                    world.positionTab[world.nbEntities] = new Position(position.position + diagonals[i] * thirdOfTaille);
                    world.tailleTab[world.nbEntities] = new Taille(newTaille);
                    world.speedTab[world.nbEntities] = new Speed(diagonals[i] * speedValue);
                    world.stateTab[world.nbEntities] = new State(State.CircleState.Explosion);
                    world.collisionCountTab[world.nbEntities] = new CollisionCount();
                    world.protectionTimeTab[world.nbEntities] = new ProtectionTime();
                    world.cooldownTimeTab[world.nbEntities] = new CooldownTime();
                    world.inCollisionTab[world.nbEntities] = new InCollision(false);

                    ECSController.Instance.CreateShape(world.nbEntities, newTaille);

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

    private bool IsMouseOverEntity(Vector3 mousePosition, Position entityPosition, Taille entityTaille)
    {
        return (mousePosition - new Vector3(entityPosition.position.x, entityPosition.position.y, 0)).magnitude < entityTaille.taille;
    }


}

