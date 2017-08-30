using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class PolygonShape : Shape {

    public enum PolygonParameter { SideLength, CircumscribedRadius, InscribedRadius }

    [Range(3,25)]
    public int sides = 6;
    public PolygonParameter parameterType = PolygonParameter.CircumscribedRadius;
    public float parameterValue = 100;
    public float cornerRadius = 0f;
    [Range(0, 100)]
    public int cornerSmoothness = 25;
        
    public override void Draw()
    {
        float r;
        float angleIncrement = 2 * Mathf.PI / sides;

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
            float anchorAngle = i * angleIncrement;
            shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * r;
            shapeRadii[i] = cornerRadius;
            radiiSmoothness[i] = cornerSmoothness;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }        
}
