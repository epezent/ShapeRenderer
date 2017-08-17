#pragma once

#ifdef TRIANGULATOR_EXPORTS
#	define TRIANGULATOR_API __declspec(dllexport)
#else
#	define TRIANGULATOR_API __declspec(dllimport)
#endif

extern "C" {
    TRIANGULATOR_API int triangulate(float* points_x, float* points_y, int size, int* indices, int indices_size);
}

extern "C" {
    TRIANGULATOR_API float area(float* points_x, float* points_y, int size);
}

extern "C" {
    TRIANGULATOR_API int inside_triangle(float ax, float ay, float bx, float by, float cx, float cy, float px, float py);
}

extern "C" {
    TRIANGULATOR_API int snip(float* points_x, float* points_y, int size, int u, int v, int w, int n, int* V);
}

int* reverse(int* int_array, int size);