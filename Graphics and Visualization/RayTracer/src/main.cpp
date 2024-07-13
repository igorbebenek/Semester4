
#include <iostream>


#include "rt.h"

extern void draw_robot();
extern void draw_triangles();

///
int main (int argc, char * const argv[]) {

   // draw_triangles();
    //draw_robot();

//    return 0;
    std::cout << "ray tracing ... \n";


    CScene scene;
    scene.create(); // defines sample scene parameters
    if(scene.cam.width == 0) {
        std::cout << "WARNING: scene not defined" << std::endl;
        return -1;
    }

    CRayTrace rt;
    CRay ray;
    COutput results;
    /// computes primary ray matrix
     glm::mat3 ray_matrix;
     CRayTrace::compPrimaryRayMatrix(scene.cam, ray_matrix);
     std::cout << "Camera projection matrix:" << std::endl;
     PRINT_MAT3(ray_matrix);

    /// computes ray direction for sample pixel positions
    // ...


    /// creates raster image object
    CImage image(scene.cam.width, scene.cam.height);
    CImage image2(scene.cam.width, scene.cam.height);
    CImage image3(scene.cam.width, scene.cam.height);
    /// main loop
    for (int j=0;j < scene.cam.height; j++)
    {
        for (int i = 0; i < scene.cam.width; i++)
        {
           /* std::cout << "i " << i << std::endl;
            std::cout << "j " << j << std::endl;*/


            /// position of the image point
            float fx = (float)i + 0.5f;
            float fy = (float) j + 0.5f;
            /// primary ray
            ray.pos = scene.cam.eyep;

            ray.dir = glm::normalize(ray_matrix * glm::vec3{fx, fy, 1});
            ////background color
            results.col = {0, 0, 0};
            /// secondary ray counter
            results.tree = 0;
            /// ray energy
            results.energy = 1.0f;
            /// rendering
            rt.rayTrace(scene, ray, results);
            /// handles pixel over-saturation
             if(results.col.x > 1 || results.col.y > 1 || results.col.z > 1) {
                results.col = {1,1,1};
            }

            /// writes pixel to output image
            image.setPixel(i, j, results.col);

             glm::vec3 rgb(0.0f, 0.0f, 0.0f);
             rgb.x = (ray.dir.x + 1.0f) / 2.0f;
             glm::vec3 rgb2(0.0f, 0.0f, 0.0f);
             rgb2.y = (ray.dir.y + 1.0f) / 2.0f;
             glm::vec3 rgb3(0.0f, 0.0f, 0.0f);
             rgb3.z = (ray.dir.z + 1.0f) / 2.0f;
             //std::cout << "\n\n" << ray.dir.x;

             //kolory
          /*   image.setPixel(i, j, rgb);
             image2.setPixel(i, j, rgb2);
             image3.setPixel(i, j, rgb3);
             image.save("kolor1.png",true);
             image.save("kolor2.png",true);
             image.save("kolor3.png",true);
            cv::imshow("kolor1",image2.getImage());*/
           cv::waitKey();


        }
    }
    //std::cout << "test" << std::endl;

    /// writes image to disk file with gamma correction


    //Zad 9
  /*  image.save("zad9.png",true);
    cv::imshow("obraz", image.getImage());
    cv::waitKey();*/
    //Zad 10
   /* image.save("zad10.png",true);
    cv::imshow("obraz", image.getImage());*/
    cv::waitKey();
    //Zad 12
   /* image.save("zad12.png",true);
    cv::imshow("obraz", image.getImage());
    cv::waitKey();*/
    //Zad 13
   /* image.save("zad13.png", true);
    cv::imshow("obraz", image.getImage());
    cv::waitKey();*/
    //Zad14
    //image.save("zad14.png", true);
    //cv::imshow("obraz", image.getImage());
   // cv::waitKey();
    //Zad16
   /* image.save("zad16.png", true);
    cv::imshow("obraz", image.getImage());
    cv::waitKey();*/

/*      cv::imshow("obraz2", image2.getImage());
      cv::imshow("obraz3", image3.getImage());
      cv::waitKey();
    image.save("obraz4.png",false);*/

    //cv::imshow("image", image.getImage());


  /*  std::cout << std::endl << std::endl;
    ray.pos = glm::vec3(0, 0, 10);
    ray.dir = glm::normalize(glm::vec3(0.3, 0.3, -1));
    float t = scene.objectList[2]->intersect(ray);

    std::cout << "Przeciecie nastepuje w t = " << t << std::endl;*/


    return 0;
}


/// Draws two trianges
void draw_triangles() {

    CImage img(1000, 1000);

    glm::vec3 color1(0.9,0.1,0.1); // red
    glm::vec3 color2(0.1,0.9,0.1); // green

    // draw circle
    glm::vec3 pp(0.0,0.0,1.0);
    float radius = 0.1f;
//    img.drawCircle(pp, radius, color1);

    // triangle vertices
    glm::vec3 pp0(0.4, 0.3, 1);
    glm::vec3 pp1(-0.4, 0.3, 1);
    glm::vec3 pp2(0.4, -0.3, 1);

    // draws triangle in 2D
    img.drawLine(pp0, pp1, color1);
    img.drawLine(pp1, pp2, color1);
    img.drawLine(pp2, pp0, color1);

    // translation
    float tX = 0.2f; // OX translation
    float tY = 0.1f; // OY translation
    glm::mat3x3 mTrans {{1,0,0}, {0,1,0}, {tX,tY,1}}; // translation matrix
    PRINT_MAT3(mTrans);

    // translation of vertices
    pp0 = mTrans * pp0;
    pp1 = mTrans * pp1;
    pp2 = mTrans * pp2;

    // draws triangle after translation
    img.drawLine(pp0, pp1, color2);
    img.drawLine(pp1, pp2, color2);
    img.drawLine(pp2, pp0, color2);

    img.save("trojkaty.png");
    cv::imshow("trojkat", img.getImage());
    cv::waitKey();

}
void draw_robot() {
    CImage img(1000, 1000);

    glm::vec3 color1(0.94,0.82,0.67); // red


    glm::vec3 pp(0.0,0.45,1.0);
    float radius = 0.1f;
    img.drawCircle(pp, radius, color1);
    // glowa koneic


    // body
    glm::vec3 pp0(-0.2, 0.3, 1);
    glm::vec3 pp1(0.2, 0.3, 1);
    glm::vec3 pp2(0.2, -0.3, 1);
    glm::vec3 pp3(-0.2, -0.3, 1);

    img.drawLine(pp0, pp1, color1);
    img.drawLine(pp1, pp2, color1);
    img.drawLine(pp2, pp3, color1);
    img.drawLine(pp3, pp0, color1);

    //rece







    img.save("robot.png");
    cv::imshow("ROBOT", img.getImage());
    cv::waitKey();
}








