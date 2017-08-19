#include "ShapeRenderer.h"
#include <limits>
#include <cmath>
#include <algorithm>

//-----------------------------------------------------------------------------
// DLL EXPORTS
//-----------------------------------------------------------------------------

int compute_shape1(float* anchors_x, float* anchors_y, float* radii, int* N, int anchors_size,
    float* vertices_x, float* vertices_y, int vertices_size, int* indices, int indices_size, float* u, float* v) {
  
    if (generate_vertices(anchors_x, anchors_y, radii, N, anchors_size, vertices_x, vertices_y, vertices_size) <= 0)
        return 0;

    if (triangulate(vertices_x, vertices_y, vertices_size, indices, indices_size) <= 0)
        return 0;

    generate_uvs(vertices_x, vertices_y, vertices_size, u, v);

    return 1;
}

int compute_shape2(float* anchors_x_in, float* anchors_y_in, float* radii_in, int* N_in, int anchors_size,
    float* vertices_x_out, float* vertices_y_out, int vertices_size, int* indices_out, int indices_size, float* u_out, float* v_out) {

    std::vector<Vector2> anchors(anchors_size);
    pack_vector2(anchors_x_in, anchors_y_in, anchors);

    std::vector<float> radii(radii_in, radii_in + anchors_size);

    std::vector<int> N(N_in, N_in + anchors_size);

    std::vector<Vector2> vertices;
    vertices.reserve(vertices_size);

    std::vector<int> indices(indices_out, indices_out + indices_size);

    std::vector<Vector2> uv(vertices_size);
    pack_vector2(u_out, v_out, uv);

    if (!generate_vertices(anchors, radii, N, vertices))
        return 0;

    if (!triangulate(vertices, indices))
        return 0;

    if (!generate_uvs(vertices, uv))
        return 0;

    unpack_vector2(vertices, vertices_x_out, vertices_y_out);
    unpack_vector(indices, indices_out);
    unpack_vector2(uv, u_out, v_out);

    return 1;
}

//-----------------------------------------------------------------------------
// CORE FUNCTIONS
//-----------------------------------------------------------------------------

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

bool generate_vertices(const std::vector<Vector2>& anchors, const std::vector<float>& radii, const std::vector<int>& N, std::vector<Vector2>& vertices_out)
{
    int anchors_size = anchors.size();
    for (int i = 0; i < anchors_size; i++) {
        if (radii[i] > 0.0f && N[i] > 1) {
            Vector2 a, b, c;
            if (i == 0) {
                a.x = anchors[anchors_size - 1].x; a.y = anchors[anchors_size - 1].y;
                b.x = anchors[i].x;                b.y = anchors[i].y;
                c.x = anchors[i + 1].x;            c.y = anchors[i + 1].y;
            }
            else if (i == anchors_size - 1) {
                a.x = anchors[i - 1].x; a.y = anchors[i - 1].y;
                b.x = anchors[i].x;     b.y = anchors[i].y;
                c.x = anchors[0].x;     c.y = anchors[0].y;
            }
            else {
                a.x = anchors[i - 1].x; a.y = anchors[i - 1].y;
                b.x = anchors[i].x;     b.y = anchors[i].y;
                c.x = anchors[i + 1].x; c.y = anchors[i + 1].y;
            }
            std::vector<Vector2> corner(N[i], Vector2());
            if (round_corner(a, b, c, radii[i], corner))
                vertices_out.insert(vertices_out.end(), corner.begin(), corner.end());
            else
                return false;
        }
        else
            vertices_out.push_back(Vector2(anchors[i].x, anchors[i].y));
    }

    return true;
}

int triangulate(float* vertices_x, float* vertices_y, int vertices_size, int* indices, int indices_size) {

    if (vertices_size < 3)
        return -1;

    std::vector<int> V(vertices_size, 0);
    if (poly_area2(vertices_x, vertices_y, vertices_size) > 0) {
        for (int v = 0; v < vertices_size; v++)
            V[v] = v;
    }
    else {
        for (int v = 0; v < vertices_size; v++)
            V[v] = (vertices_size - 1) - v;
    }

    int nv = vertices_size;
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

        if (snip(vertices_x, vertices_y, vertices_size, u, v, w, nv, V)) {
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

bool triangulate(const std::vector<Vector2>& vertices, std::vector<int>& indices_out) {

    int vertices_size = vertices.size();

    if (vertices_size < 3)
        return false;

    std::vector<int> V(vertices_size, 0);
    if (poly_area2(vertices) > 0) {
        for (int v = 0; v < vertices_size; v++)
            V[v] = v;
    }
    else {
        for (int v = 0; v < vertices_size; v++)
            V[v] = (vertices_size - 1) - v;
    }

    int nv = vertices_size;
    int count = 2 * nv;
    int i = 0;
    for (int m = 0, v = nv - 1; nv > 2;)
    {
        if ((count--) <= 0) {
            return false;
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

        if (snip(vertices, u, v, w, nv, V)) {
            int a, b, c, s, t;
            a = V[u];
            b = V[v];
            c = V[w];
            indices_out[i] = a;
            i += 1;
            indices_out[i] = b;
            i += 1;
            indices_out[i] = c;
            i += 1;
            m++;
            for (s = v, t = v + 1; t < nv; s++, t++)
                V[s] = V[t];
            nv--;
            count = 2 * nv;
        }
    }

    std::reverse(indices_out.begin(), indices_out.end());
    return true;
}

bool snip(float* points_x, float* points_y, int size, int u, int v, int w, int n, std::vector<int>& V) {
    int p;

    Vector2 A(points_x[V[u]], points_y[V[u]]);
    Vector2 B(points_x[V[v]], points_y[V[v]]);
    Vector2 C(points_x[V[w]], points_y[V[w]]);
    Vector2 P;

    if (FLT_EPSILON > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x)))) {
        return false;
    }
    for (p = 0; p < n; p++) {
        if ((p == u) || (p == v) || (p == w)) {
            continue;
        }
        P.x = points_x[V[p]]; P.y = points_y[V[p]];
        if (inside_triangle2(A, B, C, P)) {
            return false;
        }
    }
    return true;
}

bool snip(const std::vector<Vector2>& vertices, int u, int v, int w, int n, std::vector<int>& V) {
    int p;

    Vector2 A(vertices[V[u]].x, vertices[V[u]].y);
    Vector2 B(vertices[V[v]].x, vertices[V[v]].y);
    Vector2 C(vertices[V[w]].x, vertices[V[w]].y);

    if (FLT_EPSILON > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x)))) {
        return 0;
    }
    for (p = 0; p < n; p++) {
        if ((p == u) || (p == v) || (p == w)) {
            continue;
        }
        Vector2 P(vertices[V[p]].x, vertices[V[p]].y);
        if (inside_triangle2(A, B, C, P)) {
            return false;
        }
    }
    return true;
}



void generate_uvs(float* vertices_x, float* vertices_y, int vertices_size, float* u, float* v) {
    float minX = std::numeric_limits<float>::infinity();
    float maxX = -std::numeric_limits<float>::infinity();
    float minY = std::numeric_limits<float>::infinity();
    float maxY = -std::numeric_limits<float>::infinity();
    for (int i = 0; i < vertices_size; i++) {
        minX = std::min(minX, vertices_x[i]);
        maxX = std::max(maxX, vertices_x[i]);
        minY = std::min(minY, vertices_y[i]);
        maxY = std::max(maxY, vertices_y[i]);

    }
    float denX = 1.0f / (maxX - minX);
    float denY = 1.0f / (maxY - minY);
    for (int i = 0; i < vertices_size; i++) {
        u[i] = (vertices_x[i] - minX) * denX;
        v[i] = (vertices_y[i] - minY) * denY;
    }
}

bool generate_uvs(const std::vector<Vector2>& vertices, std::vector<Vector2>& uv_out) {
    float minX = std::numeric_limits<float>::infinity();
    float maxX = -std::numeric_limits<float>::infinity();
    float minY = std::numeric_limits<float>::infinity();
    float maxY = -std::numeric_limits<float>::infinity();
    for (int i = 0; i < vertices.size(); i++) {
        minX = std::min(minX, vertices[i].x);
        maxX = std::max(maxX, vertices[i].x);
        minY = std::min(minY, vertices[i].y);
        maxY = std::max(maxY, vertices[i].y);

    }
    float denX = 1.0f / (maxX - minX);
    float denY = 1.0f / (maxY - minY);
    for (int i = 0; i < vertices.size(); i++) {
        uv_out[i].x = (vertices[i].x - minX) * denX;
        uv_out[i].y = (vertices[i].y - minY) * denY;
    }
    return true;
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

//-----------------------------------------------------------------------------
// GEOMETRIC FUNCTIONS
//-----------------------------------------------------------------------------

float poly_area1(float* points_x, float* points_y, int size) {
    float area = 0.0f;
    for (int p = size - 1, q = 0; q < size; p = q++) {
        area += points_x[p] * points_y[q] - points_x[q] * points_y[p];
    }
    return 0.5f * area;
}

float poly_area1(const std::vector<Vector2>& vertices) {
    float area = 0.0f;
    for (int p = vertices.size() - 1, q = 0; q < vertices.size(); p = q++) {
        area += vertices[p].x * vertices[q].y - vertices[q].x * vertices[p].y;
    }
    return 0.5f * area;
}

float poly_area2(float* points_x, float* points_y, int size) {
    float area = 0.0f;
    for (size_t i = 1; i < size - 1; ++i)
        area += points_x[i] * (points_y[i + 1] - points_y[i - 1]);
    area += points_x[size - 1] * (points_y[0] - points_y[size - 2]);
    area += points_x[0] * (points_y[1] - points_y[size - 1]);
    return  area * 0.5f;
}

float poly_area2(const std::vector<Vector2>& vertices) {
    float area = 0.0f;
    int size = vertices.size();
    for (int i = 1; i < size - 1; ++i)
        area += vertices[i].x * (vertices[i + 1].y - vertices[i - 1].y);
    area += vertices[size - 1].x * (vertices[0].y - vertices[size - 2].y);
    area += vertices[0].x * (vertices[1].y - vertices[size - 1].y);
    return  area * 0.5f;
}

bool inside_triangle1(const Vector2& A, const Vector2& B, const Vector2& C, const Vector2& P) {
    return ((((C.x - B.x) * (P.y - B.y) - (C.y - B.y) * (P.x - B.x)) >= 0.0f) &&
            (((B.x - A.x) * (P.y - A.y) - (B.y - A.y) * (P.x - A.x)) >= 0.0f) &&
            (((A.x - C.x) * (P.y - C.y) - (A.y - C.y) * (P.x - C.x)) >= 0.0f));
}

bool inside_triangle2(const Vector2& A, const Vector2& B, const Vector2& C, const Vector2& P) {
    float s = A.y * C.x - A.x * C.y + (C.y - A.y) * P.x + (A.x - C.x) * P.y;
    float t = A.x * B.y - A.y * B.x + (A.y - B.y) * P.x + (B.x - A.x) * P.y;

    if ((s < 0) != (t < 0))
        return false;

    float area = -B.y * C.x + A.y * (C.x - B.x) + A.x * (B.y - C.y) + B.x * C.y;
    if (area < 0.0) {
        s = -s;
        t = -t;
        area = -area;
    }
    return s > 0 && t > 0 && (s + t) <= area;
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

bool inside_line(Vector2& L1, Vector2& L2, Vector2& P)
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

void linspace(float a, float b, std::vector<float>& linespace_out) {
    float delta = (b - a) / (linespace_out.size() - 1);
    linespace_out[0] = a;
    for (int i = 1; i < linespace_out.size() - 1; i++)
        linespace_out[i] = linespace_out[i - 1] + delta;
    linespace_out[linespace_out.size() - 1] = b;
}

//-----------------------------------------------------------------------------
// UTILITY FUNCTIONS
//-----------------------------------------------------------------------------

void pack_vector2(float* X, float* Y, std::vector<Vector2>& V) {
    for (int i = 0; i < V.size(); i++) {
        V[i].x = X[i];
        V[i].y = Y[i];
    }
}

void unpack_vector2(std::vector<Vector2>& V, float* X, float* Y) {
    for (int i = 0; i < V.size(); i++) {
        X[i] = V[i].x;
        Y[i] = V[i].y;
    }
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