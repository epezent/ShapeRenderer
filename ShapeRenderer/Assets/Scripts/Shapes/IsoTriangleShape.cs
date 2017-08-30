using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

public class IsoTriangleShape : Shape {

    public float width = 200;
    public float height = 200;
    public float cornerRadius = 0f;
    [Range(0, 100)]
    public int cornerSmoothness = 25;

    public override void Draw()
    {
        float half_w = width * 0.5f;
        float half_h = height * 0.5f;
        Vector2[] shapeAnchors = new Vector2[3];
        shapeAnchors[0] = new Vector2(half_w, -half_h);
        shapeAnchors[1] = new Vector2(0, half_h);
        shapeAnchors[2] = new Vector2(-half_w, -half_h);
        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[3] { cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[3] { cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }        
}
