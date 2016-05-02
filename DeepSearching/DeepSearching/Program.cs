using System.Collections.Generic;
using DeepSearching.Models;

namespace DeepSearching.Console
{
    class Program
    {
        //like ajax is calling it
        static void Main(string[] args)
        {
            while (true)
            {
                Worker worker = new Worker();

                List<EstimatedUrl> results = worker.Find("http://button.dekel.ru/", 2);

                System.Console.WriteLine();
                System.Console.WriteLine("results found: "+results.Count.ToString());

                System.Console.WriteLine("Continue?");
                string cont = System.Console.ReadLine();
                if (cont != "+")
                    break;
            }

        }
    }
}
