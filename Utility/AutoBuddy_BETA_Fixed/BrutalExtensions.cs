using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AutoBuddy.Humanizers;
using AutoBuddy.Utilities;
using AutoBuddy.Utilities.AutoShop;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace AutoBuddy
{
    internal static class BrutalExtensions
    {
        
        //This was causing AutoBuddy to not load in Medium bot games :/
        //public static string GetGameType()
        //{

        //    if (EntityManager.Heroes.Allies.Count < 5 || EntityManager.Heroes.Allies.Count(en=>en.Name.EndsWith(" Bot"))>1)
        //        return "custom";
            
        //    if (EntityManager.Heroes.Enemies.All(en => en.Name.EndsWith(" Bot")))
        //    {
        //        return EntityManager.Heroes.Enemies.All(en =>en.SkinId==0) ? "bot_easy" : "bot_intermediate";
        //    }
        //    return "normal";
        //}


        public static string GetGameType()
        {
            return "custom";
        }
        public static Lane GetLane(this Obj_AI_Minion min)
        {
            try
            {
                if (min.Name == null || min.Name.Length < 13) return Lane.Unknown;
                if (min.Name[12] == '0') return Lane.Bot;
                if (min.Name[12] == '1') return Lane.Mid;
                if (min.Name[12] == '2') return Lane.Top;
            }
            catch (Exception e)
            {
                Console.WriteLine("GetLane:" + e.Message);
            }
            return Lane.Unknown;
        }

        public static Lane GetLane(this Obj_AI_Turret tur)
        {
            if (tur.Name.EndsWith("Shrine_A")) return Lane.Spawn;
            if (tur.Name.EndsWith("C_02_A") || tur.Name.EndsWith("C_01_A")) return Lane.HQ;
            if (tur.Name == null || tur.Name.Length < 12) return Lane.Unknown;
            if (tur.Name[10] == 'R') return Lane.Bot;
            if (tur.Name[10] == 'C') return Lane.Mid;
            if (tur.Name[10] == 'L') return Lane.Top;
            return Lane.Unknown;
        }

        public static int GetWave(this Obj_AI_Minion min)
        {
            if (min.Name == null || min.Name.Length < 17) return 0;
            int result;
            try
            {
                result = Int32.Parse(min.Name.Substring(14, 2));
            }
            catch (FormatException)
            {
                result = 0;
                Console.WriteLine("GetWave error, minion name: " + min.Name);
            }
            return result;
        }

        public static Vector3 RotatedAround(this Vector3 rotated, Vector3 around, float angle)
        {
            var s = Math.Sin(angle);
            var c = Math.Cos(angle);

            var ret = new Vector2(rotated.X - around.X, rotated.Y - around.Y);

            var xnew = ret.X*c - ret.Y*s;
            var ynew = ret.X*s + ret.Y*c;

            ret.X = (float) xnew + around.X;
            ret.Y = (float) ynew + around.Y;

            return ret.To3DWorld();
        }

        public static Vector3 Randomized(this Vector3 vec, float min = -300, float max = 300)
        {
            return new Vector3(vec.X + RandGen.r.NextFloat(min, max), vec.Y + RandGen.r.NextFloat(min, max), vec.Z);
        }

        public static Obj_AI_Turret GetNearestTurret(this Vector3 pos, bool enemy = true)
        {
            return
                ObjectManager.Get<Obj_AI_Turret>()
                    .Where(tur => tur.Health > 0 && tur.IsAlly ^ enemy)
                    .OrderBy(tur => tur.Distance(pos))
                    .First();
        }

        public static Obj_AI_Turret GetNearestTurret(this Obj_AI_Base unit, bool enemy = true)
        {
            return unit.Position.GetNearestTurret(enemy);
        }

        public static bool IsVisible(this Obj_AI_Base unit)
        {
            return !unit.IsDead() && unit.IsHPBarRendered;
        }

        public static bool IsDead(this Obj_AI_Base unit)
        {
            return unit.Health <= 0;
        }

        public static float HealthPercent(this Obj_AI_Base unit)
        {
            return unit.Health/unit.MaxHealth*100f;
        }

        public static float ManaPercent(this Obj_AI_Base unit)
        {
            return unit.Mana/unit.MaxMana*100f;
        }

        public static string Concatenate<T>(this IEnumerable<T> source, string delimiter)
        {
            var s = new StringBuilder();
            var first = true;
            foreach (var t in source)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    s.Append(delimiter);
                }
                s.Append(t);
            }
            return s.ToString();
        }

        public static List<int> AllIndexesOf(string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            var indexes = new List<int>();
            for (var index = 0;; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static string GetResponseText(this string address)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Proxy = null;
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                var encoding = Encoding.GetEncoding(response.CharacterSet);

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, encoding))
                    return reader.ReadToEnd();
            }
        }

        public static string Post(this string address, Dictionary<string, string> data )
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "POST";
            request.Proxy = null;
            request.ContentType = "application/x-www-form-urlencoded";
            var postData = data.Aggregate("", (current, pair) => current + pair.Key+ "=" + pair.Value.ToBase64URL() + "&");
            postData = postData.Substring(0, postData.Length - 1);
            
            var byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();


            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var encoding = Encoding.GetEncoding(response.CharacterSet);

                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream, encoding))
                    return reader.ReadToEnd();
            }
        }

        public static string ToBase64URL(this string toEncode)
        {
            var toEncodeAsBytes
                  = Encoding.Default.GetBytes(toEncode);
            var returnValue
                  = Convert.ToBase64String(toEncodeAsBytes);
            return HttpUtility.UrlEncode(returnValue);
        }

        public static bool IsHealthlyConsumable(this IItem i)
        {
            return i.Id == 2003 || i.Id == 2009 || i.Id == 2010;
        }

        public static bool IsHealthlyConsumable(this ItemId i)
        {
            return (int)i == 2003 || (int)i == 2009 || (int)i == 2010;
        }

        public static bool IsElixir(this IItem i)
        {
            return i.Id == 2138 || i.Id == 2139 || i.Id == 2140;
        }

        public static bool IsElixir(this ItemId i)
        {
            return (int)i == 2138 || (int)i == 2139 || (int)i == 2140;
        }

        public static bool IsHPotion(this ItemId i)
        {
            return (int) i == 2003 || (int) i == 2009 || (int) i == 2010 || (int) i == 2031;
        }

        public static int GetItemSlot(this IItem it)
        {
            BrutalItemInfo.GetItemSlot(it.Id);
            return -1;
        }

        public static float GetDmg(this SpellSlot slot)
        {
            return 1;
        }

        public static Vector3 Away(this Vector3 myPos, Vector3 threatPos, float range, float add = 200,
            float resolution = 40)
        {
            var r = threatPos.Extend(myPos, range).To3D();
            var re = threatPos.Extend(myPos, range + add).To3D();
            if (!NavMesh.GetCollisionFlags(re).HasFlag(CollisionFlags.Wall)) return r;
            for (var i = 1; i < resolution; i++)
            {
                if (
                    !NavMesh.GetCollisionFlags(re.RotatedAround(threatPos, 3.14f/resolution*i))
                        .HasFlag(CollisionFlags.Wall)) return r.RotatedAround(threatPos, 3.14f/resolution*i);
                if (
                    !NavMesh.GetCollisionFlags(re.RotatedAround(threatPos, 3.14f/resolution*i*-1f))
                        .HasFlag(CollisionFlags.Wall)) return r.RotatedAround(threatPos, 3.14f/resolution*i*-1f);
            }
            return r;
        }

        public static Vector3 Copy(this Vector3 from)
        {
            return new Vector3(from.X, from.Y, from.Z);
        }

        public static Vector3[] Copy(this Vector3[] from)
        {
            var ar = new Vector3[from.Length];
            for (var i = 0; i < ar.Length; i++)
            {
                ar[i] = from[i].Copy();
            }
            return ar;

        }
    }
}
