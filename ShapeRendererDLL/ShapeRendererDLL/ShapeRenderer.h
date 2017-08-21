#pragma once
#include "Vector2.h"
#include <vector>

#ifdef SHAPERENDERERDLL_EXPORTS
#	define SHAPERENDERER_API __declspec(dllexport)
#else
#	define SHAPERENDERER_API __declspec(dllimport)
#endif

//-----------------------------------------------------------------------------
// CONSTANTS
//-----------------------------------------------------------------------------

float PI = 3.1415927410125732421875;

//-----------------------------------------------------------------------------
// DLL EXPORTS
//-----------------------------------------------------------------------------


extern "C" {
    SHAPERENDERER_API int compute_shape1(float* anchors_x, float* anchors_y, float* radii, int* N, int anchors_size,
        float* vertices_x, float* vertices_y, int vertices_size, int* indices, int indices_size, float* u, float* v);
}

extern "C" {
    SHAPERENDERER_API int compute_shape2(float* anchors_x, float* anchors_y, float* radii, int* N, int anchors_size,
        float* vertices_x, float* vertices_y, int vertices_size, int* indices, int indices_size, float* u, float* v);
}


//-----------------------------------------------------------------------------
// CORE FUNCTIONS
//-----------------------------------------------------------------------------

int generate_vertices(float* anchors_x, float* anchors_y, float* radii, int* N, int anchors_size, float* vertices_x, float* vertices_y, int vertices_size);
bool generate_vertices(const std::vector<Vector2>& anchors, const std::vector<float>& radii, const std::vector<int>& N, std::vector<Vector2>& vertices_out);

int triangulate(float* vertices_x, float* vertices_y, int vertices_size, int* indices, int indices_size);
bool triangulate(const std::vector<Vector2>& vertices, std::vector<int>& indices_out);

bool snip(float* points_x, float* points_y, int size, int u, int v, int w, int n, std::vector<int>& V);
bool snip(const std::vector<Vector2>& vertices, int u, int v, int w, int n, std::vector<int>& V);

void generate_uvs(float* vertices_x, float* vertices_y, int vertices_size, float* u, float* v);
bool generate_uvs(const std::vector<Vector2>& vertices, std::vector<Vector2>& uv);

bool round_corner(Vector2& A, Vector2& B, Vector2& C, float r, std::vector<Vector2>& corner_out);

//-----------------------------------------------------------------------------
// GEOMETRIC FUNCTIONS
//-----------------------------------------------------------------------------

float poly_area1(float* points_x, float* points_y, int size);
float poly_area1(const std::vector<Vector2>& vertices);
float poly_area2(float* points_x, float* points_y, int size); 
float poly_area2(const std::vector<Vector2>& vertices); 

bool inside_triangle1(const Vector2& A, const Vector2& B, const Vector2& C, const Vector2& P);
bool inside_triangle2(const Vector2& A, const Vector2& B, const Vector2& C, const Vector2& P);

Vector2 intersection(const Vector2& A1, const Vector2& A2, const Vector2& B1, const Vector2& B2);

float wrap_to_2_pi(float angle);

bool inside_line(const Vector2& L1, const Vector2& L2, const Vector2& P);

void linspace(float a, float b, std::vector<float>& linespace_out);

//-----------------------------------------------------------------------------
// UTILITY FUNCTIONS
//-----------------------------------------------------------------------------

void pack_vector2(float* X, float* Y, std::vector<Vector2>& V_out);

void unpack_vector2(std::vector<Vector2>& V, float* X, float* Y);

template<typename T>
void unpack_vector(std::vector<T> vector, T* array) {
    for (int i = 0; i < vector.size(); i++)
        array[i] = vector[i];
}

int* reverse(int* int_array, int size);
