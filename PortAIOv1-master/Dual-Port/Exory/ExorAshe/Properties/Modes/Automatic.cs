using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace ExorAIO.Champions.Ashe
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
        public static void Automatic(EventArgs args)
        {
            if (GameObjects.Player.LSIsRecalling())
            {
                return;
            }

            /// <summary>
            ///     The Automatic W Logic.
            /// </summary>
            if (Vars.W.IsReady() &&
                Vars.getCheckBoxItem(Vars.WMenu, "logical"))
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        Bools.IsImmobile(t) &&
                        !Invulnerable.Check(t) &&
                        t.LSIsValidTarget(Vars.W.Range)))
                {
                    if (!Vars.W.GetPrediction(target).CollisionObjects.Any())
                    {
                        Vars.W.Cast(Vars.W.GetPrediction(target).UnitPosition);
                    }
                }
            }

            /// <summary>
            ///     The Automatic E Logic.
            /// </summary>
            if (Vars.E.IsReady() &&
                GameObjects.Player.Spellbook.GetSpell(SpellSlot.E).Ammo >=
                    (Vars.getCheckBoxItem(Vars.EMenu, "logical") ? 2 : 1) &&
                Vars.getCheckBoxItem(Vars.EMenu, "vision"))
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.None) &&
                    GameObjects.Player.CountEnemyHeroesInRange(1000f) == 0 &&
                    GameObjects.EnemyHeroes.Count(
                        x =>
                            !x.IsDead &&
                            !x.IsVisible) >= 3)
                {
                    Vars.E.Cast(Vars.Locations
                        .Where(d => GameObjects.Player.Distance(d) > 1500f)
                        .OrderBy(d2 => GameObjects.Player.Distance(d2))
                        .FirstOrDefault());
                }

                foreach (var target in GameObjects.EnemyHeroes)
                {
                    if (target.Distance(target.GetWaypoints().Last()) < 1500 &&
                        !NavMesh.IsWallOfGrass(GameObjects.Player.ServerPosition, 1) &&
                        NavMesh.IsWallOfGrass((Vector3)target.GetWaypoints().Last(), 1) &&
                        GameObjects.Player.Distance(target.GetWaypoints().Last()) > 1000)
                    {
                        Vars.E.Cast(target.GetWaypoints().Last());
                    }
                }
            }

            /// <summary>
            ///     The E -> R Combo Logics.
            /// </summary>
            if (Vars.R.IsReady() &&
                Vars.getCheckBoxItem(Vars.RMenu, "bool") &&
                Vars.getKeyBindItem(Vars.RMenu, "key") &&
                !Invulnerable.Check(Targets.Target, DamageType.Magical, false) &&
                Vars.getCheckBoxItem(Vars.WhiteListMenu, Targets.Target.ChampionName.ToLower()))
            {
                if (!Vars.R.GetPrediction(Targets.Target).CollisionObjects.Any())
                {
                    if (Vars.E.IsReady() &&
                        Vars.getCheckBoxItem(Vars.EMenu, "logical"))
                    {
                        Vars.E.Cast(Vars.E.GetPrediction(Targets.Target).UnitPosition);
                    }

                    Vars.R.Cast(Vars.R.GetPrediction(Targets.Target).UnitPosition);
                }
            }

            /// <summary>
            ///     The Semi-Automatic R Management.
            /// </summary>
            if (Vars.R.IsReady() &&
                Vars.getCheckBoxItem(Vars.RMenu, "bool") &&
                Vars.getKeyBindItem(Vars.RMenu, "key"))
            {
                if (!GameObjects.EnemyHeroes.Any(
                    t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(Vars.R.Range) &&
                        Vars.getCheckBoxItem(Vars.WhiteListMenu, Targets.Target.ChampionName.ToLower())))
                {
                    return;
                }

                Vars.R.Cast(Vars.R.GetPrediction(
                    GameObjects.EnemyHeroes.Where(
                        t =>
                            !Invulnerable.Check(t) &&
                            t.IsValidTarget(Vars.R.Range) &&
                            Vars.getCheckBoxItem(Vars.WhiteListMenu, Targets.Target.ChampionName.ToLower())).OrderBy(o => o.Health).First()).UnitPosition);
            }
        }
    }
}