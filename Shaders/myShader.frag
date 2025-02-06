#version 440 core

out vec4 FragColor;

uniform vec4 myGlobalColor;

//in vec4 interpolatedColor; //make sure the same as .vert

uniform sampler2D gabeTex0;
uniform sampler2D gabeTex1;
uniform float timesince;

in vec2 UVs0;

void main()
{
    vec4 texCol0 = texture(gabeTex0, UVs0);
    vec4 texCol1 = texture(gabeTex1, UVs0);


    float avg = (texCol1.r + texCol1.b + texCol1.g)/3;

    if (avg>0.9f)
    {
    texCol1=myGlobalColor;
    }

    FragColor=mix(texCol0, texCol1, texCol1.a);

   // FragColor=vec4(myGlobalColor);
   // FragColor=vec4(interpolatedColor);
    
    
}