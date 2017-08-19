using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

[ExecuteInEditMode]
public class ShapeRenderer : MonoBehaviour
{
    public enum FillType { Solid, LinearGradient, RadialGradient};
    public enum ColliderMode { Disabled, ToCollider, FromCollider }
    public enum SetColliderTo { Anchors, Vertices }

    [Tooltip("Enables/disables shape fill.")]
    public bool fill = true;
    [Tooltip("The fill method to use.")]
    public FillType fillType = FillType.Solid;
    public Color fillColor1 = Color.white;
    public Color fillColor2 = Color.black;
    [Tooltip("The angle at which the linear gradient is applied")]
    [Range(0.0f, 360.0f)]
    public float angle;
    [Tooltip("Controls the position of the gradient along the first axis")]
    [Range(-1.0f, 1.0f)]
    public float slider1 = 0.0f;
    [Tooltip("Controls the position of the gradient along the second axis")]
    [Range(-1.0f, 1.0f)]
    public float slider2 = 0.0f;

    [Tooltip("Enables/disables shape stroke.")]
    public bool stroke = false;
    [Tooltip("The gradient describing the color along the stroke.")]
    public Gradient strokeColor;
    [Tooltip("The shape stroke width in world units.")]
    public float strokeWidth = 10;

    [Tooltip("The shape anchor points in world units, relative to this GameObject's transform.")]
    public Vector2[] shapeAnchors = new Vector2[4] { new Vector2(100, -100), new Vector2(100, 100), new Vector2(-100, 100), new Vector2(-100, -100) };
    [Tooltip("The radii, in world units, applied to corresponding shape anchor points.")]
    public float[] shapeRadii = new float[4] { 0f, 0f, 0f, 0f };
    [Tooltip("The number of line segment used to render each radius. Use as few as necessary for best performance.")]
    public int[] radiiSmoothness = new int[4] { 50, 50, 50, 50 };

    [Tooltip("The name of the ShapeRenderer's sorting layer. First add the desired sorting layer Unity's Layers dialog (top-right), then type it here.")]
    [SortingLayer]
    public int sortingLayer = 0;
    [Tooltip("The ShapeRenderer's order within a sorting layer.")]
    public int sortingOrder = 0;

    [Tooltip("Updates the attached PolygonCollider2D to match the shape geometry. Adds a new PolygonCollider2D if none exists.")]
    public ColliderMode colliderMode = ColliderMode.Disabled;
    public SetColliderTo setColliderTo = SetColliderTo.Anchors;
    [Tooltip("Shows/hides the LineRenderer, MeshFilter, and MeshRenderer required by this ShapeRenderer. Hidden by default to reduce clutter.")]
    public bool showComponents = false;

    private Material fillMaterial;
    private Material strokeMaterial;

    private int defaultSmoothness = 50;

    private LineRenderer lr;
    private MeshFilter mf;
    private MeshRenderer mr;
    private MaterialPropertyBlock mpb;
    private PolygonCollider2D pc2d;   

    void Awake()
    {
        // Add required Unity components
        ValidateComponents();
        ValidateRanges();
        ValidateAnchors();

        // Check for a PolgonCollider2D
        pc2d = GetComponent<PolygonCollider2D>();

        // Create a new Mesh
        mf.sharedMesh = new Mesh();
        mf.sharedMesh.MarkDynamic();

        // Set default Materials
        if (fillMaterial == null)
            fillMaterial = Resources.Load("SR_FillLinearGradient") as Material;
        if (strokeMaterial == null)
            strokeMaterial = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        // Set start sorting layers
        mr.sortingLayerID = sortingLayer;
        mr.sortingOrder = sortingOrder;
        mr.sortingLayerID = sortingLayer;
        mr.sortingOrder = sortingOrder;

        // Hide required components
        mr.hideFlags = HideFlags.HideInInspector;
        mf.hideFlags = HideFlags.HideInInspector;
        lr.hideFlags = HideFlags.HideInInspector;
    }

    // Called when script added or inspector element is changed (in editor only)
    public void OnValidate()
    {
        ValidateComponents();
        ValidateAnchors();
        ValidateRanges();
    }

    private void OnEnable()
    {
        mr.enabled = fill;
        lr.enabled = stroke;
    }

    private void OnDisable()
    {
        mr.enabled = false;
        lr.enabled = false;
    }

    private void Start()
    {
        #if UNITY_EDITOR
        PrefabUtility.DisconnectPrefabInstance(gameObject); // to allow for generic shape prefab templates
        #endif
        UpdateShapeAll();
    }

    void Update()
    {
        #if UNITY_EDITOR
        ShowComponenets();
        if (!EditorApplication.isPlaying)
            UpdateShapeAll(); // we don't want to call this every frame when the game is playing in the editor
        #endif
    }

    private void ValidateComponents()
    {
        // validate LineRenderer
        lr = GetComponent<LineRenderer>();
        if (lr == null)
            lr = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        // validate MeshFilter
        mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            mf = gameObject.AddComponent<MeshFilter>() as MeshFilter;
            mf.sharedMesh = new Mesh();
        }
        // validate MeshRenderer
        mr = GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        // validate MaterialPropertyBlock
        if (mpb == null)
            mpb = new MaterialPropertyBlock();
    }

    private void ValidateAnchors()
    {
        if (shapeRadii.Length != shapeAnchors.Length)
            Array.Resize(ref shapeRadii, shapeAnchors.Length);
        if (radiiSmoothness.Length != shapeAnchors.Length)
        {
            Array.Resize(ref radiiSmoothness, shapeAnchors.Length);
            for (int i = 0; i < radiiSmoothness.Length; i++)
            {
                if (radiiSmoothness[i] == 0)
                    radiiSmoothness[i] = defaultSmoothness;
            }
        }
    }

    private void ValidateRanges()
    {
        if (strokeWidth < 0)
            strokeWidth = 0;
        if (shapeAnchors.Length < 3)
            Array.Resize(ref shapeAnchors, 3);
        if (shapeRadii.Length < 3)
            Array.Resize(ref shapeRadii, 3);
        if (radiiSmoothness.Length < 3)
            Array.Resize(ref radiiSmoothness, 3);
        for (int i = 0; i < shapeAnchors.Length; i++)
        {
            if (shapeRadii[i] < 0.0f)
                shapeRadii[i] = 0.0f;
            if (radiiSmoothness[i] < 1)
                radiiSmoothness[i] = 1;
        }
    }

    /// <summary>
    /// Shows or hides required components in the inspector
    /// </summary>
    private void ShowComponenets()
    {
        if (showComponents)
        {
            mr.hideFlags = HideFlags.None;
            mf.hideFlags = HideFlags.None;
            lr.hideFlags = HideFlags.None;
        }
        else
        {
            mr.hideFlags = HideFlags.HideInInspector;
            mf.hideFlags = HideFlags.HideInInspector;
            lr.hideFlags = HideFlags.HideInInspector;
        }
    }

    /// <summary>
    /// Enables/disables certains components depending on what shape options have been selected.
    /// </summary>
    private void UpdateEnabled()
    {
        if (fill)
            mr.enabled = true;
        else
            mr.enabled = false;
        if (stroke)
            lr.enabled = true;
        else
            lr.enabled = false;
    }

    //-------------------------------------------------------------------------
    // DLL IMPORTS
    //-------------------------------------------------------------------------

    [DllImport("ShapeRenderer", EntryPoint = "compute_shape1")]
    private static extern int ComputeShape1(float[] anchorsX, float[] anchorsY, float[] radii, int[] N, int anchorsSize,
                                         float[] verticesX, float[] verticesY, int verticesSize,
                                         int[] indices, int indicesSize, float[] u, float[] v);

    [DllImport("ShapeRenderer", EntryPoint = "compute_shape2")]
    private static extern int ComputeShape2(float[] anchorsX, float[] anchorsY, float[] radii, int[] N, int anchorsSize,
                                         float[] verticesX, float[] verticesY, int verticesSize,
                                         int[] indices, int indicesSize, float[] u, float[] v);

    //-------------------------------------------------------------------------
    // PUBLIC API FUNCTIONS
    //-------------------------------------------------------------------------

    /// <summary>
    /// Generates new vertices then updates shape appearance fill and stroke.
    /// </summary>
    public void UpdateShapeAll()
    {
        UpdateShapeGeometry();
        UpdateShapeAppearance();
        UpdateEnabled();
    }

    /// <summary>
    /// Updates the shape fill and stroke geometry.
    /// </summary>
    public void UpdateShapeGeometry()
    {
        ValidateAnchors();
        ValidateRanges();

        // calculate sizes
        int anchorsSize = shapeAnchors.Length;
        int verticesSize = 0;
        for (int i = 0; i < anchorsSize; i++)
        {
            if (radiiSmoothness[i] == 0 || radiiSmoothness[i] == 1 || shapeRadii[i] <= 0.0)
                verticesSize += 1;
            else
                verticesSize += radiiSmoothness[i];
        }
        int indicesSize = (verticesSize - 2) * 3;

        // unpack Unity types containers
        float[] anchorsX = new float[shapeAnchors.Length];
        float[] anchorsY = new float[shapeAnchors.Length];
        for (int i = 0; i < anchorsSize; ++i)
        {
            anchorsX[i] = shapeAnchors[i].x;
            anchorsY[i] = shapeAnchors[i].y;
        }
        float[] verticesX = new float[verticesSize];
        float[] verticesY = new float[verticesSize];
        float[] u = new float[verticesSize];
        float[] v = new float[verticesSize];
        int[] indices = new int[indicesSize];

        // call DLL
        int result = 0;
        if (showComponents)
            result = ComputeShape1(anchorsX, anchorsY, shapeRadii, radiiSmoothness, anchorsSize, verticesX, verticesY, verticesSize, indices, indicesSize, u, v);
        else
            result = ComputeShape2(anchorsX, anchorsY, shapeRadii, radiiSmoothness, anchorsSize, verticesX, verticesY, verticesSize, indices, indicesSize, u, v);


        if (result == 1)
        {
            // repack Unity types
            Vector3[] vertices = new Vector3[verticesSize];
            Vector2[] uv = new Vector2[verticesSize];
            float z = transform.position.z;
            for (int i = 0; i < verticesSize; i++)
            {
                vertices[i] = new Vector3(verticesX[i], verticesY[i], z);
                uv[i] = new Vector2(u[i], v[i]);
            }
            UpdateFillGeometry(vertices, indices, uv);
            UpdateStrokeGeometry(vertices);
            UpdateCollider(vertices);
        }
    }

    /// <summary>
    /// Updates the shape fill geometry.
    /// </summary>
    public void UpdateFillGeometry(Vector3[] vertices, int[] indices, Vector2[] uv)
    {
        if (fill)
        {
            GenerateMesh(vertices, indices, uv);
        }
        else
        {
            //mf.sharedMesh = null;
        }
    }    

   

    /// <summary>
    /// Updates the shape stroke geometry.
    /// </summary>
    public void UpdateStrokeGeometry(Vector3[] vertices)
    {
        if (stroke)
        {
            lr.loop = true;
            lr.useWorldSpace = false;
            lr.positionCount = vertices.Length;
            lr.SetPositions(vertices);
        }
        else
        {
            lr.positionCount = 0;
        }
    }

    /// <summary>
    /// Updates the shape fill and stroke appearance.
    /// </summary>
    void UpdateShapeAppearance()
    {
        UpdateFillAppearance();
        UpdateStrokeAppearance();
    }

    /// <summary>
    /// Updates the shape fill appearance.
    /// </summary>
    public void UpdateFillAppearance()
    {
        if (fill)
        {
            if (fillType == FillType.Solid)
            {
                fillMaterial = Resources.Load("SR_FillLinearGradient") as Material;
                mpb.SetColor("_Color1", fillColor1);
                mpb.SetColor("_Color2", fillColor1);
            }
            else if (fillType == FillType.LinearGradient)
            {
                fillMaterial = Resources.Load("SR_FillLinearGradient") as Material;
                mpb.SetColor("_Color1", fillColor1);
                mpb.SetColor("_Color2", fillColor2);
            } else if (fillType == FillType.RadialGradient)
            {
                fillMaterial = Resources.Load("SR_FillRadialGradient") as Material;
                mpb.SetColor("_Color1", fillColor1);
                mpb.SetColor("_Color2", fillColor2);
            }
            mpb.SetFloat("_Angle", angle);
            mpb.SetFloat("_Slider1", slider1);
            mpb.SetFloat("_Slider2", slider2);
            mr.SetPropertyBlock(mpb);
            mr.material = fillMaterial;
            mr.sortingLayerID = sortingLayer;
            mr.sortingOrder = sortingOrder;
        }
        else
        {
            mr.material = null;
        }
    }

    /// <summary>
    /// Updates the shape stroke appearance.
    /// </summary>
    public void UpdateStrokeAppearance()
    {
        if (stroke)
        {
            lr.material = strokeMaterial;
            lr.colorGradient = strokeColor;
            lr.startWidth = strokeWidth;
            lr.endWidth = strokeWidth;
            lr.sortingLayerID = sortingLayer;
            lr.sortingOrder = sortingOrder + 1;
        }
    }

    /// <summary>
    /// Updates the attached PolygonCollider2D points if updateCollider is true. If no PC2D is attached, a new instance is created.
    /// </summary>
    public void UpdateCollider(Vector3[] vertices)
    {
        if (colliderMode == ColliderMode.ToCollider)
        {
            if (pc2d == null)
                pc2d = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
            if (setColliderTo == SetColliderTo.Anchors)
                pc2d.points = shapeAnchors;
            if (setColliderTo == SetColliderTo.Vertices)
            {                
                pc2d.points = System.Array.ConvertAll<Vector3, Vector2>(vertices, Vector3toVector2);
            }
        }
        else if (colliderMode == ColliderMode.FromCollider)
        {
            if (pc2d == null)
            {
                pc2d = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
                pc2d.points = System.Array.ConvertAll<Vector3, Vector2>(vertices, Vector3toVector2);
            }
            else
            {
                shapeAnchors = pc2d.points;
                for (int i = 0; i < pc2d.points.Length; i++)
                {
                    shapeAnchors[i] += pc2d.offset;
                }
            }
        }
    }

    //-------------------------------------------------------------------------
    // PUBLIC STATIC FUNCTIONS
    //-------------------------------------------------------------------------

    public static Vector2 Vector3toVector2(Vector3 V3)
    {
        return new Vector2(V3.x, V3.y);
    }

    public void GenerateMesh(Vector3[] vertices, int[] indices, Vector2[] uv)
    {
        Mesh mesh = mf.sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.uv = uv;
    }


    public static Vector2 RotateVector2(Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }

    public static Vector2[] RotateVertices(Vector2[] vertices, float degrees)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = RotateVector2(vertices[i], degrees);
        }
        return vertices;
    }

}
