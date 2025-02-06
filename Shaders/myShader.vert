#version 440 core  

layout(location = 0) in vec3 jeffPosition;
layout(location = 1) in vec2 UV0;


//out vec4 interpolatedColor;

out vec2 UVs0;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(jeffPosition,1.0)*model*view*projection;

    UVs0 = UV0;
    
   // interpolatedColor = vec4(vertexColor, 1);
}