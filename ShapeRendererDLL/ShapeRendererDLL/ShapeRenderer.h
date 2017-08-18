#pragma once
#include "Vector2.h"
#include <vector>

#ifdef SHAPERENDERERDLL_EXPORTS
#	define SHAPERENDERER_API __declspec(dllexport)
#else
#	define TRIANGULATOR_API __declspec(dllimport)
#endif

//-----------------------------------------------------------------------------
// CONSTANTS
//-----------------------------------------------------------------------------

float PI = 3.1415927410125732421875;

//-----------------------------------------------------------------------------
// DLL EXPORTS
//-----------------------------------------------------------------------------

extern "C" {
    SHAPERENDERER_API int triangulate(float* points_x, float* points_y, int size, int* indices, int indices_size);
}

extern "C" {
    SHAPERENDERER_API int render_shape(float* anchors_x, float* anchors_y, float* radii, int* resolutions, 
        float* vertices_x, float* vertices_y, int* triangles, float* u, float* v);
}

extern "C" {
    SHAPERENDERER_API int generate_vertices(float* anchors_x, float* anchors_y, float* radii, int* N, int anchors_size, float* vertices_x, float* vertices_y, int vertices_size);
}
     
//-----------------------------------------------------------------------------
// INTERNAL FUNCTIONS
//-----------------------------------------------------------------------------

int* reverse(int* int_array, int size);
float area(float* points_x, float* points_y, int size);
bool inside_triangle(const Vector2& A, const Vector2& B, const Vector2& C, const Vector2& P);
int snip(float* points_x, float* points_y, int size, int u, int v, int w, int n, std::vector<int>& V);
Vector2 intersection(const Vector2& A1, const Vector2& A2, const Vector2& B1, const Vector2& B2);
float wrap_to_2_pi(float angle);
bool inside_line(Vector2 L1, Vector2 L2, Vector2 P);
void linspace(float a, float b, std::vector<float>& out);
bool round_corner(Vector2& A, Vector2& B, Vector2& C, float r, std::vector<Vector2>& out);