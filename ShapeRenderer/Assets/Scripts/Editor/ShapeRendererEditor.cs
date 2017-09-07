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
    SerializedProperty rotation;
    SerializedProperty scale;
    SerializedProperty mirrorX;
    SerializedProperty mirrorY;

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
        fillOffset2 = serializedObject.FindProperty("fillOffset2_");
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
        rotation = serializedObject.FindProperty("rotation_");
        scale = serializedObject.FindProperty("scale_");
        mirrorX = serializedObject.FindProperty("mirrorX_");
        mirrorY = serializedObject.FindProperty("mirrorY_");

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

        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.PropertyField(fill, new GUIContent("Fill", "Enables/disables shape fill."));
        EditorStyles.label.fontStyle = FontStyle.Normal;

        if (fill.boolValue != false)
        {

            EditorGUILayout.PropertyField(fillType, new GUIContent("Fill Type", "The fill type to use."));
            if ((ShapeRenderer.FillType)fillType.enumValueIndex != ShapeRenderer.FillType.Custom)
            {
                if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.Solid)
                {
                    EditorGUILayout.PropertyField(fillColor1, new GUIContent("Fill Color", "The fill color of the shape."));
                }
                else if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.LinearGradient)
                {
                    EditorGUILayout.PropertyField(fillColor1, new GUIContent("Fill Color 1", "The frist fill color of the gradient."));
                    EditorGUILayout.PropertyField(fillColor2, new GUIContent("Fill Color 2", "The second fill color of the gradient."));
                    EditorGUILayout.PropertyField(fillAngle, new GUIContent("Fill Angle", "The angle at which the linear gradient is applied."));

                }
                else if ((ShapeRenderer.FillType)fillType.enumValueIndex == ShapeRenderer.FillType.RadialGradient)
                {
                    EditorGUILayout.PropertyField(fillColor1, new GUIContent("Fill Color 1", "The frist fill color of the gradient."));
                    EditorGUILayout.PropertyField(fillColor2, new GUIContent("Fill Color 2", "The second fill color of the gradient."));
                    EditorGUILayout.PropertyField(fillOffset1, new GUIContent("Offset X", "The position of the gradient along the X axis."));
                    EditorGUILayout.PropertyField(fillOffset2, new GUIContent("Offset Y", "The position of the gradient along the Y axis."));
                }
                EditorGUILayout.PropertyField(fillTexture, new GUIContent("Fill Texture", "The texture to be appled to the fill."));
                if ((Texture)fillTexture.objectReferenceValue != null)
                {
                    EditorGUILayout.PropertyField(fillTextrueTiling, new GUIContent("Tiling", "The texture tiling in X and Y directions."));
                    EditorGUILayout.PropertyField(fillTextureOffset, new GUIContent("Offset", "The texture offset in X and Y directions."));
                }
            }
            else
            {
                EditorGUILayout.PropertyField(customFillMaterial, new GUIContent("Fill Material", "The custom material to be applied to the fill."));
            }

        }


        EditorStyles.label.fontStyle = FontStyle.Bold;
        EditorGUILayout.PropertyField(stroke, new GUIContent("Stroke", "Enables/disables shape stroke."));
        EditorStyles.label.fontStyle = FontStyle.Normal;

        if (stroke.boolValue != false)
        {

            EditorGUILayout.PropertyField(strokeType, new GUIContent("Stroke Type", "The stroke type to use."));
            if ((ShapeRenderer.StrokeType)strokeType.enumValueIndex != ShapeRenderer.StrokeType.Custom)
            {
                if ((ShapeRenderer.StrokeType)strokeType.enumValueIndex == ShapeRenderer.StrokeType.Solid)
                    EditorGUILayout.PropertyField(strokeSolid, new GUIContent("Stroke Color", "The stroke color of the shape."));
                else if ((ShapeRenderer.StrokeType)strokeType.enumValueIndex == ShapeRenderer.StrokeType.MultiGradient)
                    EditorGUILayout.PropertyField(strokeGradient, new GUIContent("Stroke Gradient", "The gradient describing the color along the stroke."));
                EditorGUILayout.PropertyField(strokeTexture, new GUIContent("Stroke Texture", "The texture to be applied to the stroke"));
            }
            else
            {
                EditorGUILayout.PropertyField(customStrokeMaterial, new GUIContent("Stroke Material", "The custom material to be applied to the stroke."));
            }
            EditorGUILayout.PropertyField(strokeWidth, new GUIContent("Stroke Width", "The shape stroke width in world units."));

        }

        EditorGUILayout.LabelField("Geometry", EditorStyles.boldLabel);

        if (((ShapeRenderer)target).shape != null)
            GUI.enabled = false;

        EditorGUILayout.PropertyField(shapeAnchors, new GUIContent("Anchors", "The shape anchor points in world units, relative to this GameObject's transform."), true);
        EditorGUILayout.PropertyField(shapeRadiil, new GUIContent("Radii", "The radii, in world units, applied to corresponding shape anchor points."), true);
        EditorGUILayout.PropertyField(radiiSmoothness, new GUIContent("Smoothness", "The number of line segment used to render each radius. Use as few as necessary for best performance."), true);

        if (((ShapeRenderer)target).shape != null)
            GUI.enabled = true;


        EditorGUILayout.PropertyField(rotation, new GUIContent("Rotation", "The rotation applied to the shape in degrees."));
        EditorGUILayout.PropertyField(scale, new GUIContent("Scale", "The scaling applied to the shape."));
        EditorGUILayout.PropertyField(mirrorX, new GUIContent("Mirror X", "Mirror the shape from left to right."));
        EditorGUILayout.PropertyField(mirrorY, new GUIContent("Mirror Y", "Mirror the shape from bottom to top."));

        EditorGUILayout.LabelField("Sorting Layer", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sortingLayer, new GUIContent("Sorting Layer", "The name of the ShapeRenderer's sorting layer."));
        EditorGUILayout.PropertyField(sortingOrder, new GUIContent("Order in Layer", "The ShapeRenderer's order within a sorting layer."));

        EditorGUILayout.LabelField("Other Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(colliderMode, new GUIContent("Collider Mode", "The mode that describes the interface between the ShapeRenderer and PolygonCollider2D."));
        if ((ShapeRenderer.ColliderMode)colliderMode.enumValueIndex == ShapeRenderer.ColliderMode.ToCollider)
            EditorGUILayout.PropertyField(setColliderTo, new GUIContent("Collider Points", "The shape geometry that will be used to as the collider points"));
        EditorGUILayout.PropertyField(showComponents, new GUIContent("Show Components", "Shows/hides the LineRenderer, MeshFilter, and MeshRenderer required by this ShapeRenderer in the Inspector. Hidden by default to reduce clutter."));

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }          

}
