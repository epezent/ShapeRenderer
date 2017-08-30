using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class DiamondShape : Shape {

    public float topWidth = 100;
    public float centerWidth = 200;
    public float height = 200;
    public float shift = 50;
    public float cornerRadius = 0f;
    [Range(0,100)]
    public int cornerSmoothness = 25;

    public override void Draw()
    {
        float half_tw = topWidth * 0.5f;
        float half_cw = centerWidth * 0.5f;
        float half_h = height * 0.5f;
        Vector2[] shapeAnchors = new Vector2[5] {
            new Vector2(half_cw, shift),
            new Vector2(half_tw, half_h),
            new Vector2(-half_tw, half_h),
            new Vector2(-half_cw, shift),
            new Vector2(0, -half_h)
        };
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[5] { cornerRadius, cornerRadius, cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[5] { cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }

}
