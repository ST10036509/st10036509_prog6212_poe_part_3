namespace st10036509_prog6212_poe.Models
{
    //create module model
    public class ModuleModel
    {
        //create properties
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
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________