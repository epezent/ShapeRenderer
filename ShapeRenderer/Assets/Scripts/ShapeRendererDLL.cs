using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ShapeRendererDLL {

    //-------------------------------------------------------------------------
    // DLL API IMPORT
    //-------------------------------------------------------------------------

    [DllImport("ShapeRenderer", EntryPoint = "triangulate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Triangulate(float[] pointsX, float[] pointsY, int size, int[] indices, int indices_size);

    [DllImport("ShapeRenderer", EntryPoint = "generate_vertices", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GenerateVertices(float[] anchors_x, float[] anchors_y, float[] radii, int[] N, int anchors_size, float[] vertices_x, float[] vertices_y, int vertices_size);
}

