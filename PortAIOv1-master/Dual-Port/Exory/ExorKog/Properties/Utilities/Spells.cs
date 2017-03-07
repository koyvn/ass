using EloBuddy;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;

 namespace ExorAIO.Champions.KogMaw
{
    /// <summary>
    ///     The spells class.
    /// </summary>
    internal class Spells
    {
        /// <summary>
        ///     Sets the spells.
        /// </summary>
        public static void Initialize()
        {
            Vars.Q = new Spell(SpellSlot.Q, 1175f);
            Vars.W = new Spell(SpellSlot.W, Vars.AARange + (60f + 30f * GameObjects.Player.Spellbook.GetSpell(SpellSlot.W).Level));
            Vars.E = new Spell(SpellSlot.E, 1280f);
            Vars.R = new Spell(SpellSlot.R, 900f + 300f * GameObjects.Player.Spellbook.GetSpell(SpellSlot.R).Level);

            Vars.Q.SetSkillshot(0.25f, 70f, 1650f, true, SkillshotType.SkillshotLine);
            Vars.E.SetSkillshot(0.25f, 120f, 1350f, false, SkillshotType.SkillshotLine);
            Vars.R.SetSkillshot(1.2f, 120f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }
    }
}