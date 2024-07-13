//
//  CObject.cpp
//  rt
//
//  Created by Radoslaw Mantiuk on 22/01/2023.
//

#include "rt.h"
#include "CObject.hpp"


/// \fn intersect(CRay ray)
/// \brief Computes intersection between ray and sphere.
/// \param ray Ray parameters.
/// \return Distance from camera position to the closest intersection point, or negative value.
///
float CSphere::intersect(const CRay& ray) {
    float t = -1.0f;

    glm::vec3 v = ray.pos - this->pos;
    float A = glm::dot(ray.dir, ray.dir);
    float B = 2.0f * glm::dot(v, ray.dir);
    float C = glm::dot(v, v) - (this->r * this->r);
    float delta = (B * B) - (4.0f * A * C);

    if (delta > 0.0f) {
        float sqrtDelta = sqrt(delta);
        float t2 = (-B + sqrtDelta) / (2.0f * A);
        float t3 = (-B - sqrtDelta) / (2.0f * A);

        if (t2 > 0.0f && t2 < t3) {
            t = t2;
        } else if (t3 > 0.0f && t3 <= t2) {
            t = t3;
        }
    }

    return t;
}



/// Normal vector to the sphere surface
/// \fn normal(glm::vec3 hit_pos)
/// \brief Surface normal vector at the intersection point.
/// \param hit_pos Intersection point.
/// \return Normal vector parameters.
///
glm::vec3 CSphere::normal(const glm::vec3& hit_pos) {
    glm::vec3 n = {0,0,0};
    n = glm::normalize(hit_pos - this->pos);
    return n;
}

/// Computes texture mapping coordinates (u,v).
/// \param normal_vec Normalized normal vector at intersection point.
/// \return (u,v) texture coordinates in <0,1> range.
glm::vec2 CSphere::textureMapping(const glm::vec3& normal_vec) {
    glm::vec2 uv = {0,0};
    uv.x = 0.5 + glm::asin(normal_vec.x)/M_PI;
    uv.y = 0.5f - glm::asin(normal_vec.y)/M_PI;

    return uv;
}



/// \fn intersect(CRay ray)
/// \brief Computes intersection between triangle and sphere.
/// \param ray Ray parameters.
/// \return Distance from camera position to the closest intersection point, or negative value.
///
float CTriangle::intersect(const CRay& ray) {
    float t = -1.0f;
    glm::vec3 intersectionPoint;

    if (glm::intersectRayTriangle(ray.pos, ray.dir, v1, v2, v0, intersectionPoint)) {
        return glm::length(intersectionPoint);
    }

    return t;
}



/// \fn normal(glm::vec3 hit_pos)
/// \brief Surface normal vector at the intersection point.
/// \param hit_pos Intersection point (not used for triangle).
/// \return Normal vector parameters.
///
glm::vec3 CTriangle::normal(const glm::vec3& hit_pos) {
    glm::vec3 n;
   glm::vec3 u = this->v1 - this->v0;
   glm::vec3 v = this->v2 - this->v0;
   return glm::normalize(glm::cross(u,v));

    return n;
}



