#version 440 core  

layout(location = 0) in vec3 jeffPosition;
layout(location = 1) in vec3 vertexColor;


//out vec4 interpolatedColor;

void main()
{
    gl_Position = vec4(jeffPosition,1);
    
   // interpolatedColor = vec4(vertexColor, 1);
}