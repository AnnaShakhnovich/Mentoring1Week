using System;
using System.Linq;

namespace mentoringWeek1
{
    class Program
    {
        static void Main(string[] args)
        {
            var visitor = new FileSystemVisitor(s => s.EndsWith(".txt"));
            visitor.FileFound += foundReached;
            visitor.DitectoryFound += foundReached;
            visitor.Start += SearchStarted;
            visitor.Finish += SearchFinished;
            visitor.DirSearch(@"D:\kp").ToList().ForEach(Console.WriteLine);
            Console.Read();
        }

        static void foundReached(object sender, FoundEventArgs e)
        {
            //Console.WriteLine("{0} was found. It {1} the filtration.", e.Name,
                //e.IsFiltrationPassed ? "passed" : "didn't pass");
            if (!e.Name.EndsWith("LICENSE.txt"))
            {
                e.ExcludeEntry = true;
            }
        }

        static void SearchStarted(object sender, EventArgs e)
        {
            Console.WriteLine("Search started");
        }

        static void SearchFinished(object sender, EventArgs e)
        {
            Console.WriteLine("Search finished");
        }
    }
}
