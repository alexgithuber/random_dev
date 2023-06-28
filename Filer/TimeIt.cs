using System.Diagnostics;
using System.Globalization;



    // import with using static timeit for direct usage with tic/toc as in matlab;
    public class TimeIt
    {

        private static readonly Stopwatch sw = new();
        private static readonly Stopwatch ew = new();

        public static void Tic()
        {
            sw.Restart();
        }

        public static void Toc()
        {
            Console.WriteLine(string.Format(NumberFormatInfo.InvariantInfo, "Elapsed time: {0}s", sw.Elapsed.TotalSeconds));// sw.Elapsed.TotalSeconds);
        }
        public static bool Elapsed(double minimumtime)
        {
            if (!ew.IsRunning)
            { ew.Restart(); }
            if (ew.Elapsed.TotalSeconds > minimumtime)
            {
                ew.Restart(); return true;
            }
            else { return false; }


        }

    }
