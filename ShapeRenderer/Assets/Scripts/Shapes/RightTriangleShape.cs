using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class RightTriangleShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float width = 200;
    public float height = 100;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float cornerRadius = 0f;
    [Range(0, 100)]
    public int cornerSmoothness = 50;

    // ShapeRenderer Component
    private ShapeRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<ShapeRenderer>();
        DrawShape();
    }

    // Update is called once per frame
    void Update()
    {
        DrawShape();
    }  
    
    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
    {
        float half_w = width * 0.5f;
        float half_h = height * 0.5f;

        Vector2[] shapeAnchors = new Vector2[3];
        shapeAnchors[0] = new Vector2(half_w, -half_h);
        shapeAnchors[1] = new Vector2(-half_w, half_h);
        shapeAnchors[2] = new Vector2(-half_w, -half_h);
        shapeAnchors = ShapeRenderer.RotateVertices(shapeAnchors, rotation);

        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[3] { cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[3] { cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }    
    
}
