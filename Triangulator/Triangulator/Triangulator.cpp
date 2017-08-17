#include "Triangulator.h"
#include <limits>

float area(float* points_x, float* points_y, int size) {
    float area = 0.0f;
    for (int p = size - 1, q = 0; q < size; p = q++) {
        area += points_x[p] * points_y[q] - points_x[q] * points_y[p];
    }
    return 0.5f * area;
}

int inside_triangle(float ax, float ay, float bx, float by, float cx, float cy, float px, float py) {
    float apx, apy, bpx, bpy, cpx, cpy;
    float abx, aby, bcx, bcy, cax, cay;
    float cCROSSap, bCROSScp, aCROSSbp;

    bcx = cx - bx; bcy = cy - by;
    cax = ax - cx; cay = ay - cy;
    abx = bx - ax; aby = by - ay;

    apx = px - ax; apy = py - ay;
    bpx = px - bx; bpy = py - by;
    cpx = px - cx; cpy = py - cy;

    aCROSSbp = bcx * bpy - bcy * bpx;
    cCROSSap = abx * apy - aby * apx;
    bCROSScp = cax * cpy - cay * cpx;

    if (((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f)))
        return 1;
    else
        return 0;
}

int snip(float* points_x, float* points_y, int size, int u, int v, int w, int n, int* V) {
    int p;
    float ax = points_x[V[u]], ay = points_y[V[u]];
    float bx = points_x[V[v]], by = points_y[V[v]];
    float cx = points_x[V[w]], cy = points_y[V[w]];
    if (FLT_EPSILON > (((bx - ax) * (cy - ay)) - ((by - ay) * (cx - ax)))) {
        return 0;
    }
    for (p = 0; p < n; p++) {
        if ((p == u) || (p == v) || (p == w)) {
            continue;
        }
        float px = points_x[V[p]], py = points_y[V[p]];
        if (inside_triangle(ax, ay, bx, by, cx, cy, px, py) == 1) {
            return 0;
        }
    }
    return 1;
}

int* reverse(int* int_array, int size)
{
    int *p1 = int_array;
    int *p2 = int_array + size - 1;
    while (p1 < p2) {
        int tmp = *p1;
        *p1++ = *p2;
        *p2-- = tmp;
    }
    return int_array;
}

int triangulate(float* points_x, float* points_y, int n, int* indices, int indices_size) {

    if (n < 3)
        return -1;

    int* V = new int[n];
    if (area(points_x, points_y, n) > 0) {
        for (int v = 0; v < n; v++)
            V[v] = v;
    }
    else {
        for (int v = 0; v < n; v++)
            V[v] = (n - 1) - v;
    }

    int nv = n;
    int count = 2 * nv;
    int i = 0;
    for (int m = 0, v = nv - 1; nv > 2;)
    {
        if ((count--) <= 0) {
            delete V;
            return -2;
        }

        int u = v;
        if (nv <= u)
            u = 0;
        v = u + 1;
        if (nv <= v)
            v = 0;
        int w = v + 1;
        if (nv <= w)
            w = 0;

        if (snip(points_x, points_y, n, u, v, w, nv, V) == 1) {
            int a, b, c, s, t;
            a = V[u];
            b = V[v];
            c = V[w];
            indices[i] = a;
            i += 1;
            indices[i] = b;
            i += 1;
            indices[i] = c;
            i += 1;
            m++;
            for (s = v, t = v + 1; t < nv; s++, t++)
                V[s] = V[t];
            nv--;
            count = 2 * nv;
        }
    }

    reverse(indices, indices_size);
    delete V;
    return 1;
}

