using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class ElipseShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float radiusA = 100;
    public float radiusB = 50;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    [Range(3,100)]
    public int smoothness = 50;

    // Previous Properties
    private float prevRadiusA;
    private float prevRadiusB;
    private float prevRotation;
    private int prevSmoothness;

    // ShapeRenderer Component
    private ShapeRenderer sr;

    // Use this for initialization
    void Start () {
        sr = GetComponent<ShapeRenderer>();
        CheckStateChange();
        DrawShape();
	}
	
	// Update is called once per frame
	void Update () {
        if (CheckStateChange())
            DrawShape();
	}

    // Returns true if any shape properties have been changed
    bool CheckStateChange()
    {
        if (radiusA != prevRadiusA ||
            radiusB != prevRadiusB ||
            rotation != prevRotation ||
            smoothness != prevSmoothness)
        {
            prevRadiusA = radiusA;
            prevRadiusB = radiusB;
            prevRotation = rotation;
            prevSmoothness = smoothness;
            return true;
        }
        return false;
    }

    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
    {
        float angleIncrement = 2 * Mathf.PI / smoothness;
        sr.shapeAnchors = new Vector2[smoothness];
        sr.shapeRadii = new float[smoothness];
        sr.radiiSmoothness = new int[smoothness];
        for (int i = 0; i < smoothness; i++)
        {
            float anchorAngle = i * angleIncrement;
            sr.shapeAnchors[i] = ShapeRenderer.RotateVector2(new Vector2(Mathf.Cos(anchorAngle) * radiusA, Mathf.Sin(anchorAngle) * radiusB), rotation);
            sr.shapeRadii[i] = 0;
            sr.radiiSmoothness[i] = 0;
        }
        sr.UpdateShapeAll();
    }
}
