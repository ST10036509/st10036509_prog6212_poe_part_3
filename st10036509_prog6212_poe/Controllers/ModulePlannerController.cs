using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using System.Data.SqlClient;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModulePlannerController : Controller
    {
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";


        [HttpGet]
        public async Task<IActionResult> UserModulePlanner(int userID, int semesterID)
        {
            ViewBag.SemesterID = semesterID;
            ViewBag.UserID = userID;

            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "SELECT ModuleID, ModuleName, SelfStudyHours FROM Modules WHERE SemesterID = @SemesterID";

            List<ModuleModel> modules = new List<ModuleModel>();

            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                command.Parameters.AddWithValue("@SemesterID", semesterID);
                cnn.Open();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modules.Add(new ModuleModel
                        {
                            ModuleID = (int)reader["ModuleID"],
                            Name = reader["ModuleName"].ToString(),
                            Hours = reader["SelfStudyHours"].ToString()
                        });
                    }
                }

                cnn.Close();
            }

            ViewBag.Modules = modules;
            return View();
        }
    }
}
