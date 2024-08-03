using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvJoiner
{
    public class CsvRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Salary { get; set; }
        public string HireDate { get; set; }
    }

    public class JoinedRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Salary { get; set; }
        public string HireDate { get; set; }
    }

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
                var list1 = ReadCsvFile(csv1Path);
                var list2 = ReadCsvFile(csv2Path);

                var joinedRecords = HashJoin(list1, list2);

                string outputFileName = "resultado_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputFileName);

                WriteCsvFile(joinedRecords, outputPath);

                Console.WriteLine($"Se han unido los archivos CSV. El resultado se ha guardado en: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }
        static List<CsvRecord> ReadCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;

            var records = new List<CsvRecord>();
            while (csv.Read())
            {
                var record = new CsvRecord();
                foreach (var header in headers)
                {
                    var value = csv.GetField(header);
                    switch (header.ToLower())
                    {
                        case "id":
                            record.Id = value;
                            break;
                        case "name":
                            record.Name = value;
                            break;
                        case "department":
                            record.Department = value;
                            break;
                        case "salary":
                            record.Salary = value;
                            break;
                        case "hiredate":
                            record.HireDate = value;
                            break;
                    }
                }
                records.Add(record);
            }

            return records;
        }
        static List<CsvRecord> HashJoin(List<CsvRecord> list1, List<CsvRecord> list2)
        {
            var hashTable = list1.ToDictionary(item => item.Id, item => item);
            var result = new List<CsvRecord>();

            foreach (var item in list2)
            {
                if (hashTable.TryGetValue(item.Id, out var matchingItem))
                {
                    result.Add(new CsvRecord
                    {
                        Id = item.Id,
                        Name = matchingItem.Name ?? item.Name,
                        Department = matchingItem.Department ?? item.Department,
                        Salary = matchingItem.Salary ?? item.Salary,
                        HireDate = matchingItem.HireDate ?? item.HireDate
                    });
                }
            }

            return result;
        }
        static void WriteCsvFile(List<CsvRecord> records, string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath);
                using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

                csv.WriteRecords(records);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al escribir el archivo: {ex.Message}");
                Console.WriteLine($"Ruta intentada: {Path.GetFullPath(filePath)}");
            }
        }
    }
}