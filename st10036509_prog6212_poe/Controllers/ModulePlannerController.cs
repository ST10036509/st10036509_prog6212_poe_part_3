using Microsoft.AspNetCore.Mvc;
using st10036509_prog6212_poe.Models;
using System.Data.SqlClient;

namespace st10036509_prog6212_poe.Controllers
{
    public class ModulePlannerController : Controller
    {
        //declare variables
        string connectionString = "Server=tcp:dbserver-vc-cldv6212-st10036509.database.windows.net,1433; Initial Catalog = db-vc-prog6212-st10036509-part-2; Persist Security Info=False; User ID = ST10036509; Password=Randomsangh72; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout = 30;";

        // GET: /<controller>/
        [HttpGet]
        //create action method
        public async Task<IActionResult> UserModulePlanner(int userID, int semesterID)
        {
            //pass userID and semesterID to view
            ViewBag.SemesterID = semesterID;
            ViewBag.UserID = userID;

            //create connection
            SqlConnection cnn = new SqlConnection(connectionString);
            //create query
            string query = "SELECT ModuleID, ModuleName, SelfStudyHours FROM Modules WHERE SemesterID = @SemesterID";

            //create list to store modules
            List<ModuleModel> modules = new List<ModuleModel>();

            //create command
            using (SqlCommand command = new SqlCommand(query, cnn))
            {
                //add parameters
                command.Parameters.AddWithValue("@SemesterID", semesterID);
                //open connection
                cnn.Open();

                //create reader
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    //read each record
                    while (await reader.ReadAsync())
                    {
                        //create module object and add to list
                        modules.Add(new ModuleModel
                        {
                            ModuleID = (int)reader["ModuleID"],
                            ModuleName = reader["ModuleName"].ToString(),
                            SelfStudyHours = (int)reader["SelfStudyHours"]
                        });
                    }
                }
                //close connection
                cnn.Close();
            }
            //pass values to view
            ViewBag.Modules = modules;
            //return view with list of modules
            return View();
        }//end UserModulePlanner method
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________