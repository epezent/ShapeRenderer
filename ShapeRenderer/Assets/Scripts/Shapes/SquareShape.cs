using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

//[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class SquareShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float sideLength = 200;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float cornerRadius = 0f;
    [Range(0,100)]
    public int cornerSmoothness = 50;

    // ShapeRenderer Component
    private ShapeRenderer sr;

    private void Awake()
    {
        sr = GetComponent<ShapeRenderer>();
        if (sr == null)
            sr = gameObject.AddComponent<ShapeRenderer>() as ShapeRenderer;
    }

    // Use this for initialization
    void Start () {
        DrawShape();
    }

    // Update is called once per frame
    void Update () {
            DrawShape();
    }

    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
    {
        float half_s = sideLength * 0.5f;
        Vector2[] shapeAnchors = new Vector2[4] {
            new Vector2(half_s, half_s),
            new Vector2(-half_s, half_s),
            new Vector2(-half_s, -half_s),
            new Vector2(half_s, -half_s)
        };
        for (int i = 0; i < 4; i++)
            shapeAnchors[i] = ShapeRenderer.RotateVector2(shapeAnchors[i], rotation);

        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = new float[4] { cornerRadius, cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[4] { cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness };
    }

}
