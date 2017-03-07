using System;
using ExorAIO.Utilities;
using LeagueSharp.SDK;
using SharpDX;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.SDK.Enumerations;

 namespace ExorAIO.Champions.Sivir
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
        public static void Harass(EventArgs args)
        {
            if (!Targets.Target.LSIsValidTarget() ||
                Invulnerable.Check(Targets.Target))
            {
                return;
            }

            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Targets.Target.LSIsValidTarget(Vars.Q.Range) &&
                GameObjects.Player.ManaPercent >
                    ManaManager.GetNeededMana(Vars.Q.Slot, Vars.getSliderItem(Vars.QMenu, "harass")) &&
                Vars.getSliderItem(Vars.QMenu, "harass") != 101)
            {
                Vars.Q.Cast(Targets.Target.LSIsValidTarget(300f)
                    ? Targets.Target.ServerPosition
                    : Vars.Q.GetPrediction(Targets.Target).CastPosition.LSExtend(GameObjects.Player.ServerPosition, -140f));
            }
        }
    }
}