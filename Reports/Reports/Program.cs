using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Core.Domain;
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

            Log.Inst.WriteToLogDEBUG(string.Format("Start program"));
            Bootstrapper.Start(new CoreModule());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
            Log.Inst.WriteToLogDEBUG(string.Format("Initialization program"));
            var settings = new Settings("vankorAPI", "JUe5J4cuWu", "https://iiko.biz:9900/api/0/", 60000);
            CoreContext.InitService.Init(settings);
            Log.Inst.WriteToLogDEBUG(string.Format("Run application"));
            Application.Run(new Form1());
        }
    }
}
