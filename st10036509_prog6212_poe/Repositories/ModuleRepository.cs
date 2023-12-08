using st10036509_prog6212_poe.Models;
using System.Text.Json;

namespace st10036509_prog6212_poe.Repositories
{
    //create module repository
    public class ModuleRepository
    {
        //create list to store modules
        private static List<ModuleModel> _modules = new List<ModuleModel>();

        //create method to get modules
        public static List<ModuleModel> GetModules()
        {
            //return list of modules
            return _modules;
        }//end GetModules method

        //create method to add modules
        public static void AddModules(ModuleModel module)
        {
            //add module to repository list
            _modules.Add(module);
        }//end AddModules method

        //create method to clear the repository
        public static void ClearModules()
        {
            //clear the repository
            _modules.Clear();
        }//end ClearModules method
    }
}
//__________________________________________....oooOO0_END_OF_FILE_0OOooo....__________________________________________