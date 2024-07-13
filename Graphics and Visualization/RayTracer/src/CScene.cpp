//
//  CScene.cpp
//  rt
//
//  Created by Radoslaw Mantiuk on 22/01/2023.
//

#include "rt.h"

#include<fstream>

/// \fn create(void)
/// \brief Adds components to the scene.
///
void CScene::create() {

    lightList.clear();
    objectList.clear();

//Zad14
    cam.eyep = {0, 0, 10};
    cam.lookp = {0, 0, 0};
    cam.up = {0, 1, 0};
    cam.fov = 50;
    cam.width = 500;
    cam.height = 400;
//Zad16
/*    cam.eyep = {0, -4, 10};
    cam.lookp = {0, 0, 0};
    cam.up = {0, 1, 0};
    cam.fov = 50;
    cam.width = 500;
    cam.height = 400;*/
 /*   CSphere* sfera_1 = new CSphere({-1, 0, 3}, 0.4);
    sfera_1->matAmbient = glm::vec3{0, 0.1, 0};
    sfera_1->matDiffuse = glm::vec3{0, 0.6, 0};
    sfera_1->matSpecular = glm::vec3{0.7, 0.7, 0.7};
    sfera_1->matShininess = 30.0f;
    objectList.push_back(sfera_1);*/

    /*CSphere* sfera_1 = new CSphere({-2.5, 1.3, -3}, 1);
    objectList.push_back(sfera_1);*/

/*    CSphere* sfera_2 = new CSphere({0, 0, 0}, 1.6);
   *//* sfera_2->matAmbient = glm::vec3{0.1, 0, 0};
    sfera_2->matDiffuse = glm::vec3{0.6, 0, 0};
    sfera_2->matSpecular = glm::vec3{0.7, 0.7, 0.7};
    sfera_2->matShininess = 30.0f;
    sfera_2->reflectance = 0;
    sfera_2->isTexture = true;
    sfera_2->texture = CImage::createTexture(400, 400);*//*
    objectList.push_back(sfera_2);*/

/*    CSphere* sfera_3 = new CSphere({-3, -2, -2}, 0.6);
    sfera_3->matAmbient = glm::vec3{0, 0, 0.1};
    sfera_3->matDiffuse = glm::vec3{0, 0, 0.6};
    sfera_3->matSpecular = glm::vec3{0.7, 0.7, 0.7};
    sfera_3->matShininess = 30.0f;
    objectList.push_back(sfera_3);*/

 /*   CTriangle* trojkat_1 = new CTriangle({5, 5, -5}, {-5, 5, -5}, {-5, -5, -5});
    trojkat_1->matAmbient = glm::vec3 {0.1, 0.1, 0.1};
    trojkat_1->matDiffuse = glm::vec3 {0.4, 0.4, 0.4};
    trojkat_1->matSpecular = glm::vec3 {0, 0, 0};
    trojkat_1->matShininess = 0.0f;
    trojkat_1->reflectance = 1;
    objectList.push_back(trojkat_1);*/

    /*CTriangle* trojkat_2 = new CTriangle({5, 5, -5}, {-5, -5, -5}, {5, -5, -5});
    trojkat_2->matAmbient = glm::vec3 {0.1, 0.1, 0.1};
    trojkat_2->matDiffuse = glm::vec3 {0.4, 0.4, 0.4};
    trojkat_2->matSpecular = glm::vec3 {0, 0, 0};
    trojkat_2->matShininess = 0.0f;
    trojkat_2->reflectance = 1;
    objectList.push_back(trojkat_2);*/

  /*  CTriangle* triangle = new CTriangle({3, 3, -5}, {-3, 3, -10}, {-3, -3, -8});
    objectList.push_back(triangle);*/

/*    CLight swiatlo_1(glm::vec3(-3, -2, 8));
    swiatlo_1.color = {0.6, 0.6, 0.6};
    lightList.push_back(swiatlo_1);*/
    /*CLight swiatlo_2(glm::vec3(5, -2, 8));
    swiatlo_2.color = {0.3, 0.3, 0.3};
    lightList.push_back(swiatlo_2);*/

}

