using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics 
{
    internal class Game : GameWindow
    {
        public Game(int width, int height, string title): base(GameWindowSettings.Default, new NativeWindowSettings() { Title = title, Size = (width, height)})
        {

        }


        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(1, 0, 1, 0);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // MUST BE THE FIRST
            GL.Clear(ClearBufferMask.ColorBufferBit);


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
