using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    public Size[] sizeTab;
    public Position[] positionTab;
    public Speed[] speedTab;
    public State[] stateTab;
    public CollisionCount[] collisionCountTab;
    public ProtectionTime[] protectionTimeTab;
    public CooldownTime[] cooldownTimeTab;
    public InCollision[] inCollisionTab;
    public List<uint> entities = new List<uint>();

    public List<GameStateSave> saveGameStates = new List<GameStateSave>();
    public List<SaveTimeCode> saveTimeCodes = new List<SaveTimeCode>();
    public double timeSinceLastRollBack = 0;

    public uint nbEntities = 0;
    public uint frameCountForLeftPartofScreen = 0;

    private static World _instance;

    public static World Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = new World();
            return _instance;
        }
    }

    public void IncreaseFrameCountForLeftPartOfScreen()
    {
        frameCountForLeftPartofScreen++;
        frameCountForLeftPartofScreen = frameCountForLeftPartofScreen % 4;
    }

    public bool ShouldEntityBeUpdated(uint entityID)
    {
        if (frameCountForLeftPartofScreen==0)
        {
            return true;
        }
        else
        {
            return IsEntityOnTheLeftOnTheScreen(entityID);
        }
    }

    private bool IsEntityOnTheLeftOnTheScreen(uint entityID)
    {
        float xPosition = positionTab[entityID].position.x;
                
        return xPosition < 0;
    }

    private World() {
        sizeTab = new Size[1000];
        positionTab = new Position[1000];
        speedTab = new Speed[1000];
        stateTab = new State[1000];
        collisionCountTab = new CollisionCount[1000];
        protectionTimeTab = new ProtectionTime[1000];
        cooldownTimeTab = new CooldownTime[1000];
        inCollisionTab = new InCollision[1000];

    }
}
