using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    public Taille[] tailleTab;
    public Position[] positionTab;
    public List<uint> entities = new List<uint>();
    public uint nbEntities = 0;

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

    private World() {
        tailleTab = new Taille[1000];
        positionTab = new Position[1000];
    }
}
