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
        Debug.Log("world.saveGameStates size 1: " + world.saveGameStates.Count);

        foreach (var saveTimeCode in world.saveTimeCodes)
        {
            Debug.Log("Time code : " + saveTimeCode.time);
            if (currentTime - saveTimeCode.time > 3.0)
            {
                Debug.Log("Time code saved : " + saveTimeCode.time);
                GameStateSaveToRemove++;
            }
            else
            {
                break;
            }
        }

        Debug.Log("world.saveGameStates size 2: " + world.saveGameStates.Count);


        for (int i = 0; i < GameStateSaveToRemove; i++)
        {
            world.saveGameStates.RemoveAt(0);
            world.saveTimeCodes.RemoveAt(0);
            //log world.saveGameStates size            
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

                //log world.saveGameStates size
                Debug.Log("world.saveGameStates size: " + world.saveGameStates.Count);


                var lastGameState = world.saveGameStates[0];
                world.saveGameStates.Clear();
                world.saveTimeCodes.Clear();

                world.entities = lastGameState.entities;
                world.positionTab = lastGameState.positionTab;
                world.speedTab = lastGameState.speedTab;
                world.tailleTab = lastGameState.tailleTab;
                world.stateTab = lastGameState.stateTab;
                world.collisionCountTab = lastGameState.collisionCountTab;
                world.protectionTimeTab = lastGameState.protectionTimeTab;
                world.cooldownTimeTab = lastGameState.cooldownTimeTab;  
                
                foreach (var entityID in world.entities)
                {
                    ECSController.Instance.CreateShape(entityID, (int)world.tailleTab[entityID].taille);
                }
            }
        } else
        {
           // Add the current game state to the list of game states saved
            world.saveGameStates.Add(new GameStateSave(world.tailleTab, world.positionTab, world.speedTab, world.stateTab,
                                                        world.collisionCountTab, world.protectionTimeTab, world.cooldownTimeTab,
                                                        world.entities));
            world.saveTimeCodes.Add(new SaveTimeCode());

        }
    }
    public string Name { get; } = "RollBack";
}
