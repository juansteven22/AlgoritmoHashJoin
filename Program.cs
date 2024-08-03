using System;
using System.IO;

namespace CsvJoiner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese la ruta del primer archivo CSV:");
            string csv1Path = Console.ReadLine();

            Console.WriteLine("Ingrese la ruta del segundo archivo CSV:");
            string csv2Path = Console.ReadLine();

            try
            {
                var list1 = CsvReader.ReadCsvFile(csv1Path);
                var list2 = CsvReader.ReadCsvFile(csv2Path);

                var joinedRecords = CsvJoiner.HashJoin(list1, list2);

                string outputFileName = "resultado_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputFileName);

                CsvWriter.WriteCsvFile(joinedRecords, outputPath);

                Console.WriteLine($"Se han unido los archivos CSV. El resultado se ha guardado en: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }
    }
}