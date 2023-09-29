namespace Graphics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Game myGame = new Game(800, 700, "Hire this man!"))
            {
                myGame.Run();
            }
        }
    }
}