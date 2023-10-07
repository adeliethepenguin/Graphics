#version 440 core

out vec4 FragColor;

uniform vec4 myGlobalColor;

//in vec4 interpolatedColor; //make sure the same as .vert

void main()
{
    FragColor=vec4(myGlobalColor);
   // FragColor=vec4(interpolatedColor);
    
    
}