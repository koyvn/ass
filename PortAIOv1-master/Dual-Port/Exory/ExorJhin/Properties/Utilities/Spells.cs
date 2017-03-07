using EloBuddy;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;

 namespace ExorAIO.Champions.Jhin
{
    /// <summary>
    ///     The settings class.
    /// </summary>
    internal class Spells
    {
        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public static void Initialize()
        {
            Vars.Q = new Spell(SpellSlot.Q, Vars.AARange);
            Vars.W = new Spell(SpellSlot.W, 2500f);
            Vars.E = new Spell(SpellSlot.E, 750f);
            Vars.R = new Spell(SpellSlot.R, 3500f);

            Vars.W.SetSkillshot(0.75f, 40f, 5000f, false, SkillshotType.SkillshotLine);
            Vars.E.SetSkillshot(1f, 260f, 1000f, false, SkillshotType.SkillshotCircle);
            Vars.R.SetSkillshot(0.25f, 80f, 5000f, false, SkillshotType.SkillshotLine);
        }
    }
}