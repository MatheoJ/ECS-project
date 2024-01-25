// Source: http://wiki.unity3d.com/index.php/SerializableDictionary

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/

using System;
using System.Collections.Generic;
 
using UnityEngine;
 
[Serializable]
public abstract class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver {
    [SerializeField]
    private TKey[] keys;
    [SerializeField]
    private TValue[] values;
 
    public Dictionary<TKey, TValue> dictionary;
 
    public static T New<T>() where T : SerializableDictionary<TKey, TValue>, new() {
        var result = new T {dictionary = new Dictionary<TKey, TValue>()};
        return result;
    }
 
    public void OnAfterDeserialize() {
        var c = keys.Length;
        dictionary = new Dictionary<TKey, TValue>(c);
        for (var i = 0; i < c; i++) {
            dictionary[keys[i]] = values[i];
        }
        keys = null;
        values = null;
    }
 
    public void OnBeforeSerialize() {
        var c = dictionary.Count;
        keys = new TKey[c];
        values = new TValue[c];
        var i = 0;
        using var e = dictionary.GetEnumerator();
        while (e.MoveNext()) {
            var (key, value) = e.Current;
            keys[i] = key;
            values[i] = value;
            i++;
        }
    }
}
