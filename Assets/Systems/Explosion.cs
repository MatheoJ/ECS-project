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


        foreach (var entityID in World.Instance.entities)
        {
            var position = World.Instance.positionTab[entityID];
            var taille = World.Instance.tailleTab[entityID];
            var speed = World.Instance.speedTab[entityID];

            
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
                    entitiesToAdd.Add(World.Instance.nbEntities);

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

                entitiesToRemove.Add(entityID);                
            }
        }

        foreach (var entityID in entitiesToRemove)
        {
            World.Instance.entities.Remove(entityID);
            ECSController.Instance.DestroyShape(entityID);
        }

        foreach (var entityID in entitiesToAdd)
        {
            World.Instance.entities.Add(entityID);
        }

    }
    public string Name { get; } = "Explosion";

    private bool IsMouseOverEntity(Vector3 mousePosition, Position entityPosition, Taille entityTaille)
    {
        // Logique pour déterminer si la souris est sur l'entité
        // Cela dépend de la forme de votre entité (cercle, carré, etc.)
        // Par exemple, pour un cercle :
        return (mousePosition - new Vector3(entityPosition.position.x, entityPosition.position.y, 0)).magnitude < entityTaille.taille;
    }


}

