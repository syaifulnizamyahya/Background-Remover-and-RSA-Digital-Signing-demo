using Newtonsoft.Json;
using Python.Runtime;
using System.Diagnostics;
using System.IO;
using System.Text;

using WpfTemplateStudio.Core;

namespace WpfTemplateStudio.Core.Services
{
    public sealed class PythonInitializerService
    {
        private static PythonInitializerService instance = null;

        private static readonly object padlock = new object();

        private PythonInitializerService()
        {
            var virtualEnvFolder = WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_Conda;
            var pythonDll = WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_PythonDll;

            string pathToVirtualEnv;
            string currentDirectory;
            if (Debugger.IsAttached)
            {
                currentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName ?? throw new Exception(WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_FailedToGetCurrentDirectory);
                pathToVirtualEnv = currentDirectory + virtualEnvFolder;
            }
            else
            {
                //TODO: Define your virtual env path here
                //throw new Exception("Undefined virtual environment path.");
                currentDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName ?? throw new Exception(WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_FailedToGetCurrentDirectory);
                pathToVirtualEnv = currentDirectory + virtualEnvFolder;
            }

            var path = Environment.GetEnvironmentVariable("PATH") ?? throw new Exception(WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_FailedToGetPATHEnvironmentVariable);
            var process = EnvironmentVariableTarget.Process;
            path = path.TrimEnd(';');
            path = string.IsNullOrEmpty(path) ? pathToVirtualEnv : path + ";" + pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", path, process);
            Environment.SetEnvironmentVariable("PATH", pathToVirtualEnv, process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, process);
            Environment.SetEnvironmentVariable("PYTHONPATH", $"{pathToVirtualEnv}\\Lib\\site-packages;{pathToVirtualEnv}\\Lib", process);

            try
            {
                Runtime.PythonDLL = pathToVirtualEnv + pythonDll;

                PythonEngine.PythonHome = pathToVirtualEnv;
                PythonEngine.PythonPath = pathToVirtualEnv + @"\DLLs;" + Environment.GetEnvironmentVariable("PYTHONPATH", process);

                PythonEngine.Initialize();
                PythonEngine.BeginAllowThreads();
            }
            catch (Exception)
            {
                throw new Exception(WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_PythonInitializationFailed);
            }
        }

        /// <summary>
        /// Phyton initializer singleton
        /// </summary>
        public static PythonInitializerService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new PythonInitializerService();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Initialize Phyton
        /// </summary>
        /// <returns>Phyton version</returns>
        /// <exception cref="Exception"></exception>
        public string Initialize()
        {
            try
            {
                using (Py.GIL())
                {
                    dynamic sys = Py.Import("sys");
                    return string.Format(WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_Initialize_InitializedPythonVersion + sys.version);
                }
            }
            catch (Exception)
            {
                throw new Exception(WpfTemplateStudio.Core.Properties.Resources.PythonInitializerService_PythonInitializerService_PythonInitializationFailed);
            }
        }
    }
}
