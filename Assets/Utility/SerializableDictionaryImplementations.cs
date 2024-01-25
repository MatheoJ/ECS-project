// Source: http://wiki.unity3d.com/index.php/SerializableDictionary

using System;
 
using UnityEngine;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/
 
// ---------------
//  String => Int
// ---------------
[Serializable]
public class StringIntDictionary : SerializableDictionary<string, int> {}
 
// ---------------
//  GameObject => Float
// ---------------
[Serializable]
public class GameObjectFloatDictionary : SerializableDictionary<GameObject, float> {}

[Serializable]

public class StringBoolDictionary : SerializableDictionary<string, bool> { }

