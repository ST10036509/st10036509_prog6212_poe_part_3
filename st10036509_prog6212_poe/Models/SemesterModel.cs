namespace st10036509_prog6212_poe.Models
{
    public class SemesterModel
    {
        public string SemesterName { get; set; }
        public double NumberOfWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public List<ModuleModel> Modules { get; set; }
        public int SemesterID { get; set; }
    }
}
