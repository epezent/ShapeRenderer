using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[RequireComponent(typeof(ShapeRenderer))]
[ExecuteInEditMode]
public class TrapezoidShape : MonoBehaviour {

    [Header("Shape Properties")]
    public float widthTop = 100;
    public float widthBottom = 200;
    public float height = 100;
    public float shift = 0;
    [Range(0.0f, 360.0f)]
    public float rotation = 0f;
    public float cornerRadius = 0f;
    [Range(0,100)]
    public int cornerSmoothness = 50;

    // Previous Properties
    private float prevWidthTop;
    private float prevWidthBottom;
    private float prevHeight;
    private float prevShift;
    private float prevCornerRadius;
    private int prevCornerSmoothness;
    private float prevRotation;

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
        if (widthTop != prevWidthTop || 
            widthBottom != prevWidthBottom ||
            shift != prevShift ||
            height != prevHeight ||
            cornerRadius != prevCornerRadius ||
            cornerSmoothness != prevCornerSmoothness ||
            rotation != prevRotation)
        {
            prevWidthTop = widthTop;
            prevWidthBottom = widthBottom;
            prevShift = shift;
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
        float half_wt = widthTop * 0.5f;
        float half_wb = widthBottom * 0.5f;
        float half_h = height * 0.5f;
        sr.shapeAnchors = new Vector2[4] {
            new Vector2(half_wt+shift, half_h),
            new Vector2(-half_wt+shift, half_h),
            new Vector2(-half_wb, -half_h),
            new Vector2(half_wb, -half_h)
        };
        sr.shapeRadii = new float[4] { cornerRadius, cornerRadius, cornerRadius, cornerRadius };
        sr.radiiSmoothness = new int[4] { cornerSmoothness, cornerSmoothness, cornerSmoothness, cornerSmoothness };

        for (int i = 0; i < 4; i++)
            sr.shapeAnchors[i] = ShapeRenderer.RotateVector2(sr.shapeAnchors[i], rotation);
        
        sr.UpdateShapeAll();
    }

}
