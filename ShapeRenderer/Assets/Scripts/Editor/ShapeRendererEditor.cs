using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[CustomEditor(typeof(ShapeRenderer)), CanEditMultipleObjects]
public class ShapeRendererEditor : Editor {

    SerializedProperty fill;
    SerializedProperty fillType;
    SerializedProperty fillColor1;
    SerializedProperty fillColor2;
    SerializedProperty angle;
    SerializedProperty slider1;
    SerializedProperty slider2;

    SerializedProperty stroke;
    SerializedProperty strokeColor;
    SerializedProperty strokeWidth;

    SerializedProperty shapeAnchors;
    SerializedProperty shapeRadiil;
    SerializedProperty radiiSmoothness;

    SerializedProperty sortingLayer;
    SerializedProperty sortingOrder;

    SerializedProperty colliderMode;
    SerializedProperty setColliderTo;
    SerializedProperty showComponents;
    SerializedProperty useDLL;

    private void OnEnable()
    {
        fill = serializedObject.FindProperty("fill");
        fillType = serializedObject.FindProperty("fillType");
        fillColor1 = serializedObject.FindProperty("fillColor1");
        fillColor2 = serializedObject.FindProperty("fillColor2");
        angle = serializedObject.FindProperty("angle");
        slider1 = serializedObject.FindProperty("slider1");
        slider2 = serializedObject.FindProperty("slider2");
        stroke = serializedObject.FindProperty("stroke");
        strokeColor = serializedObject.FindProperty("strokeColor");
        strokeWidth = serializedObject.FindProperty("strokeWidth");
        shapeAnchors = serializedObject.FindProperty("shapeAnchors");
        shapeRadiil = serializedObject.FindProperty("shapeRadii");
        radiiSmoothness = serializedObject.FindProperty("radiiSmoothness");
        sortingLayer = serializedObject.FindProperty("sortingLayer");
        sortingOrder = serializedObject.FindProperty("sortingOrder");
        colliderMode = serializedObject.FindProperty("colliderMode");
        setColliderTo = serializedObject.FindProperty("setColliderTo");
        showComponents = serializedObject.FindProperty("showComponents");
        useDLL = serializedObject.FindProperty("useDLL");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Shape Appearance", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(fill);
        if (fill.boolValue == true)
        {
            EditorGUILayout.PropertyField(fillType);
            if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.Solid)
                EditorGUILayout.PropertyField(fillColor1, new GUIContent("Color"));
            else if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.LinearGradient)
            {
                EditorGUILayout.PropertyField(fillColor1, new GUIContent("Color 1"));
                EditorGUILayout.PropertyField(fillColor2, new GUIContent("Color 2"));
                EditorGUILayout.PropertyField(angle);
            }
            else if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.RadialGradient)
            {
                EditorGUILayout.PropertyField(fillColor1, new GUIContent("Color 1"));
                EditorGUILayout.PropertyField(fillColor2, new GUIContent("Color 2"));
                EditorGUILayout.PropertyField(slider1, new GUIContent("X"));
                EditorGUILayout.PropertyField(slider2, new GUIContent("Y"));
            }
        }

        EditorGUILayout.PropertyField(stroke);
        if (stroke.boolValue == true)
        {
            EditorGUILayout.PropertyField(strokeColor);
            EditorGUILayout.PropertyField(strokeWidth);
        }

        EditorGUILayout.LabelField("Shape Geometry", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(shapeAnchors, true);
        EditorGUILayout.PropertyField(shapeRadiil, true);
        EditorGUILayout.PropertyField(radiiSmoothness, true);

        EditorGUILayout.LabelField("Sorting Layer", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sortingLayer);
        EditorGUILayout.PropertyField(sortingOrder);

        EditorGUILayout.LabelField("Other Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(colliderMode);
        if ((ShapeRenderer.ColliderMode)colliderMode.enumValueIndex == ShapeRenderer.ColliderMode.ToCollider)
            EditorGUILayout.PropertyField(setColliderTo);
        EditorGUILayout.PropertyField(showComponents);
        EditorGUILayout.PropertyField(useDLL);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }

}
