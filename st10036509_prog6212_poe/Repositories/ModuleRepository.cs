using st10036509_prog6212_poe.Models;
using System.Text.Json;

namespace st10036509_prog6212_poe.Repositories
{
    public class ModuleRepository
    {
        private static List<ModuleModel> _modules = new List<ModuleModel>();

        public static List<ModuleModel> GetModules()
        {
            return _modules;
        }

        public static void AddModules(ModuleModel module)
        {
            _modules.Add(module);
        }

        public static void ClearModules()
        {
            _modules.Clear();
        }
    }
}
