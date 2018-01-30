using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CurApp
{
    static class Program
    {
        public static readonly ApplicationContext Context = new ApplicationContext();

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var controller = new ApplicationController(Context);
            controller.Run();

        }
    }
}
