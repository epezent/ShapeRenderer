using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class TrapezoidShape : Shape {

    public float widthTop = 100;
    public float widthBottom = 200;
    public float height = 100;
    public float shift = 0;
    public float cornerRadius = 0f;
    [Range(0,100)]
    public int cornerSmoothness = 50;

    public override void Draw()
    {
        float half_wt = widthTop * 0.5f;
        float half_wb = widthBottom * 0.5f;
        float half_h = height * 0.5f;
        Vector2[] shapeAnchors = new Vector2[4] {
            new Vector2(half_wt+shift, half_h),
            new Vector2(-half_wt+shift, half_h),
            new Vector2(-half_wb, -half_h),
            new Vector2(half_wb, -half_h)
        };
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[4] { cornerRadius, cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[4] { cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }
}
