using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.GL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics 
{
    internal class Game : GameWindow
    {
        private Shader myShader;

        private int vertexBufferObject;
        private int vertexArrayObject;

        private float[] vertices =
        {
            -1, -1, 0,       //  1, 0, 0, // bottom left       RED
             1, -1, 0,    //     0, 1, 0, // bottom right      GREEN
             0,  1, 0,       //   0, 0, 1 // top middle      BLUE
        };
        
        
        public Game(int width, int height, string title): base(GameWindowSettings.Default, new NativeWindowSettings() { Title = title, Size = (width, height)})
        {

        }


        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(1, 0, 1, 0);

            myShader = new Shader("myShader.vert", "myShader.frag");
            
            // Initialize VBO

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);
            
            // Static draw --> Things are not moving or changing (The array)
            // Dynamic --> Things are often changing or moving
            // Stream draw --> Thigns are constantly moving or changing
            
            // Initialize VAO

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int id = myShader.GetAttribLocation("jeffPosition");
            GL.VertexAttribPointer(id, 3, VertexAttribPointerType.Float, false, 
                3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(id);
            
            
           // id = myShader.GetAttribLocation("vertexColor");
      //      GL.VertexAttribPointer(id, 3, VertexAttribPointerType.Float, false, 
        //        6 * sizeof(float), 3*(sizeof(float)));
        //    GL.EnableVertexAttribArray(id);

            myShader.Use();

        }

        protected override void OnUnload()
        {
            // Free VRAM

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.UseProgram(0);
            
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteBuffer(vertexArrayObject);

            myShader.Dispose();
            
            //Last part:
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }

        private float x = 0;
        private float speed = 1/3f;
        private double counter = 0;
        private float redHolder = 0;
        private float greenHolder = 0;
        private float blueHolder;
        
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // MUST BE THE FIRST
            GL.Clear(ClearBufferMask.ColorBufferBit);

            myShader.Use();
            counter = counter + UpdateTime;
            x = (x + speed*(float)UpdateTime)%1; // & 1 means fract in CG

            int id = myShader.GetUniformLocation("myGlobalColor");
            switch (counter)
            {
                case <3: //starts at 1   0.5   0 orange
                    GL.Uniform4(id, new Color4(1, 0.5f-(x/2), 0f, 1.0f)); // 1  0  0 ( goes to red)
                    break;
                case <6:
                    GL.Uniform4(id, new Color4(1-x, x, x, 1.0f)); // 0  1  1 (aquamarine)
                    break;
                case <9:
                    GL.Uniform4(id, new Color4(x, 1f, 1-x, 1.0f)); // 1  1  0 (yellow)
                    break;
                case <12:
                    GL.Uniform4(id, new Color4(1-x, 1, 0, 1.0f)); // 0  0  1 (green)
                    break;
                case <15:
                    GL.Uniform4(id, new Color4(x, 1-(x/2), x, 1.0f)); // 1  0.5  1 (pink)
                    break;
                case <18:
                    GL.Uniform4(id, new Color4(1-(x/2), 0.5f, 1, 1.0f)); // 0.5  0.5  1 (blue)
                    break;
                case <21:
                    GL.Uniform4(id, new Color4(0.5f+(x/2), 0.5f, 1-x, 1.0f)); // 1  0.5  0 (orange)
                    break;  
            }      

            if (counter >= 21) // should end on    1   0.5   0 (orange)
            {
                counter = 0;
            }
                
                
            

            GL.BindVertexArray((vertexArrayObject));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            // MUST BE THE LAST
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}
