using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class StarShape : Shape {

    [Range(2,25)]
    public int points = 5;
    public float radiusA = 100;
    public float radiusB = 50;
    public float cornerRadiusA = 0;
    public float cornerRadiusB = 0;
    [Range(3, 100)]
    public int smoothness = 50;

    public override void Draw()
    {
        float angleIncrement =  Mathf.PI / points;
        float offset = Mathf.PI * 0.5f;
        Vector2[] shapeAnchors = new Vector2[2 * points];
        float[] shapeRadii = new float[2 * points];
        int[] radiiSmoothness = new int[2 * points];
        for (int i = 0; i < points; i++)
        {
            float anchorAngleA = 2 * i * angleIncrement + offset;
            float anchorAngleB = (2 * i + 1) * angleIncrement + offset;
            shapeAnchors[2 * i] = new Vector2(radiusA * Mathf.Cos(anchorAngleA), radiusA * Mathf.Sin(anchorAngleA));
            shapeAnchors[2 * i + 1] = new Vector2(radiusB * Mathf.Cos(anchorAngleB), radiusB * Mathf.Sin(anchorAngleB));
            shapeRadii[2 * i] = cornerRadiusA;
            shapeRadii[2 * i + 1] = cornerRadiusB;
            radiiSmoothness[2 * i] = smoothness;
            radiiSmoothness[2 * i + 1] = smoothness;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
