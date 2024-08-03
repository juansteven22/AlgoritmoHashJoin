using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvJoiner
{
    /*
    public static class CsvWriter
    {
        public static void WriteCsvFile(List<JoinedRecord> records, string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath);
                using var csv = new CsvHelper.CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

                csv.WriteRecords(records);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al escribir el archivo: {ex.Message}");
                Console.WriteLine($"Ruta intentada: {Path.GetFullPath(filePath)}");
            }
        }
    }
    */
    public static class CsvWriter
    {
        public static void WriteCsvFile(List<JoinedRecord> records, string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath);
                using var csv = new CsvHelper.CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

                // Escribir el encabezado en el orden deseado
                csv.WriteField("Id");
                csv.WriteField("Name");
                csv.WriteField("Department");
                csv.WriteField("Salary");
                csv.WriteField("HireDate");
                csv.WriteField("Email");
                csv.WriteField("PhoneNumber");
                csv.WriteField("Position");
                csv.WriteField("YearsOfExperience");
                csv.WriteField("EducationLevel");
                csv.WriteField("PerformanceRating");
                csv.WriteField("ProjectsCompleted");
                csv.WriteField("BonusPercentage");
                csv.WriteField("EmergencyContact");
                csv.WriteField("ManagerId");
                csv.WriteField("Cruzo");
                csv.WriteField("Nodos");
                csv.NextRecord();

                // Escribir los registros en el orden especificado
                foreach (var record in records)
                {
                    csv.WriteField(record.Id);
                    csv.WriteField(record.Name);
                    csv.WriteField(record.Department);
                    csv.WriteField(record.Salary);
                    csv.WriteField(record.HireDate);
                    csv.WriteField(record.Email);
                    csv.WriteField(record.PhoneNumber);
                    csv.WriteField(record.Position);
                    csv.WriteField(record.YearsOfExperience);
                    csv.WriteField(record.EducationLevel);
                    csv.WriteField(record.PerformanceRating);
                    csv.WriteField(record.ProjectsCompleted);
                    csv.WriteField(record.BonusPercentage);
                    csv.WriteField(record.EmergencyContact);
                    csv.WriteField(record.ManagerId);
                    csv.WriteField(record.Cruzo);
                    csv.WriteField(record.Nodos);
                    csv.NextRecord();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al escribir el archivo: {ex.Message}");
                Console.WriteLine($"Ruta intentada: {Path.GetFullPath(filePath)}");
            }
        }
    }
}