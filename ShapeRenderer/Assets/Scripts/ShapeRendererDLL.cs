using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ShapeRendererDLL {

    //-------------------------------------------------------------------------
    // DLL API IMPORT
    //-------------------------------------------------------------------------

    [DllImport("ShapeRenderer", EntryPoint = "triangulate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Triangulate(float[] verticesX, float[] verticesY, int verticesSize, int[] indices, int indicesSize);

    [DllImport("ShapeRenderer", EntryPoint = "generate_vertices", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GenerateVertices(float[] anchorsX, float[] anchorsY, float[] radii, int[] N, int anchorsSize, float[] verticesX, float[] verticesY, int verticesSize);

    [DllImport("ShapeRenderer", EntryPoint = "compute_shape", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ComputeShape(float[] anchorsX, float[] anchorsY, float[] radii, int[] N, int anchorsSize, 
                                         float[] verticesX, float[] verticesY, int verticesSize, 
                                         int[] indices, int indicesSize, float[] u, float[] v);
}

