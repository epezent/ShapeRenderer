#include "ShapeRenderer.h"
#include <limits>
#include <cmath>

//-----------------------------------------------------------------------------
// DLL EXPORTS
//-----------------------------------------------------------------------------

int triangulate(float* points_x, float* points_y, int n, int* indices, int indices_size) {

    if (n < 3)
        return -1;

    std::vector<int> V(n, 0);
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
    return 1;
}

int render_shape(float* anchors_x, float* anchors_y, float* radii, int* resolutions,
    float* vertices_x, float* vertices_y, int* triangles, float* u, float* v) {
    return 0;
}

//-----------------------------------------------------------------------------
// INTERAL FUNCTIONS
//-----------------------------------------------------------------------------

float area(float* points_x, float* points_y, int size) {
    float area = 0.0f;
    for (int p = size - 1, q = 0; q < size; p = q++) {
        area += points_x[p] * points_y[q] - points_x[q] * points_y[p];
    }
    return 0.5f * area;
}

bool inside_triangle(const Vector2& A, const Vector2& B, const Vector2& C, const Vector2& P) {
    return ((((C.x - B.x) * (P.y - B.y) - (C.y - B.y) * (P.x - B.x)) >= 0.0f) &&
            (((B.x - A.x) * (P.y - A.y) - (B.y - A.y) * (P.x - A.x)) >= 0.0f) &&
            (((A.x - C.x) * (P.y - C.y) - (A.y - C.y) * (P.x - C.x)) >= 0.0f));
}

int snip(float* points_x, float* points_y, int size, int u, int v, int w, int n, std::vector<int>& V) {
    int p;

    Vector2 A(points_x[V[u]], points_y[V[u]]);
    Vector2 B(points_x[V[v]], points_y[V[v]]);
    Vector2 C(points_x[V[w]], points_y[V[w]]);

    if (FLT_EPSILON > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x)))) {
        return 0;
    }
    for (p = 0; p < n; p++) {
        if ((p == u) || (p == v) || (p == w)) {
            continue;
        }
        Vector2 P(points_x[V[p]], points_y[V[p]]);
        if (inside_triangle(A, B, C, P) == 1) {
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

Vector2 intersection(const Vector2& A1, const Vector2& A2, const Vector2& B1, const Vector2& B2)
{
    Vector2 intersection;

    intersection.x = ((A1.x * A2.y - A1.y * A2.x) * (B1.x - B2.x) - (A1.x - A2.x) * (B1.x * B2.y - B1.y * B2.x)) /
        ((A1.x - A2.x) * (B1.y - B2.y) - (A1.y - A2.y) * (B1.x - B2.x));

    intersection.y = ((A1.x * A2.y - A1.y * A2.x) * (B1.y - B2.y) - (A1.y - A2.y) * (B1.x * B2.y - B1.y * B2.x)) /
        ((A1.x - A2.x) * (B1.y - B2.y) - (A1.y - A2.y) * (B1.x - B2.x));

    return intersection;
}

float wrap_to_2_pi(float angle) {
    if (angle < 0)
        return angle + 2 * PI;
    return angle;
}

bool inside_line(Vector2 L1, Vector2 L2, Vector2 P)
{
    float crossproduct = (P.y - L1.y) * (L2.x - L1.x) - (P.x - L1.x) * (L2.y - L1.y);
    if (std::abs(crossproduct) > 0.1)
        return false;

    float dotproduct = (P.x - L1.x) * (L2.x - L1.x) + (P.y - L1.y) * (L2.y - L1.y);
    if (dotproduct < 0.0f)
        return false;
    
    float squarelength = (L2.x - L1.x) * (L2.x - L1.x) + (L2.y - L1.y) * (L2.y - L1.y);
    if (dotproduct > squarelength)
        return false;

    return true;
}

void linspace(float a, float b, std::vector<float>& out) {
    float delta = (b - a) / (out.size() - 1);
    out[0] = a;
    for (int i = 1; i < out.size() - 1; i++)
        out[i] = out[i - 1] + delta;
    out[out.size() - 1] = b;
}

bool round_corner(Vector2& A, Vector2& B, Vector2& C, float r, std::vector<Vector2>& out)
{
    // Find directional vectors of line segments
    Vector2 V1 = B - A;
    Vector2 V2 = B - C;

    // Check if corner radius is longer than vectors
    if (r >= V1.magnitude() || r >= V2.magnitude())
        false; // set corner equal to B

    // Find unit vectors
    Vector2 U1 = V1 / V1.magnitude();
    Vector2 U2 = V2 / V2.magnitude();

    // Find normal vectors
    Vector2 N1, N2;
    N1.x = -U1.y; N1.y = U1.x;
    N2.x = -U2.y; N2.y = U2.x;

    // Check/fix direction of normal vector
    if (Vector2::dot(N1, -V2) < 0.0f)
        N1 = -N1;
    if (Vector2::dot(N2, -V1) < 0.0f)
        N2 = -N2;

    // Find end-points of offset lines
    Vector2 O11 = A + N1 * r;
    Vector2 O10 = B + N1 * r;
    Vector2 O22 = C + N2 * r;
    Vector2 O20 = B + N2 * r;

    // Find intersection point of offset lines
    Vector2 I = intersection(O11, O10, O22, O20);

    // Find tangent points
    Vector2 T1 = I + -N1 * r;
    Vector2 T2 = I + -N2 * r;

    // Check if tangent points are on line segments
    //if (!IamondMath.IsPointInsideLine(a,b,t1) || !IamondMath.IsPointInsideLine(c,b,t2))
    //return new Vector2[] { b };

    float angle1 = std::atan2(T1.y - I.y, T1.x - I.x);
    float angle2 = std::atan2(T2.y - I.y, T2.x - I.x);
    std::vector<float> angles(out.size(), 0);
    if (std::abs(angle1 - angle2) < PI)
        linspace(angle1, angle2, angles);
    else
        linspace(wrap_to_2_pi(angle1), wrap_to_2_pi(angle2), angles);

    for (int i = 0; i < out.size(); i++)
    {
        Vector2 vertex;
        vertex.x = r * std::cos(angles[i]) + I.x;
        vertex.y = r * std::sin(angles[i]) + I.y;
        out[i] = vertex;
    }

    return true;
}

int generate_vertices(float* anchors_x, float* anchors_y, float* radii, int* N, int anchors_size, float* vertices_x, float* vertices_y, int vertices_size)
{
    std::vector<Vector2> vertices;
    for (int i = 0; i < anchors_size; i++) {
        if (radii[i] > 0.0f && N[i] > 1) {
            Vector2 a, b, c;
            if (i == 0) {
                a.x = anchors_x[anchors_size - 1]; a.y = anchors_y[anchors_size - 1];
                b.x = anchors_x[i];                b.y = anchors_y[i];
                c.x = anchors_x[i + 1];            c.y = anchors_y[i + 1];
            }
            else if (i == anchors_size - 1) {
                a.x = anchors_x[i - 1]; a.y = anchors_y[i - 1];
                b.x = anchors_x[i];     b.y = anchors_y[i];
                c.x = anchors_x[0];     c.y = anchors_y[0];
            }
            else {
                a.x = anchors_x[i - 1]; a.y = anchors_y[i - 1];
                b.x = anchors_x[i];     b.y = anchors_y[i];
                c.x = anchors_x[i + 1]; c.y = anchors_y[i + 1];
            }
            std::vector<Vector2> corner(N[i], Vector2());
            if (round_corner(a, b, c, radii[i], corner))
                vertices.insert(vertices.end(), corner.begin(), corner.end());
            else
                return 0;
        }
        else
            vertices.push_back(Vector2(anchors_x[i], anchors_y[i]));
    }
    for (int i = 0; i < vertices_size; i++) {
        vertices_x[i] = vertices[i].x;
        vertices_y[i] = vertices[i].y;
    }

    return 1;
}
