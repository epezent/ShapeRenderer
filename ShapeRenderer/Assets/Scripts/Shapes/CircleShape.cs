﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class CircleShape : Shape {

    public float radius = 100;
    [Range(3, 100)]
    public int smoothness = 25;

    public override void Draw()
    {
        float angleIncrement = 2 * Mathf.PI / smoothness;
        Vector2[] shapeAnchors = new Vector2[smoothness];
        float[] shapeRadii = new float[smoothness];
        int[] radiiSmoothness = new int[smoothness];
        for (int i = 0; i < smoothness; i++)
        {
            float anchorAngle = i * angleIncrement;
            shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * radius;
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
