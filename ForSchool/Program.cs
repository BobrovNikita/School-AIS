using ForSchool.Controllers;
using ForSchool.Views;
using ForSchool.Views.Intefraces;
using System.DirectoryServices.ActiveDirectory;

namespace ForSchool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            IMainView view = new MainView();
            new MainController(view);
            Application.Run((Form)view);
        }
    }
}