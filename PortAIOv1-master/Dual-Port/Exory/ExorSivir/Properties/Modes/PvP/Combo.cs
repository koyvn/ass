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
        public static void Combo(EventArgs args)
        {
            if (Bools.HasSheenBuff() ||
                !Targets.Target.LSIsValidTarget() ||
                Invulnerable.Check(Targets.Target))
            {
                return;
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (Vars.Q.IsReady() && Targets.Target.LSIsValidTarget(Vars.Q.Range) && Vars.getCheckBoxItem(Vars.QMenu, "combo"))
            {
                Vars.Q.Cast(Targets.Target.LSIsValidTarget(300f)
                    ? Targets.Target.ServerPosition
                    : Vars.Q.GetPrediction(Targets.Target).CastPosition.LSExtend(GameObjects.Player.ServerPosition, -140f));
            }
        }
    }
}