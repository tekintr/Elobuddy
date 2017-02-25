using EloBuddy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReGaren
{
    class VersionChecker
    {
        public static System.Version AssVersion { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

        public static bool Check()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var champ = Player.Instance.ChampionName;
                    string OnlineVersion = new WebClient().DownloadString("https://github.com/tekintr/YeniTurkce/tree/master/Garen/Re" + champ + "/Re" + champ + "/Properties/AssemblyInfo.cs");
                    var Match = new Regex(@"\[assembly\: AssemblyVersion\(""(\d+)\.(\d+)\.(\d+)""\)\]").Match(OnlineVersion);

                    if (Match.Success)
                    {
                        var LatestVersion = new System.Version(string.Format("{0}.{1}.{2}", Match.Groups[1], Match.Groups[2], Match.Groups[3]));

                        if (LatestVersion > AssVersion)
                            Chat.Print("<font color='#FFFFFF'>Your Re" + champ + " addon is</font> <font color='#CF2942'>OUTDATED</font>. <font color='#FFFFFF'>The latest version is " + LatestVersion + ".</font>");
                        else
                            Chat.Print("<font color='#FFFFFF'>Your Re" + champ + " addon is</font> <font color='#33CC66'>UP2DATE</font>. <font color='#FFFFFF'>Current version is " + AssVersion + ".</font>");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }
            });
            return false;
        }
    }
}
