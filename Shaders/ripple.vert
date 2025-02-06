#version 440 core  

layout(location = 0) in vec3 jeffPosition;
uniform float[] vertices verts;

void main()
{
    float z = sort(verts.x*verts.x+verts.z*verts.z);
    float sombrero = sin(z-TIME)/2;
    verts.y=sombrero;
}