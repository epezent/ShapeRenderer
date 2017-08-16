using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class CircleShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float radius = 100;
    [Range(3, 100)]
    public int smoothness = 50;

    // Previous Properties
    private float prevRadius;
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
            smoothness != prevSmoothness)
        {
            prevRadius = radius;
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
            sr.shapeAnchors[i] = new Vector2(Mathf.Cos(anchorAngle), Mathf.Sin(anchorAngle)) * radius;
            sr.shapeRadii[i] = 0;
            sr.radiiSmoothness[i] = 0;
        }
        sr.UpdateShapeAll();
    }
}
