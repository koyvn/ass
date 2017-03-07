﻿using System; using EloBuddy.SDK.Menu.Values; using EloBuddy;
using Activators.Base;
using LeagueSharp;
using LeagueSharp.Common;

 namespace Activators.Spells.Health
{
    class chronoshift : CoreSpell
    {
        internal override string Name => "chronoshift";
        internal override string DisplayName => "Chronoshift | R";
        internal override float Range => 900f;
        internal override MenuType[] Category => new[] { MenuType.SelfLowHP };
        internal override int DefaultHP => 20;
        internal override int DefaultMP => 0;

        public override void OnTick(EventArgs args)
        {
            if (!Menu["use" + Name].Cast<CheckBox>().CurrentValue || !IsReady())
                return;

            foreach (var hero in Activator.Allies())
            {
                if (hero.Player.LSDistance(Player.ServerPosition) <= Range)
                {
                    if (Activator.amenu[Parent.UniqueMenuId + "useon" + hero.Player.NetworkId] == null)
                        continue;
                    if (!Activator.amenu[Parent.UniqueMenuId + "useon" + hero.Player.NetworkId].Cast<CheckBox>().CurrentValue)
                        continue;

                    if (!Player.HasBuffOfType(BuffType.Invulnerability))
                    {
                        if (hero.Player.Health/hero.Player.MaxHealth*100 <=
                            Menu["selflowhp" + Name + "pct"].Cast<Slider>().CurrentValue)
                        {
                            if (hero.IncomeDamage > 0)
                                UseSpellOn(hero.Player);
                        }
                    }
                }
            }
        }
    }
}
