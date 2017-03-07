﻿using System;
using Activators.Base;
using LeagueSharp;
using LeagueSharp.Common;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK;

 namespace Activators.Items.Offensives
{
    class _3748 : CoreItem
    {
        internal override int Id => 3748;
        internal override int Priority => 5;
        internal override string Name => "Titanic";
        internal override string DisplayName => "Titanic Hydra";
        internal override int Duration => 100;
        internal override float Range => 385f;
        internal override MenuType[] Category => new[] { MenuType.SelfLowHP, MenuType.EnemyLowHP };
        internal override MapType[] Maps => new[] { MapType.Common };
        internal override int DefaultHP => 95;
        internal override int DefaultMP => 0;

        public _3748()
        {
            Orbwalker.OnPostAttack += Orbwalking_AfterAttack;
        }

        private void Orbwalking_AfterAttack(AttackableUnit target, EventArgs args)
        {
            if (Player.ChampionName == "Riven")
                return;

            if (!Menu["use" + Name].Cast<CheckBox>().CurrentValue || !IsReady())
                return;

            var hero = target as AIHeroClient;
            if (hero.LSIsValidTarget(Range))
            {
                if (Activator.omenu[Activator.omenu.UniqueMenuId + "useon" + hero.NetworkId] == null)
                {
                    return;
                }

                if (!Activator.omenu[Activator.omenu.UniqueMenuId + "useon" + hero.NetworkId].Cast<CheckBox>().CurrentValue)
                    return;

                if (hero.Health / hero.MaxHealth * 100 <= Menu["enemylowhp" + Name + "pct"].Cast<Slider>().CurrentValue)
                {
                    UseItem(Tar.Player, true);
                }

                if (Player.Health / Player.MaxHealth * 100 <= Menu["selflowhp" + Name + "pct"].Cast<Slider>().CurrentValue)
                {
                    UseItem(Tar.Player, true);
                }
            }
        }

        public override void OnTick(EventArgs args)
        {

        }
    }
}
