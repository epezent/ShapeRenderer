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
    SerializedProperty fillTexture;
    SerializedProperty fillTextrueTiling;
    SerializedProperty fillTextureOffset;
    SerializedProperty customFillMaterial;

    SerializedProperty stroke;
    SerializedProperty strokeType;
    SerializedProperty strokeSolid;
    SerializedProperty strokeColor;
    SerializedProperty strokeWidth;
    SerializedProperty strokeTexture;
    SerializedProperty customStrokeMaterial;

    SerializedProperty shapeAnchors;
    SerializedProperty shapeRadiil;
    SerializedProperty radiiSmoothness;

    SerializedProperty sortingLayer;
    SerializedProperty sortingOrder;

    SerializedProperty colliderMode;
    SerializedProperty setColliderTo;
    SerializedProperty showComponents;

    private void OnEnable()
    {
        fill = serializedObject.FindProperty("fill");
        fillType = serializedObject.FindProperty("fillType");
        fillColor1 = serializedObject.FindProperty("fillColor1");
        fillColor2 = serializedObject.FindProperty("fillColor2");
        angle = serializedObject.FindProperty("angle");
        slider1 = serializedObject.FindProperty("slider1");
        slider2 = serializedObject.FindProperty("slider2");
        fillTexture = serializedObject.FindProperty("fillTexture");
        fillTextrueTiling = serializedObject.FindProperty("fillTextrueTiling");
        fillTextureOffset = serializedObject.FindProperty("fillTextureOffset");

        customFillMaterial = serializedObject.FindProperty("customFillMaterial");
        stroke = serializedObject.FindProperty("stroke");
        strokeType = serializedObject.FindProperty("strokeType");
        strokeSolid = serializedObject.FindProperty("strokeSolid");
        strokeColor = serializedObject.FindProperty("strokeColor");
        strokeWidth = serializedObject.FindProperty("strokeWidth");
        strokeTexture = serializedObject.FindProperty("strokeTexture");
        customStrokeMaterial = serializedObject.FindProperty("customStrokeMaterial");


    shapeAnchors = serializedObject.FindProperty("shapeAnchors");
        shapeRadiil = serializedObject.FindProperty("shapeRadii");
        radiiSmoothness = serializedObject.FindProperty("radiiSmoothness");
        sortingLayer = serializedObject.FindProperty("sortingLayer");
        sortingOrder = serializedObject.FindProperty("sortingOrder");
        colliderMode = serializedObject.FindProperty("colliderMode");
        setColliderTo = serializedObject.FindProperty("setColliderTo");
        showComponents = serializedObject.FindProperty("showComponents");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Fill", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(fill, new GUIContent("Show"));
        if (fill.boolValue == true)
        {
            EditorGUILayout.PropertyField(fillType);
            if ((ShapeRenderer.FillType)fillType.enumValueIndex != ShapeRenderer.FillType.Custom)
            {
                if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.Solid)
                {
                    EditorGUILayout.PropertyField(fillColor1, new GUIContent("Color"));
                }
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
                EditorGUILayout.PropertyField(fillTexture, new GUIContent("Texture"));
                if ((Texture)fillTexture.objectReferenceValue != null)
                {
                    EditorGUILayout.PropertyField(fillTextrueTiling, new GUIContent("Tiling"));
                    EditorGUILayout.PropertyField(fillTextureOffset, new GUIContent("Offset"));
                }


            }
            else
            {
                EditorGUILayout.PropertyField(customFillMaterial);
            }
        }

        EditorGUILayout.LabelField("Stroke", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(stroke, new GUIContent("Show"));
        if (stroke.boolValue == true)
        {
            EditorGUILayout.PropertyField(strokeType);
            if ((ShapeRenderer.StrokeType)strokeType.enumValueIndex != ShapeRenderer.StrokeType.Custom)
            {
                if ((ShapeRenderer.StrokeType)strokeType.enumValueIndex == ShapeRenderer.StrokeType.Solid)
                    EditorGUILayout.PropertyField(strokeSolid);
                else if ((ShapeRenderer.StrokeType)strokeType.enumValueIndex == ShapeRenderer.StrokeType.MultiGradient)
                    EditorGUILayout.PropertyField(strokeColor);
                EditorGUILayout.PropertyField(strokeTexture);
            }
            else
            {
                EditorGUILayout.PropertyField(customStrokeMaterial);
            }
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

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }          

}
