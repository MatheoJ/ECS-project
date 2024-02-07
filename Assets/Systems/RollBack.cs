using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBack : ISystem
{
    public void UpdateSystem()
    {
        double currentTime = Time.time;
        var world = World.Instance;

        int GameStateSaveToRemove = 0;

        foreach (var saveTimeCode in world.saveTimeCodes)
        {
            if (currentTime - saveTimeCode.time > 3.0)
            {
                GameStateSaveToRemove++;
            }
            else
            {
                break;
            }
        }



        for (int i = 0; i < GameStateSaveToRemove; i++)
        {
            world.saveGameStates.RemoveAt(0);
            world.saveTimeCodes.RemoveAt(0);         
        }

        //if space is pressed, rollback the game state to the previous one
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentTime - world.timeSinceLastRollBack > 3.0)
            {

                foreach (var entityID in world.entities)
                {
                    ECSController.Instance.DestroyShape(entityID);
                }

                world.timeSinceLastRollBack = Time.time;

                var lastGameState = world.saveGameStates[0];
                world.saveGameStates.Clear();
                world.saveTimeCodes.Clear();

                world.entities = lastGameState.entities;
                world.positionTab = lastGameState.positionTab;
                world.speedTab = lastGameState.speedTab;
                world.sizeTab = lastGameState.sizeTab;
                world.stateTab = lastGameState.stateTab;
                world.collisionCountTab = lastGameState.collisionCountTab;
                world.protectionTimeTab = lastGameState.protectionTimeTab;
                world.cooldownTimeTab = lastGameState.cooldownTimeTab; 
                world.inCollisionTab = lastGameState.inCollisionTab;
                
                foreach (var entityID in world.entities)
                {
                    ECSController.Instance.CreateShape(entityID, (int)world.sizeTab[entityID].size);
                }
            }
        } else
        {
           // Add the current game state to the list of game states saved
            world.saveGameStates.Add(new GameStateSave(world.sizeTab, world.positionTab, world.speedTab, world.stateTab,
                                                        world.collisionCountTab, world.protectionTimeTab, world.cooldownTimeTab,
                                                        world.entities, world.inCollisionTab));
            world.saveTimeCodes.Add(new SaveTimeCode());

        }
    }
    public string Name { get; } = "RollBack";
}
