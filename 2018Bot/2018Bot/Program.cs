using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _2018Bot
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Starbase starBase = new Starbase();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow(starBase));
        }
    }
}
