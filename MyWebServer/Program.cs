using MyWebServer;
using System.Reflection;

namespace MyWebServer
{
    class Program
    {

        static async Task Main(string[] args)
        {
            StaticFileServer server = new StaticFileServer("http://localhost:8080/", Helper.GetWebsitePath());

            int res = await server.RunAsync(args);

            Console.WriteLine($"Ended with code {res}, press any key to continue");
            Console.ReadKey();
        }
    }


    /// <summary>
    /// Simple helper class 
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets the correct path to were the files will be located relative to the application execuable
        /// </summary>
        /// <returns></returns>
        public static string GetWebsitePath()
        {
            //path of the exe.
            string websitePath = Assembly.GetExecutingAssembly().Location;

            websitePath = websitePath.LeftOfRightmostOf("\\").LeftOfRightmostOf("\\").LeftOfRightmostOf("\\").LeftOfRightmostOf("\\") + "\\Views";

            return websitePath;
        }
    }

    /// <summary>
    /// extension to the string type to parse the local machine DIR
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// returns the left side of a string devided by the last string given as a delimiter
        /// </summary>
        /// <param name="src">string</param>
        /// <param name="s">Delimiter, the first characte/string going from right to left.</param>
        /// <returns></returns>
        public static string LeftOfRightmostOf(this String src, string s)
        {
            string ret = src;
            int idx = src.IndexOf(s);
            int idx2 = idx;

            while (idx2 != -1)
            {
                idx2 = src.IndexOf(s, idx + s.Length);

                if (idx2 != -1)
                {
                    idx = idx2;
                }
            }
            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }
            return ret;
        }
    }
}
