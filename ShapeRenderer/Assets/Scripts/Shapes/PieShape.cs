﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class PieShape : Shape {

    public float radius = 100;
    [Range(0.0f, 360.0f)]
    public float angle = 90;
    public float insideRadius = 0;
    [Range(3, 100)]
    public int smoothness = 50;

    public override void Draw()
    {
        float angleIncrement = (2 * Mathf.PI - Mathf.Deg2Rad * angle) / (smoothness - 1);
        float offset = Mathf.Deg2Rad * (angle/2);
        Vector2[] shapeAnchors = new Vector2[smoothness+1];
        float[] shapeRadii = new float[smoothness+1];
        int[] radiiSmoothness = new int[smoothness+1];
        shapeAnchors[0] = Vector2.zero;
        shapeRadii[0] = insideRadius;
        radiiSmoothness[0] = smoothness;
        for (int i = 1; i < smoothness + 1; i++)
        {
            float anchorAngle = (i-1) * angleIncrement + offset;
            shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * radius;
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
