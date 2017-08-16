using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class PieShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float radius = 100;
    [Range(0.0f, 360.0f)]
    public float angle = 90;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float insideRadius = 0;
    [Range(3, 100)]
    public int smoothness = 50;

    // Previous Properties
    private float prevRadius;
    private float prevAngle;
    private float prevRotation;
    private float prevInsideRadius;
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
        if (radius != prevRadius ||
            angle != prevAngle ||
            rotation != prevRotation ||
            insideRadius != prevInsideRadius ||
            smoothness != prevSmoothness)
        {
            prevRadius = radius;
            prevAngle = angle;
            prevRotation = rotation;
            prevInsideRadius = insideRadius;
            prevSmoothness = smoothness;
            return true;
        }
        return false;
    }

    // Updates the ShapeRender Mesh with the shape geometry
    void DrawShape()
    {
        float angleIncrement = (2 * Mathf.PI - Mathf.Deg2Rad * angle) / (smoothness - 1);
        float offset = Mathf.Deg2Rad * (rotation + angle/2);
        sr.shapeAnchors = new Vector2[smoothness+1];
        sr.shapeRadii = new float[smoothness+1];
        sr.radiiSmoothness = new int[smoothness+1];
        sr.shapeAnchors[0] = Vector2.zero;
        sr.shapeRadii[0] = insideRadius;
        sr.radiiSmoothness[0] = smoothness;
        for (int i = 1; i < smoothness + 1; i++)
        {
            float anchorAngle = (i-1) * angleIncrement + offset;
            sr.shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * radius;
            sr.shapeRadii[i] = 0;
            sr.radiiSmoothness[i] = 0;
        }
    }
}
