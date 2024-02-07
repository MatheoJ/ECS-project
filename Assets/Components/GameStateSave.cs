using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSave : IComponent
{
    public Taille[] tailleTab;
    public Position[] positionTab;
    public Speed[] speedTab;
    public State[] stateTab;
    public CollisionCount[] collisionCountTab;
    public ProtectionTime[] protectionTimeTab;
    public CooldownTime[] cooldownTimeTab;
    public InCollision[] inCollisionTab;
    public List<uint> entities = new List<uint>();

    // Constructor that performs deep copying
    public GameStateSave(Taille[] tailleTab, Position[] positionTab, Speed[] speedTab, State[] stateTab, 
                        CollisionCount[] collisionCountTab, ProtectionTime[] protectionTimeTab, 
                        CooldownTime[] cooldownTimeTab, List<uint> entities, InCollision[] inCollisionTab)
    {
        this.tailleTab = DeepCopyArray(tailleTab);
        this.positionTab = DeepCopyArray(positionTab);
        this.speedTab = DeepCopyArray(speedTab);
        this.stateTab = DeepCopyArray(stateTab);
        this.collisionCountTab = DeepCopyArray(collisionCountTab);
        this.protectionTimeTab = DeepCopyArray(protectionTimeTab);
        this.cooldownTimeTab = DeepCopyArray(cooldownTimeTab);
        this.inCollisionTab = DeepCopyArray(inCollisionTab);
        this.entities = new List<uint>(entities);
    }

    // Generic method to deep copy arrays of custom types
    private T[] DeepCopyArray<T>(T[] original)
    {
        T[] copy = new T[original.Length];
        Type componentType = typeof(T);

        // Use reflection to get the copy constructor of the component type.
        var copyConstructor = componentType.GetConstructor(new Type[] { componentType });
        if (copyConstructor == null)
        {
            throw new InvalidOperationException($"Type {componentType.FullName} does not have a copy constructor.");
        }

        for (int i = 0; i < original.Length; i++)
        {
            if (original[i] == null)
            {
                copy[i] = default(T);
            }
            else
            {
                // Invoke the copy constructor to create a deep copy of each element.
                copy[i] = (T)copyConstructor.Invoke(new object[] { original[i] });
            }
        }
        return copy;
    }

}

