using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class PillShape : Shape {

    public float radius = 50;
    public float length = 100;
    [Range(3, 100)]
    public int smoothness = 25;

    public override void Draw()
    {
        Vector2[] shapeAnchors = new Vector2[2*smoothness];
        float[] shapeRadii = new float[2*smoothness];
        int[] radiiSmoothness = new int[2*smoothness];
        float[] angles = ShapeRenderer.LinSpace(-90 * Mathf.Deg2Rad, 90 * Mathf.Deg2Rad, smoothness);
        for (int i = 0; i < smoothness; i++)
        {
            float anchorAngle = angles[i];
            shapeAnchors[i] = new Vector2(length* 0.5f, 0) + new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * radius;
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }
        angles = ShapeRenderer.LinSpace(90 * Mathf.Deg2Rad, 270 * Mathf.Deg2Rad, smoothness);
        for (int i = smoothness; i < smoothness * 2; i++)
        {
            float anchorAngle = angles[i-smoothness];
            shapeAnchors[i] = new Vector2(-length * 0.5f, 0) + new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * radius;
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
