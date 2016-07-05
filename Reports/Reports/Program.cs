using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core.Module;
using Core.Services.Implimintation;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using Framework;

namespace Reports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Bootstrapper.Start(new CoreModule());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
            //CoreContext.InitService.Init();
            Application.Run(new Form1());
        }
    }
}
