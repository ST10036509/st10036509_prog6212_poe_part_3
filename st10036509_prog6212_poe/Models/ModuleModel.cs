namespace st10036509_prog6212_poe.Models
{
    public class ModuleModel
    {
        public string? ModuleName { get; set; }
        public string? ModuleCode { get; set; }
        public double Credits { get; set; }
        public double ClassHours { get; set; }
        public Dictionary<string, double>? CompletedHours { get; set; }
        public double SelfStudyHours { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public int ModuleID { get; set; }

    }
}
