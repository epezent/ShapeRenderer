using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class IsoTriangleShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float width = 200;
    public float height = 100;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float cornerRadius = 0f;
    [Range(0, 100)]
    public int cornerSmoothness = 50;

    // Previous Properties
    private float prevWidth;
    private float prevHeight;
    private float prevRotation;
    private float prevCornerRadius;
    private int prevCornerSmoothness;

    // ShapeRenderer Component
    private ShapeRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<ShapeRenderer>();
        CheckStateChange();
        DrawShape();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckStateChange())
            DrawShape();
    }  
    
    // Returns true if any shape properties have been changed
    bool CheckStateChange()
    {
        if (width != prevWidth ||
            height != prevHeight ||
            cornerRadius != prevCornerRadius ||
            cornerSmoothness != prevCornerSmoothness ||
            rotation != prevRotation)
        {
            prevWidth = width;
            prevHeight = height;
            prevCornerRadius = cornerRadius;
            prevCornerSmoothness = cornerSmoothness;
            prevRotation = rotation;
            return true;
        }
        return false;
    }

    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
    {
        float half_w = width * 0.5f;
        float half_h = height * 0.5f;

        sr.shapeAnchors = new Vector2[3];
        sr.shapeRadii = new float[3] { cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[3] { cornerSmoothness, cornerSmoothness, cornerSmoothness };

        sr.shapeAnchors[0] = new Vector2(half_w, -half_h);
        sr.shapeAnchors[1] = new Vector2(0, half_h);
        sr.shapeAnchors[2] = new Vector2(-half_w, -half_h);

        sr.shapeAnchors = ShapeRenderer.RotateVertices(sr.shapeAnchors, rotation);

        sr.UpdateShapeAll();
    }    
    
}
