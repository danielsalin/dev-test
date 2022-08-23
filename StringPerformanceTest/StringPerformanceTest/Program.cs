using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StringPerformanceTest
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        string[] Lines;

        public int NumberOfLines;

        [Params("Bacon", "pork", "prosciutto")]
        public string SearchValue;

        [Params("Files/Bacon10.txt", "Files/Bacon25.txt", "Files/Bacon50.txt")]
        public string FileToRead;

        [GlobalSetup]
        public void GlobalSetup()
        {
            string fileLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FileToRead);
            Lines = File.ReadAllLines(fileLocation);
            NumberOfLines = Lines.Count();
        }


        [Benchmark]
        public int CountOccurrences()
        {
            string[] words;
            int count = 0;
            string lowCaseSearchValue = SearchValue.ToLower();  /*Convert search word to lower case to identify all occurances*/

            foreach (var line in Lines) /*Parse the text line by line*/
            {
                string editedLine = new string(line.Where(c => !char.IsPunctuation(c)).ToArray());  /*Remove punctuation from the line*/
                words = editedLine.Split(' ');  /*Split the line into words*/
                foreach (string word in words)  /*Parse the line word by word*/
                {
                    if (word.ToLower() == lowCaseSearchValue)   /*Convert the word to lower case so that all occurances are identified*/
                    {
                        count++;
                    }
                } 
            }
            return count;
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}
