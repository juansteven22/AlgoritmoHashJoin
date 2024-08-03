using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvJoiner
{
    public static class CsvReader
    {
        public static List<CsvRecord> ReadCsvFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
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
    }
}