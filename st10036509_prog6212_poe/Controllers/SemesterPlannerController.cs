using HoursForYourLib;
using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using System.Data.SqlClient;

namespace st10036509_prog6212_poe.Controllers
{
    public class SemesterPlannerController : Controller
    {
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";
        List<ModuleModel> modules = new List<ModuleModel>();
        SemesterModel semester = new SemesterModel();

        [HttpGet]
        public async Task<IActionResult> UserSemesterPlanner(int userID)
        {
            ViewBag.UserID = userID;
            SqlConnection cnn = new SqlConnection(connectionString);
            string query = "SELECT SemesterID, SemesterName FROM Semesters WHERE UserID = @userID";

            List<SemesterModel> semesters = new List<SemesterModel>();

            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                command.Parameters.AddWithValue("@UserID", userID);
                cnn.Open();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        //semesterNames.Add(reader["SemesterName"].ToString());
                        semesters.Add(new SemesterModel
                        {
                            Name = reader["SemesterName"].ToString(),
                            SemesterID = (int)reader["SemesterID"]
                        });
                    }
                }

                cnn.Close();
            }

            ViewBag.Semesters = semesters;
            return View();
        }
    }
}
