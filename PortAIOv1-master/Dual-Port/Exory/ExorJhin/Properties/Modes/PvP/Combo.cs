using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using EloBuddy;
using LeagueSharp.SDK.Core.Utils;
using Geometry = ExorAIO.Utilities.Geometry;

 namespace ExorAIO.Champions.Jhin
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Combo(EventArgs args)
        {
            if (Bools.HasSheenBuff() ||
				!Targets.Target.LSIsValidTarget() ||
                Invulnerable.Check(Targets.Target) ||
                Vars.R.Instance.Name.Equals("JhinRShot"))
            {
                return;
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Targets.Target.LSIsValidTarget(Vars.Q.Range) &&
                GameObjects.Player.HasBuff("JhinPassiveReload") &&
                Vars.getCheckBoxItem(Vars.QMenu, "combo"))
            {
                Vars.Q.CastOnUnit(Targets.Target);
            }

            /// <summary>
            ///     The Automatic W Logic.
            /// </summary>
            if (Vars.W.IsReady() &&
                !GameObjects.Player.IsUnderEnemyTurret() &&
                Vars.getCheckBoxItem(Vars.WMenu, "combo"))
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        !Invulnerable.Check(t) &&
                        t.HasBuff("jhinespotteddebuff") &&
                        t.LSIsValidTarget(Vars.W.Range - 150f) &&
                        Vars.getCheckBoxItem(Vars.WhiteListMenu, t.ChampionName.ToLower())))
                {
                    Vars.W.Cast(Vars.W.GetPrediction(target).UnitPosition);
                }
            }
        }
    }
}