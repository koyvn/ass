﻿ namespace ElRengarRevamped
{
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using LeagueSharp.Common;

    public class Standards
    {
        #region Static Fields

        protected static readonly Dictionary<Spells, Spell> spells = new Dictionary<Spells, Spell>
                                                                         {
                                                                             {
                                                                                 Spells.Q,
                                                                                 new Spell(
                                                                                 SpellSlot.Q,
                                                                                 Orbwalking.GetRealAutoAttackRange(
                                                                                     Player) + 100)
                                                                             },
                                                                             {
                                                                                 Spells.W,
                                                                                 new Spell(
                                                                                 SpellSlot.W,
                                                                                 500 + Player.BoundingRadius)
                                                                             },
                                                                             {
                                                                                 Spells.E,
                                                                                 new Spell(
                                                                                 SpellSlot.E,
                                                                                 1000 + Player.BoundingRadius)
                                                                             },
                                                                             { Spells.R, new Spell(SpellSlot.R, 2000) }
                                                                         };

        public static int LastSwitch;

        protected static SpellSlot Ignite;

        protected static SpellSlot Smite;

        protected static Items.Item Youmuu;

        #endregion

        #region Public Properties

        public static int Ferocity => (int)ObjectManager.Player.Mana;

        public static bool HasPassive => Player.Buffs.Any(x => x.Name.ToLower().Contains("rengarpassivebuff"));

        public static AIHeroClient Player => ObjectManager.Player;

        public static bool RengarR => Player.Buffs.Any(x => x.Name.ToLower().Contains("RengarRBuff"));

        public static string ScriptVersion => typeof(Rengar).Assembly.GetName().Version.ToString();

        #endregion

        #region Public Methods and Operators

        public static float IgniteDamage(AIHeroClient target)
        {
            if (Ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(Ignite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);
        }
        #endregion
    }
}