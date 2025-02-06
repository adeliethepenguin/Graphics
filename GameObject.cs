using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Graphics
{
    public class GameObject
    {
        public Transform transform; // Every gameobject has a transform

        private int vertexBufferObject;
        private int vertexArrayObject;
        private int elementBufferObject;
        private object indices;
        private readonly uint[] Indices;
        private readonly Shader MyShader;

        public GameObject(float[] vertices, uint[] indices, Shader shader)
        {
            transform = new Transform();
            Indices = indices;
            MyShader = shader;

            // Initialize VBO

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Static draw --> Things are not moving or changing (The array)
            // Dynamic --> Things are often changing or moving
            // Stream draw --> Thigns are constantly moving or changing




            // Initialize VAO

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            int id = MyShader.GetAttribLocation("jeffPosition");
            GL.VertexAttribPointer(id, 3, VertexAttribPointerType.Float, false,
                5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(id);

            id = MyShader.GetAttribLocation("UV0");
            GL.VertexAttribPointer(id, 2, VertexAttribPointerType.Float, false,
                5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(id);


            // EBO
            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


        }
        public void Render()
        {
            MyShader.Use();

            
            int id = MyShader.GetUniformLocation("model");
            GL.UniformMatrix4(id, true, ref transform.GetMatrix);

            id = MyShader.GetUniformLocation("view");
            GL.UniformMatrix4(id, true, ref Game.view);

            id = MyShader.GetUniformLocation("projection");
            GL.UniformMatrix4(id, true, ref Game.projection);


            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);


            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(elementBufferObject);
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
            
        }
    }
}
