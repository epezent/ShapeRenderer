#pragma once
#include <cmath>

struct Vector2 {

    float x;
    float y;

    Vector2() : x(0.0f), y(0.0f) {}
    Vector2(float x, float y) : x(x), y(y) {}

    inline Vector2 & Vector2::operator - (void) { x = -x; y = -y; return *this; }

    inline const Vector2 Vector2::operator + (const Vector2& v) const { return Vector2(x + v.x, y + v.y); }
    inline const Vector2 Vector2::operator - (const Vector2& v) const { return Vector2(x - v.x, y - v.y); }
    inline const Vector2 Vector2::operator * (const Vector2& v) const { return Vector2(x * v.x, y * v.y); }
    inline const Vector2 Vector2::operator / (const Vector2& v) const { return Vector2(x / v.x, y / v.y); }

    inline const Vector2 Vector2::operator + (float v) const { return Vector2(x + v, y + v); }
    inline const Vector2 Vector2::operator - (float v) const { return Vector2(x - v, y - v); }
    inline const Vector2 Vector2::operator * (float v) const { return Vector2(x * v, y * v); }
    inline const Vector2 Vector2::operator / (float v) const { return Vector2(x / v, y / v); }

    inline float magnitude() {
        return std::sqrt(x*x + y*y);
    }

    static inline float dot(Vector2& a, Vector2& b) {
        return a.x * b.x + a.y * b.y;
    }

};