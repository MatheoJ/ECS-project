// Source: http://wiki.unity3d.com/index.php/SerializableDictionary

using System;
using UnityEditor;

/*
   DO NOT CHANGE, THIS WILL BE REPLACED AT CORRECTION
   NE PAS CHANGER, CE FICHIER VA ÊTRE REMPLACÉ A LA CORRECTION
*/

public static class SerializedPropertyExtension {
    private const double Tolerance = 1e-10;

    public static int GetObjectCode(this SerializedProperty p) { // Unique code per serialized object and property path
        return p.propertyPath.GetHashCode() ^ p.serializedObject.GetHashCode();
    }

    public static bool EqualBasics(SerializedProperty left, SerializedProperty right) {
        if (left.propertyType != right.propertyType)
            return false;
        return left.propertyType switch
        {
            SerializedPropertyType.Integer when left.type != right.type => false,
            SerializedPropertyType.Integer when left.type == "int" => left.intValue == right.intValue,
            SerializedPropertyType.Integer => left.longValue == right.longValue,
            SerializedPropertyType.String => left.stringValue == right.stringValue,
            SerializedPropertyType.ObjectReference => left.objectReferenceValue == right.objectReferenceValue,
            SerializedPropertyType.Enum => left.enumValueIndex == right.enumValueIndex,
            SerializedPropertyType.Boolean => left.boolValue == right.boolValue,
            SerializedPropertyType.Float when left.type != right.type => false,
            SerializedPropertyType.Float when left.type == "float" => Math.Abs(left.floatValue - right.floatValue) <
                                                                      Tolerance,
            SerializedPropertyType.Float => Math.Abs(left.doubleValue - right.doubleValue) < Tolerance,
            SerializedPropertyType.Color => left.colorValue == right.colorValue,
            SerializedPropertyType.LayerMask => left.intValue == right.intValue,
            SerializedPropertyType.Vector2 => left.vector2Value == right.vector2Value,
            SerializedPropertyType.Vector3 => left.vector3Value == right.vector3Value,
            SerializedPropertyType.Vector4 => left.vector4Value == right.vector4Value,
            SerializedPropertyType.Rect => left.rectValue == right.rectValue,
            SerializedPropertyType.ArraySize => left.arraySize == right.arraySize,
            SerializedPropertyType.Character => left.intValue == right.intValue,
            SerializedPropertyType.AnimationCurve => false,
            SerializedPropertyType.Bounds => left.boundsValue == right.boundsValue,
            SerializedPropertyType.Gradient => false,
            SerializedPropertyType.Quaternion => left.quaternionValue == right.quaternionValue,
            _ => false
        };
    }

    public static void CopyBasics(SerializedProperty source, SerializedProperty target)
    {
        if (source.propertyType != target.propertyType)
            return;
        switch (source.propertyType)
        {
            case SerializedPropertyType.Integer when source.type != target.type:
                return;
            case SerializedPropertyType.Integer when source.type == "int":
                target.intValue = source.intValue;
                break;
            case SerializedPropertyType.Integer:
                target.longValue = source.longValue;
                break;
            case SerializedPropertyType.String:
                target.stringValue = source.stringValue;
                break;
            case SerializedPropertyType.ObjectReference:
                target.objectReferenceValue = source.objectReferenceValue;
                break;
            case SerializedPropertyType.Enum:
                target.enumValueIndex = source.enumValueIndex;
                break;
            case SerializedPropertyType.Boolean:
                target.boolValue = source.boolValue;
                break;
            case SerializedPropertyType.Float when source.type != target.type:
                return;
            case SerializedPropertyType.Float when source.type == "float":
                target.floatValue = source.floatValue;
                break;
            case SerializedPropertyType.Float:
                target.doubleValue = source.doubleValue;
                break;
            case SerializedPropertyType.Color:
                target.colorValue = source.colorValue;
                break;
            case SerializedPropertyType.LayerMask:
                target.intValue = source.intValue;
                break;
            case SerializedPropertyType.Vector2:
                target.vector2Value = source.vector2Value;
                break;
            case SerializedPropertyType.Vector3:
                target.vector3Value = source.vector3Value;
                break;
            case SerializedPropertyType.Vector4:
                target.vector4Value = source.vector4Value;
                break;
            case SerializedPropertyType.Rect:
                target.rectValue = source.rectValue;
                break;
            case SerializedPropertyType.ArraySize:
                target.arraySize = source.arraySize;
                break;
            case SerializedPropertyType.Character:
                target.intValue = source.intValue;
                break;
            case SerializedPropertyType.AnimationCurve:
                target.animationCurveValue = source.animationCurveValue;
                break;
            case SerializedPropertyType.Bounds:
                target.boundsValue = source.boundsValue;
                break;
            case SerializedPropertyType.Gradient:
                // TODO?
                break;
            case SerializedPropertyType.Quaternion:
                target.quaternionValue = source.quaternionValue;
                break;
            default:
            {
                if (!source.hasChildren || !target.hasChildren) return;
            
                var sourceIterator = source.Copy();
                var targetIterator = target.Copy();
                while (true) {
                    if (sourceIterator.propertyType == SerializedPropertyType.Generic) {
                        if (!sourceIterator.Next(true) || !targetIterator.Next(true))
                            break;
                    } else if (!sourceIterator.Next(false) || !targetIterator.Next(false)) {
                        break;
                    }
                    CopyBasics(sourceIterator, targetIterator);
                }
                break;
            }
        }
    }
}
