﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;
using SharpDX;
using Color = System.Drawing.Color;
using EloBuddy.SDK;

namespace HeavenStrikeRyze
{
    public static class Jungle
    {
        
        private static AIHeroClient Player { get { return ObjectManager.Player; } }
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {

            if (!(Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)))
                return;

            if (Player.Mana * 100 / Player.MaxMana > Program.ManaJungClear)
            {
                var targetq = MinionManager.GetMinions(Player.Position, Program._q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.Health).FirstOrDefault();
                var target = MinionManager.GetMinions(Player.Position, 600, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.Health).FirstOrDefault();
                if (Program._q2.IsReady() && Program.QjungClear)
                {
                    if (targetq != null)
                    {
                        Program._q2.Cast(targetq);
                    }
                }
                else if (Program._e.IsReady() && Program.EjungClear)
                {
                    if (target != null)
                    {
                        Program._e.Cast(target);
                    }
                }
                else if (Program._w.IsReady() && Program.WjungClear)
                {
                    if (target != null)
                    {
                        Program._w.Cast(target);
                    }
                }
            }
        }
    }
}
