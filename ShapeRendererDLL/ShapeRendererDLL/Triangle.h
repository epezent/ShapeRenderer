#pragma once
#include "Vector2.h"

struct Triangle {

    const float Ax;
    const float Ay;
    const float Bx;
    const float By;
    const float Cx;
    const float Cy;
    const float area;

    Triangle(float Ax, float Ay, float Bx, float By, float Cx, float Cy) :
        Ax(Ax),
        Ay(Ay),
        Bx(Bx),
        By(By),
        Cx(Cx),
        Cy(Cy),
        area(area2())
    { }

    inline float area1() {
        return (((Bx - Ax) * (Cy - Ay)) - ((By - Ay) * (Cx - Ax)));
    }

    inline float area2() {
        return -By * Cx + Ay * (Cx - Bx) + Ax * (By - Cy) + Bx * Cy;
    }

    inline bool inside(Vector2 P) {
        float s = Ay * Cx - Ax * Cy + (Cy - Ay) * P.x + (Ax - Cx) * P.y;
        float t = Ax * By - Ay * Bx + (Ay - By) * P.x + (Bx - Ax) * P.y;

        if ((s < 0) != (t < 0))
            return false;

        float temp_area = area;
        if (temp_area < 0.0) {
            s = -s;
            t = -t;
            temp_area = -temp_area;
        } 
        else
            return s > 0 && t > 0 && (s + t) <= temp_area;
    }

};