using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvJoiner
{
    public class CsvRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public string HireDate { get; set; }
    }

    public class JoinedRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public string HireDate { get; set; }
        public bool Cruzo { get; set; }
        public string Nodos { get; set; }
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
                            decimal.TryParse(value, out decimal salary);
                            record.Salary = salary;
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
        static List<JoinedRecord> HashJoin(List<CsvRecord> list1, List<CsvRecord> list2)
        {
            var hashTable = list1.ToDictionary(item => $"{item.Id}_{item.Department}", item => item);
            var result = new List<JoinedRecord>();

            // Procesar list2 y crear registros unidos
            foreach (var item in list2)
            {
                string key = $"{item.Id}_{item.Department}";
                if (hashTable.TryGetValue(key, out var matchingItem))
                {
                    result.Add(new JoinedRecord
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Department = item.Department,
                        Salary = item.Salary,
                        HireDate = item.HireDate,
                        Cruzo = true,
                        Nodos = JsonSerializer.Serialize(new[] { matchingItem, item })
                    });
                    hashTable.Remove(key);
                }
                else
                {
                    result.Add(new JoinedRecord
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Department = item.Department,
                        Salary = item.Salary,
                        HireDate = item.HireDate,
                        Cruzo = false,
                        Nodos = JsonSerializer.Serialize(new[] { item })
                    });
                }
            }

            // Agregar los registros restantes de list1 que no cruzaron
            foreach (var item in hashTable.Values)
            {
                result.Add(new JoinedRecord
                {
                    Id = item.Id,
                    Name = item.Name,
                    Department = item.Department,
                    Salary = item.Salary,
                    HireDate = item.HireDate,
                    Cruzo = false,
                    Nodos = JsonSerializer.Serialize(new[] { item })
                });
            }

            return result;
        }
        static void WriteCsvFile(List<JoinedRecord> records, string filePath)
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