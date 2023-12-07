using HoursForYourLib;
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
    }
}
