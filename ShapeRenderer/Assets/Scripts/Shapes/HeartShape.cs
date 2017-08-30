using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class HeartShape : Shape {

    public float size = 200.0f;
    [Range(-1.0f, 1.0f)]
    public float scaleX = 1.0f;
    [Range(-1.0f, 1.0f)]
    public float scaleY = 1.0f;
    [Range(3, 100)]
    public int smoothness = 50;

    public override void Draw()
    {
        float angleIncrement = 2 * Mathf.PI / smoothness;
        Vector2[] shapeAnchors = new Vector2[smoothness];
        float[] shapeRadii = new float[smoothness];
        int[] radiiSmoothness = new int[smoothness];
        for (int i = 0; i < smoothness; i++)
        {
            float anchorAngle = i * angleIncrement;
            shapeAnchors[i] = new Vector2(16.0f * Mathf.Pow(Mathf.Sin(anchorAngle), 3),
                13.0f * Mathf.Cos(anchorAngle) - 5.0f * Mathf.Cos(2.0f * anchorAngle) - 2.0f * Mathf.Cos(3.0f * anchorAngle) - Mathf.Cos(4.0f * anchorAngle)) * size * 6.0f / 200.0f;
            shapeAnchors[i].x *= scaleX;
            shapeAnchors[i].y *= scaleY;
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
