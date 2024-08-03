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
}