﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AutoBuddy.Utilities.AutoShop;
using EloBuddy;
using EloBuddy.SDK;

namespace AutoBuddy.Utilities.AutoLvl
{
    internal class SkillGrabber
    {
        private struct ChampSkilltoLvl
        {
            public Champion champ;
            public SkillToLvl[] s;
        }

        private struct ChampName
        {
            public Champion champ;
            public string name;
        }
        private string status = "Updater started...";
        private List<ChampName> cn;
        private readonly string path;
        public SkillGrabber(string path)
        {
            this.path = path;
            
        }

        public void UpdateBuilds(bool[] locked = null)
        {
            if (locked != null)
                locked[0] = true;
            Drawing.OnEndScene += Drawing_OnDraw;
            var bw = new BackgroundWorker {WorkerReportsProgress = true};
            bw.DoWork += delegate(object o, DoWorkEventArgs args)
            {
                var b = o as BackgroundWorker;
                generateChampList();
                toFile(b);

            };

            bw.ProgressChanged += delegate(object o, ProgressChangedEventArgs args)
            {
                status = args.UserState.ToString();
            };

            bw.RunWorkerCompleted += delegate
            {
                status = "Skill sequences updated succesfully.";
                Core.DelayAction(() => { Drawing.OnEndScene -= Drawing_OnDraw; }, 2000);
                if (locked != null)
                    locked[0] = false;
            };

            bw.RunWorkerAsync();
        }
        

        private void Drawing_OnDraw(EventArgs args)
        {
            Drawing.DrawText(800, 10, Color.Coral, status, 14);
        }

        private void toFile(BackgroundWorker bw=null)
        {
            
            var stringi = new List<string>();
            foreach (var champLink in getChampLinks("http://www.mobafire.com/league-of-legends/champions"))
            {


                var iss = getSequence(getBestBuildLink(champLink));
                if(bw!=null)
                    bw.ReportProgress(0, "Updating skill sequences, current champ: " + iss.champ);
                else
                    status = "Updating skill sequences, current champ: " + iss.champ;
                var s = iss.champ + "=";
                for (var i = 0; i < 18; i++)
                {
                    s += iss.s[i].ToString();
                    if (i < 17)
                        s += ";";
                }
                stringi.Add(s);

            }
            File.WriteAllLines(path, stringi);
            
        }


        private List<string> getChampLinks(string startingLink)
        {

            var resp = startingLink.GetResponseText();
            var ret = new List<string>();
            var ind = BrutalExtensions.AllIndexesOf(resp, "\" class=\"champ-box");
            foreach (var i in ind)
            {
                var s = resp.Substring(i - 60, 60);
                ret.Add(s.Substring(s.IndexOf("<a href=\"") + 9));
            }
            return ret;
        }

        private static string[] getBestBuildLink(string champLink)
        {
            var resp = ("http://www.mobafire.com" + champLink).GetResponseText();
            var st =
    resp.Substring(
        resp.IndexOf("<span class=\"badge \"></span>") + 64, 200);
            var ret = new string[2];
            ret[0] = champLink.Substring(champLink.LastIndexOf("/")+1, champLink.LastIndexOf("-") - (champLink.LastIndexOf("/")+1));
            ret[1] = st.Substring(0, st.IndexOf("\" class=\"build-title"));
            
            return ret;
        }

        private ChampSkilltoLvl getSequence(string[] nameGuide)
        {

            var seq = new SkillToLvl[18];
            for (var i = 0; i < 18; i++)
            {
                seq[i] = SkillToLvl.NotSet;
            }


            var resp = ("http://www.mobafire.com" + nameGuide[1]).GetResponseText();
            var q =
                resp.Substring(
                    resp.IndexOf("<div class=\"float-right\" style=\"margin-left:7px;\"><img src=\"/images/key-q.png\"") - 2000, 2000);
            q = q.Substring(q.LastIndexOf("<div class=\"float-left\" style=\"margin-left:7px;\">") + 62);


            var matches = Regex.Matches(q, "[0-9]+");
            foreach (Match match in matches)
            {
                seq[int.Parse(match.ToString()) - 1] = SkillToLvl.Q;
            }


            q =
                resp.Substring(
                    resp.IndexOf("<div class=\"float-right\" style=\"margin-left:7px;\"><img src=\"/images/key-w.png\"") - 2000, 2000);
            q = q.Substring(q.LastIndexOf("<div class=\"float-left\" style=\"margin-left:7px;\">") + 62);


            matches = Regex.Matches(q, "[0-9]+");
            foreach (Match match in matches)
            {
                seq[int.Parse(match.ToString()) - 1] = SkillToLvl.W;
            }
            q =
    resp.Substring(
        resp.IndexOf("<div class=\"float-right\" style=\"margin-left:7px;\"><img src=\"/images/key-e.png\"") - 2000, 2000);
            q = q.Substring(q.LastIndexOf("<div class=\"float-left\" style=\"margin-left:7px;\">") + 62);


            matches = Regex.Matches(q, "[0-9]+");
            foreach (Match match in matches)
            {
                seq[int.Parse(match.ToString()) - 1] = SkillToLvl.E;
            }
            q =
    resp.Substring(
        resp.IndexOf("<div class=\"float-right\" style=\"margin-left:7px;\"><img src=\"/images/key-r.png\"") - 2000, 2000);
            q = q.Substring(q.LastIndexOf("<div class=\"float-left\" style=\"margin-left:7px;\">") + 62);


            matches = Regex.Matches(q, "[0-9]+");
            foreach (Match match in matches)
            {
                seq[int.Parse(match.ToString()) - 1] = SkillToLvl.R;
            }
            return new ChampSkilltoLvl
            {
                champ = cn.OrderByDescending(it => it.name.Match(nameGuide[0])).First().champ,
                s = seq
            };
        }


        private void generateChampList()
        {
            cn = new List<ChampName>();
            foreach (var c in from Champion champ in Enum.GetValues(typeof(Champion))
                                    select new ChampName
                                        {
                                            champ = champ,
                                            name = champ.ToString()
                                        })
            {
                cn.Add(c);
            }
        }


    }
}

