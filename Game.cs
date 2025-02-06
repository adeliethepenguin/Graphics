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
using StbImageSharp;

namespace Graphics
{
    internal class Game : GameWindow
    {
        private Shader myShader;
        private Texture tex0;
        private Texture tex1;

        public static Matrix4 view;
        public static Matrix4 projection;

        private GameObject A;
        private GameObject B;
        private GameObject C;

        public static Camera gameCam;
        private Vector2 previousMousePos;

        private float[] vertices =
        {
            -1, 1, 0,         0, 2, // TL       UV Bottom Left
             -1, -1, 0,       0, 0,     // BL      UV Bottom Right
             1,  -1, 0,       2, 0,// BR      UV Top Middle
             1, 1, 0,         2, 2 //TR
        };

        private uint[] indices =
        {
            0, 1, 2,
            0, 2, 3
        };


        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Title = title, Size = (width, height) })
        {
            //model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(80));

        }


        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(1, 0, 1, 0);

            previousMousePos = new Vector2(MouseState.X, MouseState.Y);

            myShader = new Shader("myShader.vert", "myShader.frag");
            tex0 = new Texture("container.jpg");
            tex1 = new Texture("Bender.png");

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            tex0.Use(TextureUnit.Texture0);
            tex1.Use(TextureUnit.Texture1);




            // id = myShader.GetAttribLocation("vertexColor");
            //      GL.VertexAttribPointer(id, 3, VertexAttribPointerType.Float, false, 
            //        6 * sizeof(float), 3*(sizeof(float)));
            //    GL.EnableVertexAttribArray(id);

            myShader.Use();

            int id = myShader.GetUniformLocation("gabeTex0");
            GL.ProgramUniform1(myShader.Handle, id, 0);

            id = myShader.GetUniformLocation("gabeTex1");
            GL.ProgramUniform1(myShader.Handle, id, 1);

            //id = myShader.GetUniformLocation("gabeTex0");
           // GL.ProgramUniform1(myShader.Handle, id, 2);


            gameCam = new Camera(Vector3.UnitZ * 3, (float)Size.X / Size.Y);
            A = new GameObject(vertices, indices, myShader);
            A.transform.Position += Vector3.UnitX * 3f;
            B = new GameObject(vertices, indices, myShader);
            C = new GameObject(vertices, indices, myShader);
            C.transform.Position += Vector3.UnitZ * 3;
        }

        protected override void OnUnload()
        {
            // Free VRAM

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.UseProgram(0);

            A.Dispose();
            B.Dispose();
            myShader.Dispose();

            //Last part:
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }

        private float x = 0;
        private float speed = 1 / 3f;
        private double counter = 0;
        private float redHolder = 0;
        private float greenHolder = 0;
        private float blueHolder;

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // MUST BE THE FIRST
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            view = gameCam.GetViewMatrix();
            projection = gameCam.GetProjectionMatrix();



            tex0.Use(TextureUnit.Texture0);
            tex1.Use(TextureUnit.Texture1);
            myShader.Use();


            counter = counter + UpdateTime;
            x = (x + speed * (float)UpdateTime) % 1; // & 1 means fract in CG

            int id = myShader.GetUniformLocation("myGlobalColor");
            switch (counter)
            {
                case < 3: //starts at 1   0.5   0 orange
                    GL.Uniform4(id, new Color4(1, 0.5f - (x / 2), 0f, 1.0f)); // 1  0  0 ( goes to red)
                    break;
                case < 6:
                    GL.Uniform4(id, new Color4(1 - x, x, x, 1.0f)); // 0  1  1 (aquamarine)
                    break;
                case < 9:
                    GL.Uniform4(id, new Color4(x, 1f, 1 - x, 1.0f)); // 1  1  0 (yellow)
                    break;
                case < 12:
                    GL.Uniform4(id, new Color4(1 - x, 1, 0, 1.0f)); // 0  0  1 (green)
                    break;
                case < 15:
                    GL.Uniform4(id, new Color4(x, 1 - (x / 2), x, 1.0f)); // 1  0.5  1 (pink)
                    break;
                case < 18:
                    GL.Uniform4(id, new Color4(1 - (x / 2), 0.5f, 1, 1.0f)); // 0.5  0.5  1 (blue)
                    break;
                case < 21:
                    GL.Uniform4(id, new Color4(0.5f + (x / 2), 0.5f, 1 - x, 1.0f)); // 1  0.5  0 (orange)
                    break;
            }

            if (counter >= 21) // should end on    1   0.5   0 (orange)
            {
                counter = 0;
            }

            A.Render();
            B.Render();
            C.Render();
            StaticUtilities.CheckError("0");


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



            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                gameCam.Position += gameCam.Forward * cameraSpeed * (float)args.Time; // Forward
            }

            if (KeyboardState.IsKeyDown(Keys.S))
            {
                gameCam.Position -= gameCam.Forward * cameraSpeed * (float)args.Time; // Backwards
            }

            if (KeyboardState.IsKeyDown(Keys.A))
            {
                gameCam.Position -= gameCam.Right * cameraSpeed * (float)args.Time; // Left
            }

            if (KeyboardState.IsKeyDown(Keys.D))
            {
                gameCam.Position += gameCam.Right * cameraSpeed * (float)args.Time; // Right
            }

            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                gameCam.Position += gameCam.Up * cameraSpeed * (float)args.Time; // Up
            }

            if (KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                gameCam.Position -= gameCam.Up * cameraSpeed * (float)args.Time; // Down
            }

            // Get the mouse state

            // Calculate the offset of the mouse position
            var deltaX = MouseState.X - previousMousePos.X;
            var deltaY = MouseState.Y - previousMousePos.Y;
            previousMousePos = new Vector2(MouseState.X, MouseState.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            gameCam.Yaw += deltaX * sensitivity;
            gameCam.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top


        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            gameCam.Fov -= e.OffsetY;
        }
    }
}
