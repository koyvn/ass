﻿using System; using EloBuddy.SDK.Menu.Values; using EloBuddy;
using Activators.Base;
using LeagueSharp;
using LeagueSharp.Common;

 namespace Activators.Spells.Evaders
{
    class fizzjump : CoreSpell
    {
        internal override string Name => "fizzjump";
        internal override string DisplayName => "Playful / Trickster | E";
        internal override float Range => float.MaxValue;
        internal override MenuType[] Category => new[] { MenuType.SelfMuchHP, MenuType.Zhonyas, MenuType.SelfMinMP };
        internal override int DefaultHP => 30;
        internal override int DefaultMP => 45;

        public override void OnTick(EventArgs args)
        {
            if (!Menu["use" + Name].Cast<CheckBox>().CurrentValue || !IsReady())
                return;

            if (Player.Mana/Player.MaxMana*100 <
                Menu["selfminmp" + Name + "pct"].Cast<Slider>().CurrentValue)
                return;

            foreach (var hero in Activator.Allies())
            {
                if (hero.Player.NetworkId == Player.NetworkId)
                {
                    if (Activator.amenu[Parent.UniqueMenuId + "useon" + hero.Player.NetworkId] == null)
                        continue;
                    if (!Activator.amenu[Parent.UniqueMenuId + "useon" + hero.Player.NetworkId].Cast<CheckBox>().CurrentValue)
                        continue;

                    if (Menu["use" + Name + "norm"].Cast<CheckBox>().CurrentValue)
                        if (hero.IncomeDamage > 0 && hero.HitTypes.Contains(HitType.Danger))
                            UseSpellTowards(Game.CursorPos);

                    if (Menu["use" + Name + "ulti"].Cast<CheckBox>().CurrentValue)
                        if (hero.IncomeDamage > 0 && hero.HitTypes.Contains(HitType.Ultimate))
                            UseSpellTowards(Game.CursorPos);

                    if (hero.IncomeDamage / Player.MaxHealth * 100 >=
                        Menu["selfmuchhp" + Name + "pct"].Cast<Slider>().CurrentValue)
                            UseSpellTowards(Game.CursorPos);
                }
            }
        }
    }
}
