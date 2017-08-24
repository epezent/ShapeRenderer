using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class DiamondShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float topWidth = 100;
    public float centerWidth = 200;
    public float height = 200;
    public float shift = 50;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float cornerRadius = 0f;
    [Range(0,100)]
    public int cornerSmoothness = 50;

    // ShapeRenderer Component
    private ShapeRenderer sr;

    // Use this for initialization
    void Start () {
        sr = GetComponent<ShapeRenderer>();
        DrawShape();
    }

    // Update is called once per frame
    void Update () {
        DrawShape();
    }

    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
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

        for (int i = 0; i < 5; i++)
            shapeAnchors[i] = ShapeRenderer.RotateVector2(shapeAnchors[i], rotation);

        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[5] { cornerRadius, cornerRadius, cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[5] { cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }

}
