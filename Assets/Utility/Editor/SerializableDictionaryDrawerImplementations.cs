// Source: http://wiki.unity3d.com/index.php/SerializableDictionary

using UnityEditor;
using UnityEngine;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/

// ---------------
//  String => Int
// ---------------
[CustomPropertyDrawer(typeof(StringIntDictionary))]
public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int> {
    protected override SerializableKeyValueTemplate<string, int> GetTemplate() {
        return GetGenericTemplate<SerializableStringIntTemplate>();
    }
}
internal class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int> {}
 
// ---------------
//  GameObject => Float
// ---------------
[CustomPropertyDrawer(typeof(GameObjectFloatDictionary))]
public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float> {
    protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate() {
        return GetGenericTemplate<SerializableGameObjectFloatTemplate>();
    }
}
internal class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float> {}


[CustomPropertyDrawer(typeof(StringBoolDictionary))]
public class StringBoolDictionaryDrawer : SerializableDictionaryDrawer<string, bool>
{
    protected override SerializableKeyValueTemplate<string, bool> GetTemplate()
    {
        return GetGenericTemplate<SerializableStringBoolTemplate>();
    }
}
internal class SerializableStringBoolTemplate : SerializableKeyValueTemplate<string, bool> { }
