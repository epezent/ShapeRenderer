using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class HeartShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float size = 200.0f;
    [Range(-1.0f, 1.0f)]
    public float scaleX = 1.0f;
    [Range(-1.0f, 1.0f)]
    public float scaleY = 1.0f;
    [Range(0.0f, 360.0f)]
    public float rotation = 0.0f;
    [Range(3, 100)]
    public int smoothness = 50;

    // Previous Properties
    private float prevSize;
    private float prevScaleX;
    private float prevScaleY;
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
        if (size != prevSize ||
            scaleX != prevScaleX ||
            scaleY != prevScaleY ||
            rotation != prevRotation ||
            smoothness != prevSmoothness)
        {
            prevSize = size;
            prevScaleX = scaleX;
            prevScaleY = scaleY;
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
            sr.shapeAnchors[i] = new Vector2(16.0f * Mathf.Pow(Mathf.Sin(anchorAngle), 3),
                13.0f * Mathf.Cos(anchorAngle) - 5.0f * Mathf.Cos(2.0f * anchorAngle) - 2.0f * Mathf.Cos(3.0f * anchorAngle) - Mathf.Cos(4.0f * anchorAngle)) * size * 6.0f / 200.0f;
            sr.shapeAnchors[i].x *= scaleX;
            sr.shapeAnchors[i].y *= scaleY;
            sr.shapeAnchors[i] = ShapeRenderer.RotateVector2(sr.shapeAnchors[i], rotation);
            sr.shapeRadii[i] = 0;
            sr.radiiSmoothness[i] = 0;
        }
        sr.UpdateShapeAll();
    }
}
