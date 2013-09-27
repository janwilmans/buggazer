using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BugGazer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Settings settings = Settings.DeserializeFromConfiguration();
            if (settings.VisualStyle)
            {
                Application.EnableVisualStyles();
            }
            Application.SetCompatibleTextRenderingDefault(true);

            MainForm form = new MainForm();
            form.Initialize(settings);
            Application.Run(form);
        }
    }
}
