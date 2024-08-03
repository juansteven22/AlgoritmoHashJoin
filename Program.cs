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
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public int? YearsOfExperience { get; set; }
        public string EducationLevel { get; set; }
        public string PerformanceRating { get; set; }
        public int? ProjectsCompleted { get; set; }
        public decimal? BonusPercentage { get; set; }
        public string EmergencyContact { get; set; }
        public string ManagerId { get; set; }
    }

    public class JoinedRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public string HireDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public int? YearsOfExperience { get; set; }
        public string EducationLevel { get; set; }
        public string PerformanceRating { get; set; }
        public int? ProjectsCompleted { get; set; }
        public decimal? BonusPercentage { get; set; }
        public string EmergencyContact { get; set; }
        public string ManagerId { get; set; }
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
                        case "id": record.Id = value; break;
                        case "name": record.Name = value; break;
                        case "department": record.Department = value; break;
                        case "salary": decimal.TryParse(value, out decimal salary); record.Salary = salary; break;
                        case "hiredate": record.HireDate = value; break;
                        case "email": record.Email = value; break;
                        case "phonenumber": record.PhoneNumber = value; break;
                        case "position": record.Position = value; break;
                        case "yearsofexperience": int.TryParse(value, out int years); record.YearsOfExperience = years; break;
                        case "educationlevel": record.EducationLevel = value; break;
                        case "performancerating": record.PerformanceRating = value; break;
                        case "projectscompleted": int.TryParse(value, out int projects); record.ProjectsCompleted = projects; break;
                        case "bonuspercentage": decimal.TryParse(value, out decimal bonus); record.BonusPercentage = bonus; break;
                        case "emergencycontact": record.EmergencyContact = value; break;
                        case "managerid": record.ManagerId = value; break;
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

            foreach (var item in list2)
            {
                string key = $"{item.Id}_{item.Department}";
                if (hashTable.TryGetValue(key, out var matchingItem))
                {
                    // Registros que cruzan
                    result.Add(CombineRecords(matchingItem, item, true));
                    hashTable.Remove(key);
                }
                else
                {
                    // Registros de list2 que no cruzan
                    result.Add(CreateJoinedRecord(item, false));
                }
            }

            // Registros restantes de list1 que no cruzaron
            foreach (var item in hashTable.Values)
            {
                result.Add(CreateJoinedRecord(item, false));
            }

            return result;
        }

        static JoinedRecord CombineRecords(CsvRecord record1, CsvRecord record2, bool cruzo)
        {
            return new JoinedRecord
            {
                Id = record1.Id ?? record2.Id,
                Name = record1.Name ?? record2.Name,
                Department = record1.Department ?? record2.Department,
                Salary = record1.Salary != 0 ? record1.Salary : record2.Salary,
                HireDate = record1.HireDate ?? record2.HireDate,
                Email = record1.Email ?? record2.Email,
                PhoneNumber = record1.PhoneNumber ?? record2.PhoneNumber,
                Position = record1.Position ?? record2.Position,
                YearsOfExperience = record1.YearsOfExperience ?? record2.YearsOfExperience,
                EducationLevel = record1.EducationLevel ?? record2.EducationLevel,
                PerformanceRating = record1.PerformanceRating ?? record2.PerformanceRating,
                ProjectsCompleted = record1.ProjectsCompleted ?? record2.ProjectsCompleted,
                BonusPercentage = record1.BonusPercentage ?? record2.BonusPercentage,
                EmergencyContact = record1.EmergencyContact ?? record2.EmergencyContact,
                ManagerId = record1.ManagerId ?? record2.ManagerId,
                Cruzo = cruzo,
                Nodos = JsonSerializer.Serialize(new[] { record1, record2 })
            };
        }

        static JoinedRecord CreateJoinedRecord(CsvRecord record, bool cruzo)
        {
            return new JoinedRecord
            {
                Id = record.Id,
                Name = record.Name,
                Department = record.Department,
                Salary = record.Salary,
                HireDate = record.HireDate,
                Email = record.Email,
                PhoneNumber = record.PhoneNumber,
                Position = record.Position,
                YearsOfExperience = record.YearsOfExperience,
                EducationLevel = record.EducationLevel,
                PerformanceRating = record.PerformanceRating,
                ProjectsCompleted = record.ProjectsCompleted,
                BonusPercentage = record.BonusPercentage,
                EmergencyContact = record.EmergencyContact,
                ManagerId = record.ManagerId,
                Cruzo = cruzo,
                Nodos = JsonSerializer.Serialize(new[] { record })
            };
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