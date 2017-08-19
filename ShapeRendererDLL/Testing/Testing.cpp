#include "ShapeRenderer.h"
#include <chrono>
#include <iostream>


int main()
{
   
    float anchors_x[4] = { 100.0f, -100.0f, -100.0f, 100.0f };
    float anchors_y[4] = { 100.0f, 100.0f, -100.0f, -100.0f };

    float radii[4] = { 25.0f ,25.0f ,25.0f ,25.0f };
    int N[4] = { 50.0f ,50.0f ,50.0f ,50.0f };

    const int V = 200;
    const int I = (V - 2) * 3;

    float vertices_x[V];
    float vertices_y[V];

    int indices[I];

    float u[V];
    float v[V];   

    // BENCHMARK
    // 0.623853

    auto begin = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < 10000; i++)
        compute_shape1(anchors_x, anchors_y, radii, N, 4, vertices_x, vertices_y, V, indices, I, u, v);
    
    auto end = std::chrono::high_resolution_clock::now();

    auto elapsed = end - begin;
    std::cout << elapsed.count() / 1000000000.0 << std::endl;
    
}

