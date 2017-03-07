﻿using System;
using System.Linq;
using LeagueSharp.Common;
using EloBuddy.SDK.Menu.Values;

 namespace Activators.Summoners
{
    class snowball : CoreSum
    {
        internal override string Name => "summonersnowball";
        internal override string DisplayName => "Mark";
        internal override float Range => 1500f;
        internal override int Duration => 100;

        private static Spell mark;

        public snowball()
        {
            mark = new Spell(Player.GetSpellSlot(Name), Range);
            mark.SetSkillshot(0f, 60f, 1500f, true, SkillshotType.SkillshotLine);
        }

        public override void OnTick(EventArgs args)
        {
            if (!Menu["use" + Name].Cast<CheckBox>().CurrentValue || !mark.IsReady())
                return;

            if (Player.GetSpell(mark.Slot).Name.ToLower() != Name)
                return;

            foreach (var tar in Activator.Heroes.Where(hero => hero.Player.LSIsValidTarget(Range)))
            {
                if (Activator.smenu[Parent.UniqueMenuId + "useon" + tar.Player.NetworkId].Cast<CheckBox>().CurrentValue)
                    mark.CastIfHitchanceEquals(tar.Player, EloBuddy.SDK.Enumerations.HitChance.Medium);
            }
        }
    }
}
