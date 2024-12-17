using System;
using System.Data;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DemoConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var csvPath = @"G:\Stock_predictor\all_stocks.csv"; //Path to a CSV file
            var columnToExtract = "Close"; // Column name to find
            var extractedData = new Dictionary<string, List<object>>();


            Console.WriteLine("Please, choose one or several parameters to display (separate each parameter by space): [Index] [Date] [Open] [High] [Low] [Close] [Adj Close] [Volume]");
            string input = Console.ReadLine();
            string[] requestedColumns = input.Split(' '); // Splits input by spaces


            using (var reader = new StreamReader(csvPath)) // Initializes Read operation, does not read the file itself
            {
                var headers = reader.ReadLine().Split(','); // Splits the first row into parts, like an array with indexes
                var columnIndices = new Dictionary<string, int>();

                foreach (var column in requestedColumns)
                {
                    int index = Array.IndexOf(headers, column);
                    if (index == -1)
                    {
                        Console.WriteLine($"Column '{column}' not found in the CSV file");
                        continue;
                    }
                    columnIndices[column] = index;
                    extractedData[column] = new List<object>();
                }


                // Reads the whole CSV file and extracts all data from target column 
                while (!reader.EndOfStream)
                {
                    var row = reader.ReadLine().Split(',');
                    foreach (var column in columnIndices.Keys)
                    {
                        int columnIndex = columnIndices[column];

                        if (column == "Date" || column == "Index")
                        {
                            extractedData[column].Add(row[columnIndex]);
                        }
                        else
                        {
                            if (double.TryParse(row[columnIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out double numValue))
                            {
                                extractedData[column].Add(numValue);
                            }
                        }
                    }
                }
                Console.WriteLine(string.Join("\t", requestedColumns));

                int rowCount = extractedData.Values.Min(list => list.Count);
                for (int i = 0; i < 30; i++)
                {
                    var rowData = requestedColumns.Select(col => extractedData[col][i].ToString()).ToArray();
                    Console.WriteLine(string.Join("\t", rowData));
                }
            }
        }
    }
}