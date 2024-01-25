using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Config;
using static ECSController;

public class Initialize : ISystem
{
    static bool hasBeenInitialized = false;
    public void UpdateSystem()
    {

        if (hasBeenInitialized)
            return;
        hasBeenInitialized = true;

        var world = World.Instance;
        foreach (ShapeConfig shapeConf in ECSController.Instance.Config.circleInstancesToSpawn)
       {
            world.entities.Add(world.nbEntities);
            world.positionTab[world.nbEntities] = new Position ( shapeConf.initialPosition );
            world.tailleTab[world.nbEntities] = new Taille ( shapeConf.initialSize );
            world.speedTab[world.nbEntities] = new Speed ( shapeConf.initialVelocity);

            Color color = Color.white;

            ECSController.Instance.CreateShape(world.nbEntities, shapeConf.initialSize);


            world.nbEntities++;

            //Log the entity creation
            Debug.Log("Entity " + world.nbEntities );
        }


    }
    public string Name { get; } = "Initialize";

}
