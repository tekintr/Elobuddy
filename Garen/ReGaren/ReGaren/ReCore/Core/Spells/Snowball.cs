using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using ReGaren.ReCore.ConfigList;
using ReGaren.ReCore.Managers;
using ReGaren.ReCore.Utility;
using System;
using System.Collections.Generic;

namespace ReGaren.ReCore.Core.Spells
{
    class Snowball : ISpell
    {
        public void Execute()
        {
            Obj_AI_Base target = TargetSelector.GetTarget(SummonerManager.Snowball.Range, DamageType.True);
            if (target == null || !target.IsValid()) return;
            var prediction = SummonerManager.Snowball.GetPrediction(target);
            if (prediction.HitChancePercent >= 75)
                SummonerManager.Snowball.Cast(prediction.CastPosition);
        }

        public bool ShouldGetExecuted()
        {
            if (!SummonerManager.Snowball.IsReady() || !MenuHelper.GetCheckBoxValue(Summoners.Menu, "Summoners.Snowball.Status") || SummonerManager.Snowball.Name.ToLower().Contains("snowballfollowupcast"))
                return false;
            return true;
        }

        public void OnDraw()
        {

        }

        public void OnEndScene()
        {

        }
    }
}
