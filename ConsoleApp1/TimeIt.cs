using System.Diagnostics;
using System.Globalization;



    // import with using static timeit for direct usage with tic/toc as in matlab;
    public class TimeIt
    {

        private static Stopwatch sw = new Stopwatch();
        private static Stopwatch ew = new Stopwatch();

        public static void tic()
        {
            sw.Restart();
        }

        public static void toc()
        {
            Console.WriteLine(string.Format(NumberFormatInfo.InvariantInfo, "Elapsed time: {0}s", sw.Elapsed.TotalSeconds));// sw.Elapsed.TotalSeconds);
        }
        public static bool elapsed(double minimumtime)
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
