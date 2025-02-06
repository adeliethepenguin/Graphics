
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Graphics
{
    public static class StaticUtilities
    {
        public static readonly string RootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!+"\\";
        public static readonly string ShaderDirectory = RootDirectory + "Shaders\\";
        public static readonly string TextureDirectory = RootDirectory + "Textures\\";

        static StaticUtilities()
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
        }

        public static void CheckError(string stage)
        {
            ErrorCode errorCode = GL.GetError();
            if (errorCode != ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error WTFFFFFF ({stage}): {errorCode}");
            }
        }
    }

    

    
}
