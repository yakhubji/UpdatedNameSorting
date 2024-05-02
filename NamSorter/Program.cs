using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameSortering
{
    public class Name
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public interface INameParser
    {
        List<Name> Parse(string filePath);
    }

    public interface INameSorter
    {
        List<Name> Sort(List<Name> names);
    }

    public interface INameOutput
    {
        void PrintToConsole(List<Name> names);
        void WriteToFile(List<Name> names, string filePath);
    }

    public class NameParser : INameParser
    {
        public List<Name> Parse(string filePath)
        {
            var names = new List<Name>();

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var nameParts = line.Split(' ');
                    var lastName = nameParts.Last();
                    var firstName = string.Join(" ", nameParts.Take(nameParts.Length - 1));
                    names.Add(new Name(firstName, lastName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing names: {ex.Message}");
            }

            return names;
        }
    }

    public class NameSorter : INameSorter
    {
        public List<Name> Sort(List<Name> names)
        {
            return names.OrderBy(n => n.LastName).ThenBy(n => n.FirstName).ToList();
        }
    }

    public class NameOutput : INameOutput
    {
        public void PrintToConsole(List<Name> names)
        {
            foreach (var name in names)
            {
                Console.WriteLine($"{name.FirstName} {name.LastName}");
            }
        }

        public void WriteToFile(List<Name> names, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var name in names)
                    {
                        writer.WriteLine($"{name.FirstName} {name.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }

    public class NameSorterApp
    {
        private readonly INameParser _nameParser;
        private readonly INameSorter _nameSorter;
        private readonly INameOutput _nameOutput;

        public NameSorterApp(INameParser nameParser, INameSorter nameSorter, INameOutput nameOutput)
        {
            _nameParser = nameParser;
            _nameSorter = nameSorter;
            _nameOutput = nameOutput;
        }

        public void Run(string inputFilePath, string outputFilePath)
        {
            var names = _nameParser.Parse(inputFilePath);
            if (names.Count == 0)
            {
                Console.WriteLine("No names found to sort.");
                return;
            }

            var sortedNames = _nameSorter.Sort(names);
            _nameOutput.PrintToConsole(sortedNames);
            _nameOutput.WriteToFile(sortedNames, outputFilePath);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\UnSortedNames.txt");
                string sFilePath = Path.GetFullPath(sFile);
                string outputCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string oFile = System.IO.Path.Combine(outputCurrentDirectory, @"..\..\..\sorted-names-list.txt");
                string oFilePath = Path.GetFullPath(oFile);

                var nameParser = new NameParser();
                var nameSorter = new NameSorter();
                var nameOutput = new NameOutput();
                var nameSorterApp = new NameSorterApp(nameParser, nameSorter, nameOutput);

                nameSorterApp.Run(sFilePath, oFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
    }
}

