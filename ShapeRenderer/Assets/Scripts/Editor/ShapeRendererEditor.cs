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
    SerializedProperty fillAngle;
    SerializedProperty fillOffset1;
    SerializedProperty fillOffset2;
    SerializedProperty fillTexture;
    SerializedProperty fillTextrueTiling;
    SerializedProperty fillTextureOffset;
    SerializedProperty customFillMaterial;

    SerializedProperty stroke;
    SerializedProperty strokeType;
    SerializedProperty strokeSolid;
    SerializedProperty strokeGradient;
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
        fill = serializedObject.FindProperty("fill_");
        fillType = serializedObject.FindProperty("fillType_");
        fillColor1 = serializedObject.FindProperty("fillColor1_");
        fillColor2 = serializedObject.FindProperty("fillColor2_");
        fillAngle = serializedObject.FindProperty("fillAngle_");
        fillOffset1 = serializedObject.FindProperty("fillOffset1_");
        fillOffset2 = serializedObject.FindProperty("fillOffset1_");
        fillTexture = serializedObject.FindProperty("fillTexture_");
        fillTextrueTiling = serializedObject.FindProperty("fillTextrueTiling_");
        fillTextureOffset = serializedObject.FindProperty("fillTextureOffset_");
        customFillMaterial = serializedObject.FindProperty("customFillMaterial_");

        stroke = serializedObject.FindProperty("stroke_");
        strokeType = serializedObject.FindProperty("strokeType_");
        strokeSolid = serializedObject.FindProperty("strokeSolid_");
        strokeGradient = serializedObject.FindProperty("strokeGradient_");
        strokeWidth = serializedObject.FindProperty("strokeWidth_");
        strokeTexture = serializedObject.FindProperty("strokeTexture_");
        customStrokeMaterial = serializedObject.FindProperty("customStrokeMaterial_");

        shapeAnchors = serializedObject.FindProperty("shapeAnchors_");
        shapeRadiil = serializedObject.FindProperty("shapeRadii_");
        radiiSmoothness = serializedObject.FindProperty("radiiSmoothness_");

        sortingLayer = serializedObject.FindProperty("sortingLayer_");
        sortingOrder = serializedObject.FindProperty("sortingOrder_");
        colliderMode = serializedObject.FindProperty("colliderMode_");

        setColliderTo = serializedObject.FindProperty("setColliderTo_");
        showComponents = serializedObject.FindProperty("showComponents_");
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
                    EditorGUILayout.PropertyField(fillAngle);

                }
                else if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.RadialGradient)
                {
                    EditorGUILayout.PropertyField(fillColor1, new GUIContent("Color 1"));
                    EditorGUILayout.PropertyField(fillColor2, new GUIContent("Color 2"));
                    EditorGUILayout.PropertyField(fillOffset1, new GUIContent("X"));
                    EditorGUILayout.PropertyField(fillOffset2, new GUIContent("Y"));
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
                    EditorGUILayout.PropertyField(strokeGradient);
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
