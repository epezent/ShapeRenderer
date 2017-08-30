using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class SquareShape : Shape {

    public float sideLength = 200;
    public float cornerRadius = 0f;
    [Range(0,100)]
    public int cornerSmoothness = 25;

    public override void Draw()
    {
        float half_s = sideLength * 0.5f;
        Vector2[] shapeAnchors = new Vector2[4] {
            new Vector2(half_s, half_s),
            new Vector2(-half_s, half_s),
            new Vector2(-half_s, -half_s),
            new Vector2(half_s, -half_s)
        };
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[4] { cornerRadius, cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[4] { cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }

}
