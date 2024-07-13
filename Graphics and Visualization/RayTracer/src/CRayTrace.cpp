//
//  CRayTrace.cpp
//  rt
//
//  Created by Radoslaw Mantiuk on 22/01/2023.
//

#include "rt.h"


/// \fn compPrimaryRayMatrix(CCamera cam, glm::mat3& m)
/// \brief Computation of the projection matrix.
/// \param cam Camera parameters.
/// \param m Output projection matrix.
///
bool CRayTrace::compPrimaryRayMatrix(const CCamera& cam, glm::mat3& m) {
    glm::vec3 look = cam.lookp - cam.eyep;
    glm::vec3 u = glm::normalize(glm::cross(cam.up, look));
    glm::vec3 v = glm::cross(u, look);
    v = glm::normalize(v);
    float rad = (float)cam.fov * (3.14f / 180.0f);
    float tang = tan(rad / 2.0f);
    // float asp = (float)cam.width / (float)cam.height;
    glm::vec3 o = glm::normalize(look) * ((float)cam.width / (2.0f * tang)) - ((((float)cam.width / 2.0f) * u) + (v * ((float)cam.height / 2.0f)));
    m = {u, v, o};
    return true;
}


/// \fn rayTrace(CScene scene, CRay& ray, COutput& out)
/// \brief Traces single ray.
/// \param scene Object with all scene components including a camera.
/// \param ray Ray parameter (primary and secondary rays).
/// \param out Object with output color and parameters used in recursion.
///
bool CRayTrace::rayTrace(const CScene& scene, CRay& ray, COutput& out) {
    CObject* hitObject;
    float tMin = FLT_MAX;
    float EPSILON = 0.0001f;
    bool isIntersection = false;

    // float MAX_RAY_TREE = 1.0f;
     //float MIN_RAY_ENERGY = 0.01f;

    for (auto& obj : scene.objectList) {
        float t = obj->intersect(ray);
        if (t > EPSILON && t < tMin) {
            tMin = t;
            hitObject = obj;
            isIntersection = true;
        }
    }

    if (!isIntersection) {
        return false;
    }

    if(isIntersection==true) {
        out.col = {0, 0.5, 0};
    }
    else
        out.col = {0,0,0};

    glm::vec3 intersectionPoint = ray.pos + tMin * ray.dir;

    for (auto light : scene.lightList) {
        out.col += light.color * hitObject->matAmbient;
        glm::vec3 normal = hitObject->normal(intersectionPoint);
        glm::vec3 lightDir = glm::normalize(light.pos - intersectionPoint);

        CRay shadowRay;
        shadowRay.pos = intersectionPoint;
        shadowRay.dir = lightDir;

        bool inShadow = false;
        float shadowTMin = FLT_MAX;

        for (const auto& obj : scene.objectList) {
            float t = obj->intersect(shadowRay);
            if (t > EPSILON && t < shadowTMin) {
                shadowTMin = t;
                inShadow = true;
                break;
            }
        }

        if (inShadow) {
            continue;
        }

        float cosAngle = glm::dot(normal, lightDir);
        if (hitObject->isTexture) {
            glm::vec2 uv = hitObject->textureMapping(normal);
            glm::vec3 texColor = CImage::getTexel(hitObject->texture, uv.x, uv.y);
            out.col *= texColor;
        }

        if (cosAngle > 0.001f) {
            out.col += out.energy * light.color * hitObject->matDiffuse * cosAngle;
            glm::vec3 viewDir = -ray.dir;
            glm::vec3 halfVector = glm::normalize(lightDir + viewDir);
            float cosBeta = glm::dot(normal, halfVector);

            if (cosBeta > 0.001f) {
                out.col += out.energy * light.color * hitObject->matSpecular * powf(cosBeta, hitObject->matShininess);
            }
        }

        if (hitObject->reflectance > 0.0f && out.tree < MAX_RAY_TREE && out.energy > MIN_RAY_ENERGY) {
            out.tree++;
            out.energy *= hitObject->reflectance;
            CRay reflectedRay = this->reflectedRay(ray, normal, intersectionPoint);
            rayTrace(scene, reflectedRay, out);
        }
    }
    return true;
}



    /// looks for the closest object along the ray path
    /// returns false if there are no intersection

    /// computes 3D position of the intersection point

    /// computes normal vector at intersection point

    /// for each light source defined in the scene

    /// computes if the intersection point is in the shadows

    /// computes diffuse color component

    /// computes specular color component

    /// adds texture for textured spheres

    /// computes ambient color component

    /// if the surface is reflective

    /// if out.tree >= MAX_RAY_TREE return from function

    /// computes the secondary ray parameters (reflected ray)

    /// recursion
    //rayTrace(scene, secondary_ray, out);





/// \fn reflectedRay(CRay ray, glm::vec3 n, glm::vec3 pos)
/// \brief Computes parameters of the ray reflected at the surface point with given normal vector.
/// \param ray Input ray.
/// \param n Surface normal vector.
/// \param pos Position of reflection point.
/// \return Reflected ray.
///
CRay CRayTrace::reflectedRay(const CRay& ray, const glm::vec3& n, const glm::vec3& pos) {
    CRay reflected_ray;
    reflected_ray.pos = pos;
    glm::vec3 v = ray.dir;
    glm::vec3 r = v - (2.0f * glm::dot(v, n)) * n;
    reflected_ray.dir = glm::normalize(r);
    return reflected_ray;
}

