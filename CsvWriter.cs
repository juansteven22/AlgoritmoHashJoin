using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvJoiner
{
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
}