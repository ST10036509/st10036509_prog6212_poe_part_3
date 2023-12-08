
using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using System.Data.SqlClient;
using System.Text.Json;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModuleViewerController : Controller
    {
        ModuleModel module = new ModuleModel();
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";

        [HttpGet]
        public async Task<IActionResult> UserModuleViewer(int userID,int semesterID, int moduleID) 
        {
            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "SELECT ModuleName, ModuleCode, NumberOfCredits, NumberOfHoursPerWeek, StartDate, SelfStudyHours, CompletedHours FROM Modules WHERE ModuleID = @ModuleID";

            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                command.Parameters.AddWithValue("@ModuleID", moduleID);
                cnn.Open();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        module.ModuleName = (string)reader["ModuleName"];
                        module.ModuleCode = (string)reader["ModuleCode"];
                        module.Credits = (int)reader["NumberOfCredits"];
                        module.ClassHours = (int)reader["NumberOfHoursPerWeek"];
                        module.SemesterStartDate = (DateTime)reader["StartDate"];
                        module.SelfStudyHours = (int)reader["SelfStudyHours"];

                        //convert the json string to a dictionary<string, double>
                        string jsonString = reader["CompletedHours"].ToString();
                        Dictionary<string, double> completedHours = JsonSerializer.Deserialize<Dictionary<string, double>>(jsonString);
                        module.CompletedHours = completedHours;
                    }
                }

                cnn.Close();
                ViewBag.SelectedModule = module;
                ViewBag.SemesterID = semesterID;
                ViewBag.ModuleID = moduleID;
                ViewBag.UserID = userID;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserModuleViewer(int userID, int semesterID, int moduleID, string moduleJson, DateTime selectedDate, string hours)
        {
            var currentModule = JsonSerializer.Deserialize<ModuleModel>(moduleJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            currentModule.ModuleID = moduleID;

            var updatedModule = IdentifyAndUpdateWeek(selectedDate, currentModule, hours);

            await Task.Run(() => UpdateCompletedHours(moduleID, updatedModule));

            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "SELECT ModuleName, ModuleCode, NumberOfCredits, NumberOfHoursPerWeek, StartDate, SelfStudyHours, CompletedHours FROM Modules WHERE ModuleID = @ModuleID";

            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                command.Parameters.AddWithValue("@ModuleID", moduleID);
                cnn.Open();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        module.ModuleName = (string)reader["ModuleName"];
                        module.ModuleCode = (string)reader["ModuleCode"];
                        module.Credits = (int)reader["NumberOfCredits"];
                        module.ClassHours = (int)reader["NumberOfHoursPerWeek"];
                        module.SemesterStartDate = (DateTime)reader["StartDate"];
                        module.SelfStudyHours = (int)reader["SelfStudyHours"];

                        //convert the json string to a dictionary<string, double>
                        string jsonString = reader["CompletedHours"].ToString();
                        Dictionary<string, double> completedHours = JsonSerializer.Deserialize<Dictionary<string, double>>(jsonString);
                        module.CompletedHours = completedHours;
                    }
                }

                cnn.Close();
                ViewBag.SelectedModule = module;
                ViewBag.SemesterID = semesterID;
                ViewBag.ModuleID = moduleID;
                ViewBag.UserID = userID;
                TempData["AlertMessage"] = "Successfully Added Hours!";
                return View();
            }         
        }

        //tuple to identify the week and update the hours
        public ModuleModel IdentifyAndUpdateWeek(DateTime date, ModuleModel selectedModule, string hours)
        {
            //local variable declarations:
            double dayIndex = ((date.Subtract(selectedModule.SemesterStartDate).TotalDays) / 7) - 1;//calculate an index to check which week the selected date is in
            //loop through the weeks
            foreach (KeyValuePair<string, double> week in selectedModule.CompletedHours)
            {
                //check if the index is within the selected week
                if (dayIndex >= int.Parse(week.Key) + 1)
                {
                    //skip if it is bigger
                    continue;
                }
                else
                {
                    //update the hours
                    selectedModule.CompletedHours[week.Key] -= Double.Parse(hours);

                    //check if the hours are less than 0
                    if (selectedModule.CompletedHours[week.Key] < 0)
                    {
                        //set to default if is less than 0
                        selectedModule.CompletedHours[week.Key] = 0;
                    }
                    break;
                }
            }
            return (selectedModule);
        }//end IdentifyAndUpdateWeek method

        public async Task UpdateCompletedHours(int moduleID, ModuleModel selectedModule)
        {
            //convert the dictionary to a json string
            var jsonCompletedHours = JsonSerializer.Serialize(selectedModule.CompletedHours);

            //sql query to update  selected module data
            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "UPDATE Modules SET CompletedHours = @CompletedHours WHERE ModuleID = @ModuleID;";
            cnn.Open();
            //create sql command and set paramter values
            SqlCommand command = new SqlCommand(query, cnn);
            command.Parameters.AddWithValue("@ModuleID", moduleID);
            command.Parameters.AddWithValue("@CompletedHours", jsonCompletedHours);

            //execute the query
            await command.ExecuteNonQueryAsync();
            cnn.Close();
        }//end UpdateModuleDatabase method
    }
}
