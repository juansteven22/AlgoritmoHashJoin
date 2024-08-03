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

    Console.WriteLine("Ingrese la ruta para guardar el resultado:");
    string outputPath = Console.ReadLine();

    try
    {
        var list1 = ReadCsvFile(csv1Path);
        var list2 = ReadCsvFile(csv2Path);

        var joinedRecords = HashJoin(list1, list2);

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
    using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
    return csv.GetRecords<CsvRecord>().ToList();
}
static List<JoinedRecord> HashJoin(List<CsvRecord> employees, List<CsvRecord> salaries)
{
    var hashTable = employees.ToDictionary(item => item.Id, item => item);
    var result = new List<JoinedRecord>();

    foreach (var salary in salaries)
    {
        if (hashTable.TryGetValue(salary.Id, out var employee))
        {
            result.Add(new JoinedRecord
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Salary = salary.Salary,
                HireDate = salary.HireDate
            });
        }
    }

    return result;
}
static void WriteCsvFile(List<JoinedRecord> records, string filePath)
{
    using var writer = new StreamWriter(filePath);
    using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
    csv.WriteRecords(records);
}
    }
}