using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class ArcShape : Shape {

    public float radius = 100;
    [Range(-360.0f, 360.0f)]
    public float angle = 270;
    public float width = 20;
    [Range(3, 100)]
    public int smoothness = 25;

    public override void Draw()
    {
        float halfWidth = width * 0.5f;
        Vector2[] shapeAnchors = new Vector2[2 * smoothness];
        float[] shapeRadii = new float[2 * smoothness];
        int[] radiiSmoothness = new int[2 * smoothness];
        float[] angles = ShapeRenderer.LinSpace(0, angle * Mathf.Deg2Rad, smoothness);
        for (int i = 0; i < smoothness; i++)
        {
            shapeAnchors[i] = new Vector2(Mathf.Cos(angles[i]), Mathf.Sin(angles[i])) * (radius + halfWidth);
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }
        Array.Reverse(angles);
        for (int i = 0; i < smoothness; i++)
        {
            shapeAnchors[i+smoothness] = new Vector2(Mathf.Cos(angles[i]), Mathf.Sin(angles[i])) * (radius - halfWidth);
            shapeRadii[i+smoothness] = 0;
            radiiSmoothness[i+smoothness] = 0;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
