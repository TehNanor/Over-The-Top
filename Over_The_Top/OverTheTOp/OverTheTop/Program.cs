using System;

namespace OverTheTop
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (OverTheTop game = new OverTheTop())
            {
                game.Run();
            }
        }
    }
#endif
}

