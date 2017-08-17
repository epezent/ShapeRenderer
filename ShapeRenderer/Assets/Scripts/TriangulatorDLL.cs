using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TriangulatorDLL {

    //-------------------------------------------------------------------------
    // TRIANGULATE
    //-------------------------------------------------------------------------

    [DllImport("Triangulator", EntryPoint = "triangulate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int TriagulateDll(float[] pointsX, float[] pointsY, int size, int[] indices, int indices_size);

    //-------------------------------------------------------------------------
    // COMPUTE AREA
    //-------------------------------------------------------------------------

    [DllImport("Triangulator", EntryPoint = "area", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern float AreaDll(float[] pointsX, float[] pointsY, int size);

    public static float Area(Vector2[] points)
    {
        int size = points.Length;
        float[] pointsX = new float[size];
        float[] pointsY = new float[size];
        for (int i = 0; i < size; ++i)
        {
            pointsX[i] = points[i].x;
            pointsY[i] = points[i].y;
        }
        return AreaDll(pointsX, pointsY, size);
    }

    //-------------------------------------------------------------------------
    // INSIDE TRIANGLE
    //-------------------------------------------------------------------------

    [DllImport("Triangulator", EntryPoint = "inside_triangle", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int InsideTriangleDll(float ax, float ay, float bx, float by, float cx, float cy, float px, float py);

    public static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        if (InsideTriangleDll(A.x, A.y, B.x, B.y, C.x, C.y, P.x, P.y) == 1)
            return true;
        else
            return false;
    }

    //-------------------------------------------------------------------------
    // SNIP
    //-------------------------------------------------------------------------

    [DllImport("Triangulator", EntryPoint = "snip", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SnipDll(float[] pointsX, float[] pointsY, int size, int u, int v, int w, int n, int[] V);

    public static bool Snip(Vector2[] points, int u, int v, int w, int n, int[] V)
    {
        int size = points.Length;
        float[] pointsX = new float[size];
        float[] pointsY = new float[size];
        for (int i = 0; i < size; ++i)
        {
            pointsX[i] = points[i].x;
            pointsY[i] = points[i].y;
        }
        if (SnipDll(pointsX, pointsY, size, u, v, w, n, V) == 1)
            return true;
        else
            return false;
    }

}

