using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using EloBuddy;
using EloBuddy.SDK;

namespace ExorAIO.Champions.Ryze
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Clear(EventArgs args)
        {
            if (Bools.HasSheenBuff())
            {
                return;
            }

            /// <summary>
            ///     The LaneClear Q Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                GameObjects.Player.ManaPercent >
                    ManaManager.GetNeededMana(Vars.Q.Slot, Vars.getSliderItem(Vars.QMenu, "laneclear")) &&
                Vars.getSliderItem(Vars.QMenu, "laneclear") != 101)
            {
                foreach (var minion in Targets.Minions.Where(m => m.IsValidTarget(Vars.Q.Range)))
                {
                    if (minion.HasBuff("RyzeE") &&
                        Vars.GetRealHealth(minion) >
                            (float)GameObjects.Player.GetSpellDamage(minion, SpellSlot.E) &&
                        Vars.GetRealHealth(minion) <
                            (float)GameObjects.Player.GetSpellDamage(minion, SpellSlot.Q) * (1 + (minion.HasBuff("RyzeE")
                                ? new double[] { 40, 55, 70, 85, 100 }[GameObjects.Player.Spellbook.GetSpell(SpellSlot.E).Level - 1] / 100
                                : 0)))
                    {
                        Vars.Q.Cast(minion);
                    }
                }
            }

            /// <summary>
            ///     The LaneClear E Logic.
            /// </summary>
            if (Vars.E.IsReady() &&
                GameObjects.Player.ManaPercent >
                    ManaManager.GetNeededMana(Vars.E.Slot, Vars.getSliderItem(Vars.EMenu, "laneclear")) &&
                Vars.getSliderItem(Vars.EMenu, "laneclear") != 101)
            {
                foreach (var minion in Targets.Minions.Where(m => m.IsValidTarget(Vars.E.Range)))
                {
                    if (minion.HasBuff("RyzeE") ||
                        (Vars.GetRealHealth(minion) <
                            (float)GameObjects.Player.GetSpellDamage(minion, SpellSlot.E) &&
                        Vars.GetRealHealth(minion) >
                            (float)GameObjects.Player.GetAutoAttackDamage(minion)))
                    {
                        Vars.E.CastOnUnit(minion);
                        return;
                    }

                    Vars.E.CastOnUnit(Targets.Minions.First(m => m.IsValidTarget(Vars.E.Range)));
                }
            }

            foreach (var minion in Targets.JungleMinions)
            {
                /// <summary>
                ///     The JungleClear E Logic.
                /// </summary>
                if (Targets.JungleMinions.Any(m => !m.HasBuff("RyzeE")))
                {
                    if (Vars.E.IsReady() &&
                        minion.IsValidTarget(Vars.E.Range) &&
                        !GameObjects.JungleSmall.Contains(minion) &&
                        GameObjects.Player.ManaPercent >
                            ManaManager.GetNeededMana(Vars.E.Slot, Vars.getSliderItem(Vars.EMenu, "jungleclear")) &&
                        Vars.getSliderItem(Vars.EMenu, "jungleclear") != 101)
                    {
                        Vars.E.CastOnUnit(minion);
                    }
                }
                else
                {
                    /// <summary>
                    ///     The JungleClear Q Logic.
                    /// </summary>
                    if (Vars.Q.IsReady() &&
                        minion.IsValidTarget(Vars.Q.Range) &&
                        Vars.GetRealHealth(minion) >
                            (float)GameObjects.Player.GetSpellDamage(minion, SpellSlot.E) &&
                        GameObjects.Player.ManaPercent >
                            ManaManager.GetNeededMana(Vars.Q.Slot, Vars.getSliderItem(Vars.QMenu, "jungleclear")) &&
                        Vars.getSliderItem(Vars.QMenu, "jungleclear") != 101)
                    {
                        Vars.Q.Cast(minion.ServerPosition);
                    }
                }
            }
        }
    }
}