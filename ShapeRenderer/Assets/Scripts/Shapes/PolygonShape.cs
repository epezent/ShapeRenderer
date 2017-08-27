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

    // ShapeRenderer Component
    private ShapeRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<ShapeRenderer>();
        DrawShape();
    }

    // Update is called once per frame
    void Update()
    {
        DrawShape();
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

        Vector2[] shapeAnchors = new Vector2[sides];
        float[] shapeRadii = new float[sides];
        int[] radiiSmoothness = new int[sides];

        for (int i = 0; i < sides; i++)
        {
            float anchorAngle = i * angleIncrement + offset;
            shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * r;
            shapeRadii[i] = cornerRadius;
            radiiSmoothness[i] = cornerSmoothness;
        }

        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
        sr.Update();
    }    
    
}
