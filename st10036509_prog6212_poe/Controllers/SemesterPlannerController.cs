using HoursForYourLib;
using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using st10036509_prog6212_poe.Repositories;
using System.Data.SqlClient;

namespace st10036509_prog6212_poe.Controllers
{
    public class SemesterPlannerController : Controller
    {
        //declare variables
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";
        List<ModuleModel> modules = new List<ModuleModel>();
        SemesterModel semester = new SemesterModel();

        // GET: /<controller>/
        [HttpGet]
        //create action method
        public async Task<IActionResult> UserSemesterPlanner(int userID)
        {
            //clear modules repository data
            ModuleRepository.ClearModules();
            //pass userID to view
            ViewBag.UserID = userID;
            //create connection
            SqlConnection cnn = new SqlConnection(connectionString);
            //create query
            string query = "SELECT SemesterID, SemesterName FROM Semesters WHERE UserID = @userID";
            //create list to store semesters
            List<SemesterModel> semesters = new List<SemesterModel>();

            //create command
            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                //add parameters
                command.Parameters.AddWithValue("@UserID", userID);
                //open connection
                cnn.Open();

                //create reader
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    //read each record
                    while (await reader.ReadAsync())
                    {
                        //create semester object
                        semesters.Add(new SemesterModel
                        {
                            SemesterName = reader["SemesterName"].ToString(),
                            SemesterID = (int)reader["SemesterID"]
                        });
                    }
                }
                //close connection
                cnn.Close();
            }

            //pass values to view
            ViewBag.Semesters = semesters;
            //return view with list of semesters
            return View();
        }//end UserSemesterPlanner method
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________