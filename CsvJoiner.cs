using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace CsvJoiner
{
    public static class CsvJoiner
    {
        public static List<JoinedRecord> HashJoin(List<CsvRecord> list1, List<CsvRecord> list2)
        {
            var hashTable = list1.ToDictionary(item => $"{item.Id}_{item.Department}", item => item);
            var result = new List<JoinedRecord>();

            foreach (var item in list2)
            {
                string key = $"{item.Id}_{item.Department}";
                if (hashTable.TryGetValue(key, out var matchingItem))
                {
                    result.Add(CombineRecords(matchingItem, item, true));
                    hashTable.Remove(key);
                }
                else
                {
                    result.Add(CreateJoinedRecord(item, false));
                }
            }

            foreach (var item in hashTable.Values)
            {
                result.Add(CreateJoinedRecord(item, false));
            }

            return result;
        }

        private static JoinedRecord CombineRecords(CsvRecord record1, CsvRecord record2, bool cruzo)
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

        private static JoinedRecord CreateJoinedRecord(CsvRecord record, bool cruzo)
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
    }
}