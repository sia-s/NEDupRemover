using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OrgNameDupRemover.Preprocessing;
using OrgNameDupRemover.Similarity;
using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Please specify the file to process. It needs to be a text file containing one name per line.");
                string exeFileName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
                Console.WriteLine(exeFileName + " [filename] [threshold 0.0-1.0]");
                return;
            }

            
            string namesFilePath = args[0];
            double threshold = 0.9;
            if (args.Length > 1)
            {
                double.TryParse(args[1], out threshold);
            }

            try
            {
                var watch = Stopwatch.StartNew();

                var tokenizer = new SimpleTokenizer();
                var names = File.ReadAllLines(namesFilePath);

                var tokenizedNameProvider = new TokenizedNameProvider(
                    tokenizer, 
                    new List<ITokenTransformer>{new AbbreviationTransformer()},
                    new List<IStringNameOptimizer> { new DomainnameOptimizer()});

                var namesTokens = tokenizedNameProvider.GetTokenizedNames(names);

                var statistics = new TokenStatistics();
                statistics.AddDocuments(namesTokens);

                var sim = new SoftTfIdf(statistics, new JaroWinklerDistance(), 0.93, true);                

                var dupFinder = new TokenBasedDuplicateFinder(sim);

                Console.WriteLine("Processing " + names.Length + " names.");
                Console.WriteLine("Similarity Algorithm: " + sim);
                Console.WriteLine("Similarity Threshold: " + threshold);
                Console.WriteLine("...");

                var list = dupFinder.Find(namesTokens, threshold);
                var multiple = list.Where(l => l.Count > 1).ToList();

                Console.WriteLine("Found " + (multiple.Sum(m => m.Count) - multiple.Count) + " duplicates.");
                
                string resultFilePath = namesFilePath + ".result.txt";
                string resultDupsOnlyFilePath = namesFilePath + ".result.dups.txt";

                Console.WriteLine("Outputing " + resultFilePath);
                WriteNameSetsToFile(resultFilePath, list);
                
                Console.WriteLine("Outputing " + resultDupsOnlyFilePath);
                WriteNameSetsToFile(resultDupsOnlyFilePath, multiple);

                watch.Stop();
                Console.WriteLine("Execution time: " + watch.ElapsedMilliseconds + "ms");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
            
            Console.ReadKey();
        }

        static void WriteNameSetsToFile(string filePath, List<List<TokenizedString>> nameSets)
        {
            File.WriteAllText(filePath, 
                string.Join(Environment.NewLine, 
                    nameSets.Select(l => 
                        string.Join(", ", l.Select(n => "\"" + n + "\"")
                                           .OrderBy(n => n)))));
        }
    }
}
