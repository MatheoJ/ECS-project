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

                    if (taille2.taille > taille.taille)
                    {
                        taille2.taille++;
                        taille.taille--;
                    }
                    else if (taille2.taille < taille.taille)
                    {
                        taille2.taille--;
                        taille.taille++;
                    }
                } 
            }
        }
    }
    public string Name { get; } = "Draw";
}
