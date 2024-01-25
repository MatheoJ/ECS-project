// Source: http://wiki.unity3d.com/index.php/SerializableDictionary

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/

public abstract class SerializableKeyValueTemplate<TKey, TValue> : ScriptableObject {
    public TKey key;
    public TValue value;
}
 
public abstract class SerializableDictionaryDrawer<TKey, TValue> : PropertyDrawer {
 
    protected abstract SerializableKeyValueTemplate<TKey, TValue> GetTemplate();
    protected T GetGenericTemplate<T>() where T: SerializableKeyValueTemplate<TKey, TValue> {
        return ScriptableObject.CreateInstance<T>();
    }
 
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
 
        EditorGUI.BeginProperty(position, label, property);
 
        var firstLine = position;
        firstLine.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(firstLine, property);
 
        if (property.isExpanded) {
            var secondLine = firstLine;
            secondLine.y += EditorGUIUtility.singleLineHeight;
 
            EditorGUIUtility.labelWidth = 50f;
 
            secondLine.x += 15f; // indentation
            secondLine.width -= 15f;
 
            var secondLineKey = secondLine;
 
            const float buttonWidth = 60f;
            secondLineKey.width -= buttonWidth; // assign button
            secondLineKey.width /= 2f;
 
            var secondLineValue = secondLineKey;
            secondLineValue.x += secondLineValue.width;
            if (GetTemplateValueProp(property).hasVisibleChildren) { // if the value has children, indent to make room for fold arrow
                secondLineValue.x += 15;
                secondLineValue.width -= 15;
            }
 
            var secondLineButton = secondLineValue;
            secondLineButton.x += secondLineValue.width;
            secondLineButton.width = buttonWidth;
 
            var kHeight = EditorGUI.GetPropertyHeight(GetTemplateKeyProp(property));
            var vHeight = EditorGUI.GetPropertyHeight(GetTemplateValueProp(property));
            var extraHeight = Mathf.Max(kHeight, vHeight);
            
            const float spaceWidth = 30;
            secondLineValue.x += spaceWidth;
            
            secondLineKey.height = kHeight;
            secondLineValue.height = vHeight;
 
            EditorGUI.PropertyField(secondLineKey, GetTemplateKeyProp(property), true);
            EditorGUI.PropertyField(secondLineValue, GetTemplateValueProp(property), true);
 
            var keysProp = GetKeysProp(property);
            var valuesProp = GetValuesProp(property);
 
            var numLines = keysProp.arraySize;
 
            if (GUI.Button(secondLineButton, "Assign")) {
                var assignment = false;
                for (var i = 0; i < numLines; i++)
                {
                    // Try to replace existing value
                    if (!SerializedPropertyExtension.EqualBasics(GetIndexedItemProp(keysProp, i),
                        GetTemplateKeyProp(property))) continue;
                    
                    SerializedPropertyExtension.CopyBasics(GetTemplateValueProp(property), GetIndexedItemProp(valuesProp, i));
                    assignment = true;
                    break;
                }
                if (!assignment) { // Create a new value
                    keysProp.arraySize += 1;
                    valuesProp.arraySize += 1;
                    SerializedPropertyExtension.CopyBasics(GetTemplateKeyProp(property), GetIndexedItemProp(keysProp, numLines));
                    SerializedPropertyExtension.CopyBasics(GetTemplateValueProp(property), GetIndexedItemProp(valuesProp, numLines));
                }
            }
 
            for (var i = 0; i < numLines; i++) {
                secondLineKey.y += extraHeight;
                secondLineValue.y += extraHeight;
                secondLineButton.y += extraHeight;
 
                kHeight = EditorGUI.GetPropertyHeight(GetIndexedItemProp(keysProp, i));
                vHeight = EditorGUI.GetPropertyHeight(GetIndexedItemProp(valuesProp, i));
                extraHeight = Mathf.Max(kHeight, vHeight);
                
                secondLineKey.height = kHeight;
                secondLineValue.height = vHeight;
 
                EditorGUI.PropertyField(secondLineKey, GetIndexedItemProp(keysProp, i), true);
                EditorGUI.PropertyField(secondLineValue, GetIndexedItemProp(valuesProp, i), true);

                if (!GUI.Button(secondLineButton, "Remove")) continue;
                
                keysProp.DeleteArrayElementAtIndex(i);
                valuesProp.DeleteArrayElementAtIndex(i);
            }
        }
        EditorGUI.EndProperty ();
    }
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;
 
        var total = EditorGUIUtility.singleLineHeight;
 
        var kHeight = EditorGUI.GetPropertyHeight(GetTemplateKeyProp(property));
        var vHeight = EditorGUI.GetPropertyHeight(GetTemplateValueProp(property));
        total += Mathf.Max(kHeight, vHeight);
 
        var keysProp = GetKeysProp(property);
        var valuesProp = GetValuesProp(property);
        var numLines = keysProp.arraySize;
        for (var i = 0; i < numLines; i++) {
            kHeight = EditorGUI.GetPropertyHeight(GetIndexedItemProp(keysProp, i));
            vHeight = EditorGUI.GetPropertyHeight(GetIndexedItemProp(valuesProp, i));
            total += Mathf.Max(kHeight, vHeight);
        }
        return total;
    }
 
    private SerializedProperty GetTemplateKeyProp(SerializedProperty mainProp) {
        return GetTemplateProp(_templateKeyProp, mainProp);
    }
    private SerializedProperty GetTemplateValueProp(SerializedProperty mainProp) {
        return GetTemplateProp(_templateValueProp, mainProp);
    }
 
    private SerializedProperty GetTemplateProp(Dictionary<int, SerializedProperty> source, SerializedProperty mainProp) {
        if (source.TryGetValue(mainProp.GetObjectCode(), out var p)) return p;
        
        var templateObject = GetTemplate();
        var templateSerializedObject = new SerializedObject(templateObject);
        var kProp = templateSerializedObject.FindProperty("key");
        var vProp = templateSerializedObject.FindProperty("value");
        
        _templateKeyProp[mainProp.GetObjectCode()] = kProp;
        _templateValueProp[mainProp.GetObjectCode()] = vProp;
        p = source == _templateKeyProp ? kProp : vProp;
        return p;
    }
    private readonly Dictionary<int, SerializedProperty> _templateKeyProp = new();
    private readonly Dictionary<int, SerializedProperty> _templateValueProp = new();
 
    private SerializedProperty GetKeysProp(SerializedProperty mainProp) {
        return GetCachedProp(mainProp, "keys", _keysProps); }
    private SerializedProperty GetValuesProp(SerializedProperty mainProp) {
        return GetCachedProp(mainProp, "values", _valuesProps); }
 
    private static SerializedProperty GetCachedProp(SerializedProperty mainProp, string relativePropertyName, Dictionary<int, SerializedProperty> source) {
        var objectCode = mainProp.GetObjectCode();
        if (!source.TryGetValue(objectCode, out var p))
            source[objectCode] = p = mainProp.FindPropertyRelative(relativePropertyName);
        return p;
    }
 
    private readonly Dictionary<int, SerializedProperty> _keysProps = new();
    private readonly Dictionary<int, SerializedProperty> _valuesProps = new();
 
    private readonly Dictionary<int, Dictionary<int, SerializedProperty>> _indexedPropertyDicts = new();
 
    private SerializedProperty GetIndexedItemProp(SerializedProperty arrayProp, int index) {
        if (!_indexedPropertyDicts.TryGetValue(arrayProp.GetObjectCode(), out var d))
            _indexedPropertyDicts[arrayProp.GetObjectCode()] = d = new Dictionary<int, SerializedProperty>();
        if (!d.TryGetValue(index, out var result))
            d[index] = result = arrayProp.FindPropertyRelative($"Array.data[{index}]");
        return result;
    }
 
}
