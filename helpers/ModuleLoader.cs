using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MaliyeHesaplama.helpers
{
    public class ModuleMenuItem
    {
        public string Title { get; set; }
        public string EntryControl { get; set; }
        public Assembly Assembly { get; set; }
    }
    public static class ModuleLoader
    {
        public static List<ModuleMenuItem> AllMenus = new List<ModuleMenuItem>();

        public static void LoadAllModules()
        {
            string modulesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules");

            if (!Directory.Exists(modulesPath))
                Directory.CreateDirectory(modulesPath);

            foreach (var moduleDir in Directory.GetDirectories(modulesPath))
            {
                var dllFile = Directory.GetFiles(moduleDir, "*.dll").FirstOrDefault();
                if (dllFile == null) continue;

                var asm = Assembly.LoadFrom(dllFile);

                string manifestPath = Path.Combine(moduleDir, "manifest.json");
                if (!File.Exists(manifestPath)) continue;

                string json = File.ReadAllText(manifestPath);
                var manifest = JsonSerializer.Deserialize<ManifestModel>(json);

                foreach (var menu in manifest.menus)
                {
                    AllMenus.Add(new ModuleMenuItem
                    {
                        Title = menu.title,
                        EntryControl = menu.entryControl,
                        Assembly = asm
                    });
                }
            }
        }

        public class ManifestModel
        {
            public string moduleName { get; set; }
            public List<MenuItemModel> menus { get; set; }
        }

        public class MenuItemModel
        {
            public string title { get; set; }
            public string entryControl { get; set; }
        }

    }
}
