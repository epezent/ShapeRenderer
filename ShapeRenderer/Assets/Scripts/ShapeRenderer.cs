using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //-------------------------------------------------------------------------
    // SHAPE FILL APPEARANCE
    //-------------------------------------------------------------------------

    public enum FillType { Solid, LinearGradient, RadialGradient, Custom };

    [SerializeField]
    private bool fill_ = true;
    public bool fill
    {
        get { return fill_; }
        set
        {
            if (fill_ != value)
            {
                fill_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    private FillType fillType_ = FillType.Solid;
    public FillType fillType
    {
        get { return fillType_; }
        set
        {
            if (fillType_ != value)
            {
                fillType_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    private Color fillColor1_ = Color.white;
    public Color fillColor1
    {
        get { return fillColor1_; }
        set
        {
            if (fillColor1_ != value)
            {
                fillColor1_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    private Color fillColor2_ = Color.black;
    public Color fillColor2
    {
        get { return fillColor2_; }
        set
        {
            if (fillColor2_ != value)
            {
                fillColor2_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    [Range(0.0f, 360.0f)]
    private float fillAngle_;
    public float fillAngle
    {
        get { return fillAngle_; }
        set
        {
            if (fillAngle_ != value)
            {
                fillAngle_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float fillOffset1_ = 0.0f;
    public float fillOffset1
    {
        get { return fillOffset1_; }
        set
        {
            if (fillOffset1_ != value)
            {
                fillOffset1_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float fillOffset2_ = 0.0f;
    public float fillOffset2
    {
        get { return fillOffset2_; }
        set
        {
            if (fillOffset2_ != value)
            {
                fillOffset2_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    private Texture fillTexture_;
    public Texture fillTexture
    {
        get { return fillTexture_; }
        set
        {
            if (fillTexture_ != value)
            {
                fillTexture_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    private Vector2 fillTextrueTiling_ = Vector2.one;
    public Vector2 fillTextrueTiling
    {
        get { return fillTextrueTiling_; }
        set
        {
            if (fillTextrueTiling_ != value)
            {
                fillTextrueTiling_ = value;
                updateFill = true;
            }
        }
    }

    [SerializeField]
    private Vector2 fillTextureOffset_ = Vector2.zero;
    public Vector2 fillTextureOffset
    {
        get { return fillTextureOffset_; }
        set
        {
            if (fillTextureOffset_ != value)
            {
                fillTextureOffset_ = value;
                updateFill = true;
            }
        }
    }


    [SerializeField]
    private Material customFillMaterial_;
    public Material customFillMaterial
    {
        get { return customFillMaterial_; }
        set
        {
            if (customFillMaterial_ != value)
            {
                customFillMaterial_ = value;
                updateFill = true;
            }
        }
    }

    private Material fillMaterial;

    //-------------------------------------------------------------------------
    // SHAPE STROKE APPEARANCE
    //-------------------------------------------------------------------------

    public enum StrokeType { Solid, MultiGradient, Custom };

    [SerializeField]
    private bool stroke_ = false;
    public bool stroke
    {
        get { return stroke_; }
        set
        {
            if (stroke_ != value)
            {
                stroke_ = value;
                updateStroke = true;
            }
        }
    }

    [SerializeField]
    private StrokeType strokeType_ = StrokeType.Solid;
    public StrokeType strokeType
    {
        get { return strokeType_; }
        set
        {
            if (strokeType_ != value)
            {
                strokeType_ = value;
                updateStroke = true;
            }
        }
    }

    [SerializeField]
    private Color strokeSolid_ = Color.black;
    public Color strokeSolid
    {
        get { return strokeSolid_; }
        set
        {
            if (strokeSolid_ != value)
            {
                strokeSolid_ = value;
                updateStroke = true;
            }
        }
    }

    [SerializeField]
    private Gradient strokeGradient_;
    public Gradient strokeGradient
    {
        get { return strokeGradient_; }
        set
        {
            if (strokeGradient_ != value)
            {
                strokeGradient_ = value;
                updateStroke = true;
            }
        }
    }

    [SerializeField]
    private float strokeWidth_ = 10;
    public float strokeWidth
    {
        get { return strokeWidth_; }
        set
        {
            if (strokeWidth_ != value)
            {
                strokeWidth_ = value;
                updateStroke = true;
            }
        }
    }

    [SerializeField]
    private Texture strokeTexture_;
    public Texture strokeTexture
    {
        get { return strokeTexture_; }
        set
        {
            if (strokeTexture_ != value)
            {
                strokeTexture_ = value;
                updateStroke = true;
            }
        }
    }

    [SerializeField]
    private Material customStrokeMaterial_;
    public Material customStrokeMaterial
    {
        get { return customStrokeMaterial_; }
        set
        {
            if (customStrokeMaterial_ != value)
            {
                customStrokeMaterial_ = value;
                updateStroke = true;
            }
        }
    }

    private Material strokeMaterial;

    //-------------------------------------------------------------------------
    // SHAPE GEOMETRY
    //-------------------------------------------------------------------------

    [SerializeField]
    private Vector2[] shapeAnchors_ = new Vector2[4] { new Vector2(100, -100), new Vector2(100, 100), new Vector2(-100, 100), new Vector2(-100, -100) };
    public Vector2[] shapeAnchors
    {
        get { return shapeAnchors_; }
        set
        {
            if (!shapeAnchors_.SequenceEqual(value))
            {
                shapeAnchors_ = value;
                updateGeometry = true;
            }
        }
    }


    [SerializeField]
    private float[] shapeRadii_ = new float[4] { 0f, 0f, 0f, 0f };
    public float[] shapeRadii
    {
        get { return shapeRadii_; }
        set
        {
            if (!shapeRadii_.SequenceEqual(value))
            {
                shapeRadii_ = value;
                updateGeometry = true;
            }
        }
    }

    [SerializeField]
    private int[] radiiSmoothness_ = new int[4] { 50, 50, 50, 50 };
    public int[] radiiSmoothness
    {
        get { return radiiSmoothness_; }
        set
        {
            if (!radiiSmoothness_.SequenceEqual(value))
            {
                radiiSmoothness_ = value;
                updateGeometry = true;
            }
        }
    }

    [SerializeField]
    [Range(0.0f, 360.0f)]
    private float rotation_ = 0.0f;
    public float rotation
    {
        get { return rotation_; }
        set
        {
            if (rotation_ != value)
            {
                rotation_ = value;
                updateGeometry = true;
            }
        }
    }

    [SerializeField]
    private bool mirrorX_ = false;
    public bool mirrorX
    {
        get { return mirrorX_; }
        set
        {
            if (mirrorX_ != value)
            {
                mirrorX_ = value;
                updateGeometry = true;
            }
        }
    }

    [SerializeField]
    private bool mirrorY_ = false;
    public bool mirrorY
    {
        get { return mirrorY_; }
        set
        {
            if (mirrorY_ != value)
            {
                mirrorY_ = value;
                updateGeometry = true;
            }
        }
    }

    private int defaultSmoothness = 10;

    //-------------------------------------------------------------------------
    // SORTING LAYERS
    //-------------------------------------------------------------------------

    [SerializeField]
    [SortingLayer]
    private int sortingLayer_ = 0;
    public int sortingLayer
    {
        get { return sortingLayer_; }
        set
        {
            if (sortingLayer_ != value)
            {
                sortingLayer_ = value;
                updateSortingLayer = true;
            }
        }
    }

    [SerializeField]
    private int sortingOrder_ = 0;
    public int sortingOrder
    {
        get { return sortingOrder_; }
        set
        {
            if (sortingOrder_ != value)
            {
                sortingOrder_ = value;
                updateSortingLayer = true;
            }
        }
    }

    //-------------------------------------------------------------------------
    // COLLIDER
    //-------------------------------------------------------------------------

    public enum ColliderMode { Disabled, ToCollider, FromCollider }
    public enum SetColliderTo { Anchors, Vertices }

    [SerializeField]
    private ColliderMode colliderMode_ = ColliderMode.Disabled;
    public ColliderMode colliderMode
    {
        get { return colliderMode_; }
        set
        {
            if (colliderMode_ != value)
            {
                colliderMode_ = value;
            }
        }
    }

    [SerializeField]
    private SetColliderTo setColliderTo_ = SetColliderTo.Anchors;
    public SetColliderTo setColliderTo
    {
        get { return setColliderTo_; }
        set
        {
            if (setColliderTo_ != value)
            {
                setColliderTo_ = value;
            }
        }
    }

    [SerializeField]
    private bool showComponents_ = false;
    public bool showComponents
    {
        get { return showComponents_; }
        set
        {
            if (showComponents_ != value)
            {
                showComponents_ = value;
                ShowComponenets();
            }
        }
    }

    //-------------------------------------------------------------------------
    // UPDATE FLAGS
    //-------------------------------------------------------------------------

    private bool updateGeometry = false;
    private bool updateFill = false;
    private bool updateStroke = false;
    private bool updateSortingLayer = false;

    //-------------------------------------------------------------------------
    // COMPONENET HANDLES
    //-------------------------------------------------------------------------

    private LineRenderer lr;
    private MeshFilter mf;
    private MeshRenderer mr;
    private MaterialPropertyBlock mpb_fill;
    private MaterialPropertyBlock mpb_stroke;
    private PolygonCollider2D pc2d;   

    //-------------------------------------------------------------------------
    // MONOBEHAVIOR CALLBACKS
    //-------------------------------------------------------------------------

    void Awake()
    {
        // Add required Unity components and check ranges
        ValidateComponents();
        ValidateValues();

        // Check for a PolgonCollider2D
        pc2d = GetComponent<PolygonCollider2D>();

        // Set default Materials
        if (fillMaterial == null)
            fillMaterial = Resources.Load("SR_FillLinearGradient") as Material;
        if (strokeMaterial == null)
            strokeMaterial = Resources.Load("SR_Stroke") as Material;

        // Set start sorting layers
        mr.sortingLayerID = sortingLayer_;
        mr.sortingOrder = sortingOrder_;
        mr.sortingLayerID = sortingLayer_;
        mr.sortingOrder = sortingOrder_;

        // Show/Hide required components
        ShowComponenets();
    }
    
    private void Start()
    {
        UpdateShapeAll();
    }

    void Update()
    {
        #if UNITY_EDITOR
        ShowComponenets();
        if (!EditorApplication.isPlaying)
            UpdateShapeAll(); 
        #endif

        if (colliderMode_ == ColliderMode.FromCollider)
        {
            FromCollider();
            updateGeometry = true;
        }

        if (updateGeometry)
        {
            UpdateShapeGeometry();
            updateGeometry = false;
        }
        if (updateFill)
        {
            UpdateFillAppearance();
            updateFill = false;
        }
        if (updateStroke)
        {
            UpdateStrokeAppearance();
            updateStroke = false;
        }
        if (updateSortingLayer)
        {
            UpdateSortingLayer();
            updateSortingLayer = false;
        }     

    }

    private void OnEnable()
    {
        mr.enabled = fill_;
        lr.enabled = stroke_;
    }

    private void OnDisable()
    {
        mr.enabled = false;
        lr.enabled = false;
    }

    // Called when script added or inspector element is changed (in editor only)
    public void OnValidate()
    {
        ValidateComponents();
        ValidateValues();
    }

    //-------------------------------------------------------------------------
    // SHAPERENDERER DLL IMPORTS
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
    // PUBLIC FUNCTIONS
    //-------------------------------------------------------------------------



    //-------------------------------------------------------------------------
    // PRIVATE FUNCTIONS
    //-------------------------------------------------------------------------

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
            mf.sharedMesh.MarkDynamic();
        }
        // validate MeshRenderer
        mr = GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>() as MeshRenderer;
        // validate MaterialPropertyBlock
        if (mpb_fill == null)
            mpb_fill = new MaterialPropertyBlock();
        if (mpb_stroke == null)
            mpb_stroke = new MaterialPropertyBlock();
    }

    private void ValidateValues()
    {
        if (shapeAnchors_.Length < 3)
            Array.Resize(ref shapeAnchors_, 3);
        if (shapeRadii_.Length != shapeAnchors_.Length)
        {
            Array.Resize(ref shapeRadii_, shapeAnchors_.Length);
            for (int i = 0; i < shapeAnchors_.Length; i++)
            {
                if (shapeRadii_[i] < 0.0f)
                    shapeRadii_[i] = 0.0f;
                if (radiiSmoothness_[i] < 1)
                    radiiSmoothness_[i] = 1;
            }
        }
        if (radiiSmoothness_.Length != shapeAnchors_.Length)
        {
            Array.Resize(ref radiiSmoothness_, shapeAnchors_.Length);
            for (int i = 0; i < radiiSmoothness_.Length; i++)
            {
                if (radiiSmoothness_[i] == 0)
                    radiiSmoothness_[i] = defaultSmoothness;
            }
        }
        if (strokeWidth_ < 0)
            strokeWidth_ = 0;
    }

    /// <summary>
    /// Shows or hides required components in the inspector
    /// </summary>
    private void ShowComponenets()
    {
        if (showComponents_)
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
        if (fill_)
            mr.enabled = true;
        else
            mr.enabled = false;
        if (stroke_)
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
        UpdateSortingLayer();
        UpdateEnabled();
    }

    /// <summary>
    /// Updates the shape fill and stroke geometry.
    /// </summary>
    public void UpdateShapeGeometry()
    {
        ValidateValues();

        // rotate
        Vector2[] shapeAnchorsTransform = new Vector2[shapeAnchors_.Length];
        Array.Copy(shapeAnchors_, shapeAnchorsTransform, shapeAnchors.Length);

        if (rotation_ != 0)
            shapeAnchorsTransform = RotateVertices(shapeAnchorsTransform, rotation_);

        // mirror
        if (mirrorX_)
            for (int i = 0; i < shapeAnchorsTransform.Length; ++i)
                shapeAnchorsTransform[i].x *= -1;

        if (mirrorY_)
            for (int i = 0; i < shapeAnchorsTransform.Length; ++i)
                shapeAnchorsTransform[i].y *= -1;

        // calculate sizes
        int anchorsSize = shapeAnchorsTransform.Length;
        int verticesSize = 0;
        for (int i = 0; i < anchorsSize; i++)
        {
            if (radiiSmoothness_[i] == 0 || radiiSmoothness_[i] == 1 || shapeRadii_[i] <= 0.0)
                verticesSize += 1;
            else
                verticesSize += radiiSmoothness_[i];
        }
        int indicesSize = (verticesSize - 2) * 3;

        // unpack Unity types containers
        float[] anchorsX = new float[shapeAnchorsTransform.Length];
        float[] anchorsY = new float[shapeAnchorsTransform.Length];
        for (int i = 0; i < anchorsSize; ++i)
        {
            anchorsX[i] = shapeAnchorsTransform[i].x;
            anchorsY[i] = shapeAnchorsTransform[i].y;
        }
        float[] verticesX = new float[verticesSize];
        float[] verticesY = new float[verticesSize];
        float[] u = new float[verticesSize];
        float[] v = new float[verticesSize];
        int[] indices = new int[indicesSize];



        // call DLL
        int result = 0;
        result = ComputeShape1(anchorsX, anchorsY, shapeRadii_, radiiSmoothness_, anchorsSize, verticesX, verticesY, verticesSize, indices, indicesSize, u, v);

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
            ToCollider(shapeAnchorsTransform, vertices);
        }
    }

    /// <summary>
    /// Updates the shape fill geometry.
    /// </summary>
    public void UpdateFillGeometry(Vector3[] vertices, int[] indices, Vector2[] uv)
    {
        if (fill_)
        {
            Mesh mesh = mf.sharedMesh;
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.uv = uv;
        }
    }

    /// <summary>
    /// Updates the shape stroke geometry.
    /// </summary>
    public void UpdateStrokeGeometry(Vector3[] vertices)
    {
        if (stroke_)
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
        if (fill_)
        {
            if (fillType_ != FillType.Custom)
            {
                if (fillTexture_ != null)
                {
                    mpb_fill.SetTexture("_MainTex", fillTexture_);
                    mpb_fill.SetVector("_TileOff", new Vector4(fillTextrueTiling_.x, fillTextrueTiling_.y, fillTextureOffset_.x, fillTextureOffset_.y));
                }
                else
                    mpb_fill.Clear();

                if (fillType_ == FillType.Solid)
                {
                    fillMaterial = Resources.Load("SR_FillLinearGradient") as Material;
                    mpb_fill.SetColor("_Color1", fillColor1_);
                    mpb_fill.SetColor("_Color2", fillColor1_);
                }
                else if (fillType_ == FillType.LinearGradient)
                {
                    fillMaterial = Resources.Load("SR_FillLinearGradient") as Material;
                    mpb_fill.SetColor("_Color1", fillColor1_);
                    mpb_fill.SetColor("_Color2", fillColor2_);
                }
                else if (fillType_ == FillType.RadialGradient)
                {
                    fillMaterial = Resources.Load("SR_FillRadialGradient") as Material;
                    mpb_fill.SetColor("_Color1", fillColor1_);
                    mpb_fill.SetColor("_Color2", fillColor2_);

                }
                mpb_fill.SetFloat("_Angle", fillAngle_);
                mpb_fill.SetFloat("_Slider1", fillOffset1_);
                mpb_fill.SetFloat("_Slider2", fillOffset2_);
                mr.SetPropertyBlock(mpb_fill);
                mr.sharedMaterial = fillMaterial;
            }
            else
            {
                mpb_fill.Clear();
                mr.SetPropertyBlock(null);
                mr.sharedMaterial = customFillMaterial_;
            }
        }
        else
        {
            mr.sharedMaterial = null;
        }
    }

    /// <summary>
    /// Updates the shape stroke appearance.
    /// </summary>
    public void UpdateStrokeAppearance()
    {
        if (stroke_)
        {
            if (strokeType_ != StrokeType.Custom)
            {
                if (strokeTexture_ != null)
                {
                    mpb_stroke.SetTexture("_MainTex", strokeTexture_);
                }
                else
                    mpb_stroke.Clear();
                
                if (strokeType_ == StrokeType.Solid)
                {
                    GradientColorKey[] gck = new GradientColorKey[2];
                    GradientAlphaKey[] gak = new GradientAlphaKey[2];

                    gck[0].color = strokeSolid_; gck[0].time = 0.0f;
                    gck[1].color = strokeSolid_; gck[1].time = 1.0f;
                    gak[0].alpha = strokeSolid_.a; gak[0].time = 0.0f;
                    gak[1].alpha = strokeSolid_.a; gak[1].time = 1.0f;

                    Gradient g = new Gradient();
                    g.SetKeys(gck, gak);

                    lr.colorGradient = g;
                } 
                else if (strokeType_ == StrokeType.MultiGradient)
                {
                    lr.colorGradient = strokeGradient_;
                }
                lr.SetPropertyBlock(mpb_stroke);
                strokeMaterial = Resources.Load("SR_Stroke") as Material;
                lr.sharedMaterial = strokeMaterial;
            }
            else
            {
                mpb_stroke.Clear();
                lr.SetPropertyBlock(null);
                lr.sharedMaterial = customStrokeMaterial_;
            }
            lr.startWidth = strokeWidth_;
            lr.endWidth = strokeWidth_;
        }
    }

    public void ToCollider(Vector2[] anchors, Vector3[] vertices)
    {
        if (colliderMode_ == ColliderMode.ToCollider)
        {
            if (pc2d == null)
                pc2d = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
            if (setColliderTo_ == SetColliderTo.Anchors)
                pc2d.points = anchors;
            if (setColliderTo_ == SetColliderTo.Vertices)
            {
                pc2d.points = System.Array.ConvertAll<Vector3, Vector2>(vertices, Vector3toVector2);
            }
        }
    }

    /// <summary>
    /// Updates the attached PolygonCollider2D points if updateCollider is true. If no PC2D is attached, a new instance is created.
    /// </summary>
    public void FromCollider()
    {
        if (colliderMode_ == ColliderMode.FromCollider)
        {
            if (pc2d == null)
            {
                pc2d = gameObject.AddComponent<PolygonCollider2D>() as PolygonCollider2D;
            }
            else
            {
                shapeAnchors_ = pc2d.points;
                for (int i = 0; i < pc2d.points.Length; i++)
                {
                    shapeAnchors_[i] += pc2d.offset;
                }
            }
        }
    }

    private void UpdateSortingLayer()
    {
        mr.sortingLayerID = sortingLayer_;
        mr.sortingOrder = sortingOrder_;
        lr.sortingLayerID = sortingLayer_;
        lr.sortingOrder = sortingOrder_ + 1;
    }

    //-------------------------------------------------------------------------
    // PUBLIC STATIC FUNCTIONS
    //-------------------------------------------------------------------------

    public static Vector2 Vector3toVector2(Vector3 V3)
    {
        return new Vector2(V3.x, V3.y);
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

}
