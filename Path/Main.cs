using System;
using System.IO;
namespace Labirint 
{
    class Program 
    {
        int Main() 
        {
            var labirint = new Labirint("input.txt");
            var result = labirint.SearchPath();
            using (var wr = new StreamWriter('output.txt')) 
            {
                wr.WriteLine(result);
            }

        }
    }
}