using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        ValidateVertices();

        // Check for a PolgonCollider2D
        pc2d = GetComponent<PolygonCollider2D>();

        // Create a new Mesh
        mf.mesh = new Mesh();   

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
        ValidateRanges();
        ValidateVertices();
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

    private void ValidateComponents()
    {
        // validate LineRenderer
        lr = GetComponent<LineRenderer>();
        if (lr == null)
            lr = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        // validate MeshFilter
        mf = GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>() as MeshFilter;
        // validate MeshRenderer
        mr = GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        // validate MaterialPropertyBlock
        if (mpb == null)
            mpb = new MaterialPropertyBlock();
    }

    private void ValidateVertices()
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
    }

    void Update()
    {
        #if UNITY_EDITOR
        ShowComponenets();
        if (!EditorApplication.isPlaying)
            UpdateShapeAll(); // we don't want to call this every frame when the game is playing in the editor
        #endif
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
        ValidateVertices();
        Vector2[] vertices = GenerateVertices(shapeAnchors, shapeRadii, radiiSmoothness);
        if (vertices != null)
        {
            UpdateFillGeometry(vertices);
            UpdateStrokeGeometry(vertices);
            UpdateCollider(vertices);
        }
    }

    /// <summary>
    /// Updates the shape fill geometry.
    /// </summary>
    public void UpdateFillGeometry(Vector2[] vertices)
    {
        if (fill)
        {
            mf.mesh = GenerateMesh2(vertices);
        }
        else
        {
            mf.mesh = null;
        }
    }    

    /// <summary>
    /// Updates the attached PolygonCollider2D points if updateCollider is true. If no PC2D is attached, a new instance is created.
    /// </summary>
    public void UpdateCollider(Vector2[] vertices)
    {
        if (colliderMode == ColliderMode.ToCollider)
        {
            if (pc2d == null)
                pc2d = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
            if (setColliderTo == SetColliderTo.Anchors)
                pc2d.points = shapeAnchors;
            if (setColliderTo == SetColliderTo.Vertices)
                pc2d.points = vertices;
        } 
        else if (colliderMode == ColliderMode.FromCollider)
        {
            if (pc2d == null)
            {
                pc2d = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
                pc2d.points = vertices;
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

    /// <summary>
    /// Updates the shape stroke geometry.
    /// </summary>
    public void UpdateStrokeGeometry(Vector2[] vertices)
    {
        if (stroke)
        {
            lr.loop = true;
            lr.useWorldSpace = false;
            lr.positionCount = vertices.Length;
            Vector3[] positions = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                positions[i] = new Vector3(vertices[i].x, vertices[i].y, transform.position.z);
            lr.SetPositions(positions);
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

    //-------------------------------------------------------------------------
    // PUBLIC STATIC FUNCTIONS
    //-------------------------------------------------------------------------

    /// <summary>
    /// Generates a 2D shape mesh given array of 2D verticies.
    /// </summary>
    public static Mesh GenerateMesh1(Vector2[] vertices)
    {
        Mesh newMesh = new Mesh();
        int nPoints = vertices.Length;
        Vector3[] points = new Vector3[nPoints];
        Vector2[] uv = new Vector2[nPoints];
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;
        for (int i = 0; i < nPoints; i++)
        {
            Vector2 actual = vertices[i];
            minX = Mathf.Min(minX, actual.x);
            maxX = Mathf.Max(maxX, actual.x);
            minY = Mathf.Min(minY, actual.y);
            maxY = Mathf.Max(maxY, actual.y);
            points[i] = new Vector3(actual.x, actual.y, 0);

        }
        float denX = 1.0f / (maxX - minX);
        float denY = 1.0f / (maxY - minY);
        for (int i = 0; i < nPoints; i++)
        {
            Vector2 actual = vertices[i];
            float u = (actual.x - minX) * denX;
            float v = (actual.y - minY) * denY;
            uv[i] = new Vector2(u, v);
        }

        Triangulator tr = new Triangulator(vertices);
        int[] triangles = tr.Triangulate();

        newMesh.Clear();
        newMesh.vertices = points;
        newMesh.triangles = triangles;
        newMesh.uv = uv;

        return newMesh;
    }

    public static Mesh GenerateMesh2(Vector2[] vertices)
    {
        Mesh newMesh = new Mesh();
        int nPoints = vertices.Length;
        Vector3[] points = new Vector3[nPoints];
        Vector2[] uv = new Vector2[nPoints];
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;
        for (int i = 0; i < nPoints; i++)
        {
            Vector2 actual = vertices[i];
            minX = Mathf.Min(minX, actual.x);
            maxX = Mathf.Max(maxX, actual.x);
            minY = Mathf.Min(minY, actual.y);
            maxY = Mathf.Max(maxY, actual.y);
            points[i] = new Vector3(actual.x, actual.y, 0);

        }
        float denX = 1.0f / (maxX - minX);
        float denY = 1.0f / (maxY - minY);
        for (int i = 0; i < nPoints; i++)
        {
            Vector2 actual = vertices[i];
            float u = (actual.x - minX) * denX;
            float v = (actual.y - minY) * denY;
            uv[i] = new Vector2(u, v);
        }

        Triangulator tr = new Triangulator(vertices);

        List<int> indices1 = null;
        List<Vector3> vertices1 = null;
        List<List<Vector2>> holes = new List<List<Vector2>>();


        TriangleDotNET.Triangulate2(vertices, holes, out indices1, out vertices1);
        //int[] triangles = tr.Triangulate();

        newMesh.Clear();
        newMesh.vertices = vertices1.ToArray();
        newMesh.triangles = indices1.ToArray();
        newMesh.uv = uv;

        return newMesh;
    }




    /// <summary>
    /// Rounds array of anchors with corresponding array of radii and array of number
    /// of interpolated vertices N. Paramters anchors, radii, and N must be the same length.
    /// </summary>
    public static Vector2[] GenerateVertices(Vector2[] anchors, float[] radii, int[] N)
    {
        if (anchors.Length != radii.Length || anchors.Length != N.Length || radii.Length != N.Length)
            return null;

        List<Vector2> newVerticesList = new List<Vector2>();
        for (int i = 0; i < anchors.Length; i++)
        {
            if (radii[i] > 0.0f && N[i] > 1)
            {
                Vector2 a, b, c;
                if (i == 0)
                {
                    a = anchors[anchors.Length - 1];
                    b = anchors[i];
                    c = anchors[i + 1];
                }
                else if (i == anchors.Length - 1)
                {
                    a = anchors[i - 1];
                    b = anchors[i];
                    c = anchors[0];
                }
                else
                {
                    a = anchors[i - 1];
                    b = anchors[i];
                    c = anchors[i + 1];
                }
                newVerticesList.AddRange(RoundCorner(a, b, c, radii[i], N[i]));
            }
            else
                newVerticesList.Add(anchors[i]);
        }
        return newVerticesList.ToArray();
    }

    /// <summary>
    /// Rounds anchor b with radius r and number of interpolated vertices n given consecutive anchors a, b, and c.
    /// </summary>
    public static Vector2[] RoundCorner(Vector2 a, Vector2 b, Vector2 c, float r, int n)
    {
        // Find directional vectors of line segments
        Vector2 v1 = b - a;
        Vector2 v2 = b - c;

        // Check if corner radius is longer than vectors
        if (r >= v1.magnitude || r >= v2.magnitude)
        {
            print("Radius is larger than the side lengths.");
            return new Vector2[] { b };
        }

        // Find unit vectors
        Vector2 u1 = v1 / v1.magnitude;
        Vector2 u2 = v2 / v2.magnitude;

        // Find normal vectors
        Vector2 n1, n2;
        n1.x = -u1.y; n1.y = u1.x;
        n2.x = -u2.y; n2.y = u2.x;

        // Check/fix direction of normal vector
        if (Vector2.Dot(n1, -v2) < 0)
            n1 = -n1;
        if (Vector2.Dot(n2, -v1) < 0)
            n2 = -n2;

        // Find end-points of offset lines
        Vector2 o11 = a + r * n1;
        Vector2 o10 = b + r * n1;
        Vector2 o22 = c + r * n2;
        Vector2 o20 = b + r * n2;

        // Find intersection point of offset lines
        Vector2 x = FindIntersection(o11, o10, o22, o20);

        // Find tangent points
        Vector2 t1 = x + r * -n1;
        Vector2 t2 = x + r * -n2;

        // Check if tangent points are on line segments
        //if (!IamondMath.IsPointInsideLine(a,b,t1) || !IamondMath.IsPointInsideLine(c,b,t2))
        //return new Vector2[] { b };

        float angle1 = Mathf.Atan2(t1.y - x.y, t1.x - x.x);
        float angle2 = Mathf.Atan2(t2.y - x.y, t2.x - x.x);
        float[] angles = new float[n];

        if (Mathf.Abs(angle1 - angle2) < Mathf.PI)
            angles = LinSpace(angle1, angle2, n);
        else if (Mathf.Abs(angle1 - angle2) > Mathf.PI)
            angles = LinSpace(WrapTo2Pi(angle1), WrapTo2Pi(angle2), n);

        Vector2[] verts = new Vector2[n];
        for (int i = 0; i < n; i++)
        {
            Vector2 vertex;
            vertex.x = r * Mathf.Cos(angles[i]) + x.x;
            vertex.y = r * Mathf.Sin(angles[i]) + x.y;
            verts[i] = vertex;
        }

        return verts;
    }

    public static Vector2 FindIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float x1 = a1.x; float y1 = a1.y;
        float x2 = a2.x; float y2 = a2.y;
        float x3 = b1.x; float y3 = b1.y;
        float x4 = b2.x; float y4 = b2.y;

        Vector2 intersection;
        intersection.x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
        intersection.y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
        return intersection;
    }

    public static bool IsPointInsideLine(Vector2 l1, Vector2 l2, Vector2 p)
    {
        // This code can be optimized by creating variables for expressions below
        float crossproduct = (p.y - l1.y) * (l2.x - l1.x) - (p.x - l1.x) * (l2.y - l1.y);
        if (Mathf.Abs(crossproduct) > 0.1)
        {
            print(Mathf.Abs(crossproduct));
            print("Cross product is not zero.");
            return false;
        }


        float dotproduct = (p.x - l1.x) * (l2.x - l1.x) + (p.y - l1.y) * (l2.y - l1.y);
        if (dotproduct < 0.0f)
        {
            print("Dot product is not less than zero.");
            return false;
        }

        float squarelength = (l2.x - l1.x) * (l2.x - l1.x) + (l2.y - l1.y) * (l2.y - l1.y);
        if (dotproduct > squarelength)
        {
            print("Dot product is not greater than square length.");
            return false;
        }

        return true;
    }

    public static float[] LinSpace(float a, float b, int n)
    {
        float[] linspace = new float[n];
        float delta = (b - a) / (n - 1);

        linspace[0] = a;
        for (int i = 1; i < n - 1; i++)
            linspace[i] = linspace[i - 1] + delta;

        linspace[n - 1] = b;
        return linspace;
    }

    public static float WrapTo2Pi(float angle)
    {
        if (angle < 0)
            return angle + 2 * Mathf.PI;
        return angle;
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
