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
        float angleIncrement = 2 * Mathf.PI / smoothness;
        Vector2[] shapeAnchors = new Vector2[smoothness];
        float[] shapeRadii = new float[smoothness];
        int[] radiiSmoothness = new int[smoothness];
        for (int i = 0; i < smoothness; i++)
        {
            float anchorAngle = i * angleIncrement;
            shapeAnchors[i] = ShapeRenderer.RotateVector2(new Vector2(Mathf.Cos(anchorAngle) * radiusA, Mathf.Sin(anchorAngle) * radiusB), rotation);
            shapeRadii[i] = 0;
            radiiSmoothness[i] = 0;
        }

        sr.shapeAnchors = shapeAnchors;
        sr.shapeRadii = shapeRadii;
        sr.radiiSmoothness = radiiSmoothness;
    }
}
