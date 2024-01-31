using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : ISystem
{
    private float previousTime = 0;
    public void UpdateSystem()
    {

        if (previousTime == 0)
        {
            previousTime = Time.time;
            return;
        }
        var currentTime = Time.time;
        var deltaTime = currentTime - previousTime;
        previousTime = currentTime;

        var world = World.Instance;
        foreach (var entityID in world.entities)
        {
            var position = world.positionTab[entityID];
            var speed = world.speedTab[entityID];
            position.position += speed.speed * (float)deltaTime;
            ECSController.Instance.UpdateShapePosition(entityID, position.position);
            //Debug.Log("Position " + position.position);
        }
    }
    public string Name { get; } = "Mouvement";
}

