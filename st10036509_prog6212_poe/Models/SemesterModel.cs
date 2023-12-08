namespace st10036509_prog6212_poe.Models
{
    //create semester model
    public class SemesterModel
    {
        //create properties
        public string SemesterName { get; set; }
        public double NumberOfWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public List<ModuleModel> Modules { get; set; }
        public int SemesterID { get; set; }
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________