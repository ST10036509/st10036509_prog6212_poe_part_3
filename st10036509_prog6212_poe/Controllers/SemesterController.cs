using HoursForYourLib;
using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;
using System.Data.SqlClient;
using System.Text.Json;

namespace st10036509_prog6212_poe.Controllers
{
    public class SemesterController : Controller
    {
        //declare variables
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";
        List<ModuleModel> modulesList = new List<ModuleModel>();

        // GET: /<controller>/
        [HttpGet]
        //create action method
        public IActionResult SemesterCreation(int userID, List<ModuleModel> modules = null, 
            string semesterName = null, int numberOfWeeks = 1, string startDate = null)
        {
            //create list to store modules
            modulesList = modules;
            //pass values to view
            ViewBag.Modules = modulesList;
            ViewBag.UserID = userID;
            ViewBag.SemesterName = semesterName;
            ViewBag.NumberOfWeeks = numberOfWeeks;
            ViewBag.StartDate = startDate;
            //return view with list of modules
            return View();
        }//end SemesterCreation method

        // POST: /<controller>/
        [HttpPost]
        //create action method
        public async Task<IActionResult> SemesterCreation(int userID, string semesterName, int numberOfWeeks, string startDate)
        {
            //create list to store modules
            List<ModuleModel> modules = ModuleRepository.GetModules();
            //assign each week the default value per module
            modules = ModuleHoursAssignment(numberOfWeeks, modules);

            //parse the start date string into a datetime object
            var parsedStartDate = DateTime.Parse(startDate);

            //loop through modules in a semester
            foreach (ModuleModel module in modules)
            {
                //assign the start date to each module
                module.SemesterStartDate = parsedStartDate;
            }

            //create semester object
            SemesterModel newSemester = new SemesterModel
            {
                SemesterName = semesterName,
                NumberOfWeeks = numberOfWeeks,
                StartDate = parsedStartDate,
                Modules = modules
            };

            //add semester to database and return semesterID
            var semesterID = await Task.Run(() => AddSemesterToDatabase(newSemester, userID));

            //loop through modules in a semester
            foreach (ModuleModel module in modules)
            {
                //add module to database
                await Task.Run(() => AddModuleToDatabase(module, semesterID));
            }

            //pass values to view
            ViewBag.UserID = userID;
            ViewBag.SemesterName = null;
            ViewBag.NumberOfWeeks = 1;
            ViewBag.StartDate = null;
            //return view with list of modules
            TempData["AlertMessage"] = "Semester Created Successfully";
            //return view with list of modules
            return View();
        }//end SemesterCreation method

        //calculate and assign each week the defualt value per module
        public List<ModuleModel> ModuleHoursAssignment(double weeks, List<ModuleModel> modules)
        {
            //loop through modules in a semester
            foreach (ModuleModel module in modules)
            {
                //local variable declaration
                double hoursHolder;
                //calculate the self study hours and store in a temp variable
                hoursHolder = ((module.Credits * 10) / weeks) - module.ClassHours;
                //check if the hours calculated is a negative number
                if (hoursHolder < 0)
                {
                    //if it is negative then set it to 0
                    module.SelfStudyHours = 0;
                }
                else
                {
                    //if it is positive set it to the calculated value
                    module.SelfStudyHours = hoursHolder;
                }
                //generate dictinary of weeks and give default value of calculated self study hours
                for (int i = 0; i < weeks; i++)
                {
                    //add week and default value to dictionary
                    module.CompletedHours.Add(i.ToString(), module.SelfStudyHours);
                }
            }

            

            return modules;
        }//end ModuleHoursAssignment method

        public async Task<int> AddSemesterToDatabase(SemesterModel newSemester, int userID)
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            //create sl query to add semester to database
            string query = "INSERT INTO Semesters (UserID, SemesterName, NumberOfWeeks, StartDate) OUTPUT INSERTED.SemesterID VALUES (@UserID, @SemesterName, @NumberOfWeeks, @StartDate);";
            //create command and assign parameter values
            SqlCommand command = new SqlCommand(query, cnn);
            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@SemesterName", newSemester.SemesterName);
            command.Parameters.AddWithValue("@NumberOfWeeks", newSemester.NumberOfWeeks);
            command.Parameters.AddWithValue("@StartDate", newSemester.StartDate);

            //execute query and return semesterID
            int semesterID = (int)await command.ExecuteScalarAsync();
            cnn.Close();
            return semesterID;
        }//end AddSemesterToDatabase method

        public async Task AddModuleToDatabase(ModuleModel newModule, int semesterID)
        {
            //serialize the dictionary of completed hours into json string format
            var jsonCompletedHours = JsonSerializer.Serialize(newModule.CompletedHours);

            //create sql query to add module to database
            string query = "INSERT INTO Modules (SemesterID, ModuleName, ModuleCode, NumberOfCredits, NumberOfHoursPerWeek, StartDate, SelfStudyHours, CompletedHours) VALUES (@SemesterID, @ModuleName, @ModuleCode, @NumberOfCredits, @NumberOfHoursPerWeek, @StartDate, @SelfStudyHours, @CompletedHours);";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            //create command and assign parameter values
            SqlCommand command = new SqlCommand(query, cnn);
            command.Parameters.AddWithValue("@SemesterID", semesterID);
            command.Parameters.AddWithValue("@ModuleName", newModule.ModuleName);
            command.Parameters.AddWithValue("@ModuleCode", newModule.ModuleCode);
            command.Parameters.AddWithValue("@NumberOfCredits", newModule.Credits);
            command.Parameters.AddWithValue("@NumberOfHoursPerWeek", newModule.ClassHours);
            command.Parameters.AddWithValue("@StartDate", newModule.SemesterStartDate);
            command.Parameters.AddWithValue("@SelfStudyHours", newModule.SelfStudyHours);
            command.Parameters.AddWithValue("@CompletedHours", jsonCompletedHours);

            //execute query
            await command.ExecuteNonQueryAsync();
            cnn.Close();
        }//end AddModuleToDatabase
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________