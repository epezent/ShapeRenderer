using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class PolygonShape : MonoBehaviour {

    public enum PolygonParameter { SideLength, CircumscribedRadius, InscribedRadius }

    [Header("Shape Properties")]
    [Range(3,25)]
    public int sides = 6;
    public PolygonParameter parameterType = PolygonParameter.CircumscribedRadius;
    public float parameterValue = 100;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float cornerRadius = 0f;
    [Range(0, 100)]
    public int cornerSmoothness = 50;

    // Previous Properties
    private int prevSides;
    private PolygonParameter prevParameter;
    private float prevValue;
    private float prevRotation;
    private float prevCornerRadius;
    private int prevCornerSmoothness;

    // ShapeRenderer Component
    private ShapeRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<ShapeRenderer>();
        CheckStateChange();
        DrawShape();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckStateChange())
            DrawShape();
    }  
    
    // Returns true if any shape properties have been changed
    bool CheckStateChange()
    {
        if (sides != prevSides ||
            parameterType != prevParameter ||
            parameterValue != prevValue ||
            cornerRadius != prevCornerRadius ||
            cornerSmoothness != prevCornerSmoothness ||
            rotation != prevRotation)
        {
            prevSides = sides;
            prevParameter = parameterType;
            prevValue = parameterValue;
            prevCornerRadius = cornerRadius;
            prevCornerSmoothness = cornerSmoothness;
            prevRotation = rotation;
            return true;
        }
        return false;
    }

    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
    {
        float r;
        float angleIncrement = 2 * Mathf.PI / sides;
        float offset = Mathf.Deg2Rad * rotation;

        if (parameterType == PolygonParameter.SideLength)
            r = parameterValue / (2 * Mathf.Sin(Mathf.PI / sides));
        else if (parameterType == PolygonParameter.InscribedRadius)
            r = parameterValue / Mathf.Cos(Mathf.PI / sides);
        else
            r = parameterValue;

        sr.shapeAnchors = new Vector2[sides];
        sr.shapeRadii = new float[sides];
        sr.radiiSmoothness = new int[sides];

        for (int i = 0; i < sides; i++)
        {
            float anchorAngle = i * angleIncrement + offset;
            sr.shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * r;
            sr.shapeRadii[i] = cornerRadius;
            sr.radiiSmoothness[i] = cornerSmoothness;
        }
        sr.UpdateShapeGeometry();
    }    
    
}
