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
        var world = World.Instance;
        world.IncreaseFrameCountForLeftPartOfScreen();

        if (hasBeenInitialized)
            return;
        hasBeenInitialized = true;
        
        foreach (ShapeConfig shapeConf in ECSController.Instance.Config.circleInstancesToSpawn)
        {
            world.entities.Add(world.nbEntities);
            world.positionTab[world.nbEntities] = new Position ( shapeConf.initialPosition );
            world.sizeTab[world.nbEntities] = new Size ( shapeConf.initialSize );
            world.speedTab[world.nbEntities] = new Speed ( shapeConf.initialVelocity);
            if (shapeConf.initialVelocity.magnitude > 0) {
                world.stateTab[world.nbEntities] = new State(State.CircleState.Dynamic);
            } else
            {
                world.stateTab[world.nbEntities] = new State(State.CircleState.Static);
            }            
            world.collisionCountTab[world.nbEntities] = new CollisionCount ();
            world.protectionTimeTab[world.nbEntities] = new ProtectionTime ();
            world.cooldownTimeTab[world.nbEntities] = new CooldownTime ();
            world.inCollisionTab[world.nbEntities] = new InCollision (false);

            Color color = Color.white;

            ECSController.Instance.CreateShape(world.nbEntities, shapeConf.initialSize);

            //Log the entity creation
            Debug.Log("Entity " + world.nbEntities + " "+ world.sizeTab[world.nbEntities].size);
            world.nbEntities++;
        }


    }
    public string Name { get; } = "Initialize";

}
