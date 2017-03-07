using EloBuddy;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;

 namespace ExorAIO.Champions.DrMundo
{
    /// <summary>
    ///     The spell class.
    /// </summary>
    internal class Spells
    {
        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public static void Initialize()
        {
            Vars.Q = new Spell(SpellSlot.Q, 1000f);
            Vars.W = new Spell(SpellSlot.W, GameObjects.Player.BoundingRadius * 2 + 162.5f);
            Vars.E = new Spell(SpellSlot.E, GameObjects.Player.BoundingRadius + 150f);
            Vars.R = new Spell(SpellSlot.R);

            Vars.Q.SetSkillshot(0.25f, 60f, 1850f, true, SkillshotType.SkillshotLine);
        }
    }
}