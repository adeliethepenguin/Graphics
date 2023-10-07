using OpenTK.Graphics.OpenGL4;
namespace Graphics;

public class Shader : IDisposable
{
    public readonly int Handle;
    protected bool _isDisposed;

    public Shader(string vertexPath, string fragmentPath)
    {
        int vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexHandle, File.ReadAllText(@"D:\(more) Projects\Graphics\"+vertexPath));
        
        int fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentHandle, File.ReadAllText(@"D:\(more) Projects\Graphics\"+fragmentPath));

        int successState = 0; // 1 true 0 false
        
        
        // --- VERTEX SHADER LOADING ---
        
        GL.CompileShader(vertexHandle);
        GL.GetShader(vertexHandle, ShaderParameter.CompileStatus, out successState);
        
        if (successState == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error in Vertex Shader: " + GL.GetShaderInfoLog(vertexHandle));
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        // --- FRAGMENT SHADER LOADING ---
        
        GL.CompileShader(fragmentHandle);
        GL.GetShader(fragmentHandle, ShaderParameter.CompileStatus, out successState);
        
        
        if (successState == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error in Fragment Shader: " + GL.GetShaderInfoLog(fragmentHandle));
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        // Bind the shaders to the program

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexHandle);
        GL.AttachShader(Handle, fragmentHandle);
        
        // Handle Linking
        GL.LinkProgram(Handle);
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out successState);

        if (successState == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error in Fragment Shader: "+GL.GetProgramInfoLog(Handle));
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        // CLEAN UP VRAM
        GL.DetachShader(Handle, fragmentHandle);
        GL.DetachShader(Handle, vertexHandle);
        
        GL.DeleteShader(fragmentHandle);
        GL.DeleteShader(vertexHandle);
    }

    ~Shader()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Oops all spiders memory leak");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public int GetAttribLocation(string attributeName)
    {
        return GL.GetAttribLocation(Handle, attributeName);
    }

    public int GetUniformLocation(string attributeName)
    {
        return GL.GetUniformLocation(Handle, attributeName);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);//DO NOT CALL DECONSTRUCTOR
    }

    protected virtual void Dispose(bool state)
    {
        if (state)
        {
            GL.DeleteProgram(Handle);
            _isDisposed=true;
        }
    } 
}