using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using ExorAIO.Utilities;
using LeagueSharp.Data.Enumerations;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using System.Linq;

namespace ExorAIO.Champions.Sivir
{
    /// <summary>
    ///     The menu class.
    /// </summary>
    internal class Menus
    {
        /// <summary>
        ///     Sets the menu.
        /// </summary>
        public static void Initialize()
        {
            /// <summary>
            ///     Sets the menu for the Q.
            /// </summary>
            Vars.QMenu = Vars.Menu.AddSubMenu("Use Q to:", "q");
            {
                Vars.QMenu.Add("combo", new CheckBox("Combo", true));
                Vars.QMenu.Add("killsteal", new CheckBox("KillSteal", true));
                Vars.QMenu.Add("logical", new CheckBox("Logical", true));
                Vars.QMenu.AddLabel("Set the sliders to 101 below to disable them.");
                Vars.QMenu.Add("harass", new Slider("Harass / if Mana >= x%", 50, 0, 101));
                Vars.QMenu.Add("clear", new Slider("Clear / if Mana >= x%", 50, 0, 101));
            }

            /// <summary>
            ///     Sets the menu for the W.
            /// </summary>
            Vars.WMenu = Vars.Menu.AddSubMenu("Use W to:", "w");
            {
                Vars.WMenu.Add("combo", new CheckBox("Combo", true));
                Vars.QMenu.AddLabel("Set the sliders to 101 below to disable them.");
                Vars.WMenu.Add("clear", new Slider("Clear / if Mana >= x%", 50, 0, 101));
                Vars.WMenu.Add("buildings", new Slider("Buildings / if Mana >= x%", 50, 0, 101));
            }

            /// <summary>
            ///     Sets the menu for the E.
            /// </summary>
            Vars.EMenu = Vars.Menu.AddSubMenu("Use E to:", "e");
            {
                Vars.EMenu.AddLabel("It has to be used in conjunction with Evade, else it will not shield Skillshots");
                Vars.EMenu.AddLabel("It it meant to shield what Evade doesn't support, like targetted spells, AoE, etc.");
                Vars.EMenu.Add("logical", new CheckBox("Logical", true));
                Vars.EMenu.Add("delay", new Slider("E Delay (ms)", 0, 0, 250));
            }

            /// <summary>
            ///     Sets the menu for the E Whitelist.
            /// </summary>
            Vars.WhiteListMenu = Vars.Menu.AddSubMenu("Shield: Whitelist Menu");
            {
                Vars.WhiteListMenu.Add("minions", new CheckBox("Shield: Dragon/Baron Attacks"));
                foreach (var enemy in GameObjects.EnemyHeroes)
                {
                    if (enemy.ChampionName.Equals("Alistar"))
                    {
                        Vars.WhiteListMenu.Add($"{enemy.ChampionName.ToLower()}.pulverize", new CheckBox($"Shield: {enemy.ChampionName}'s Q"));
                    }

                    if (enemy.ChampionName.Equals("Braum"))
                    {
                        Vars.WhiteListMenu.Add($"{enemy.ChampionName.ToLower()}.braumbasicattackpassiveoverride", new CheckBox($"Shield: {enemy.ChampionName}'s Passive"));
                    }

                    if (enemy.ChampionName.Equals("Jax"))
                    {
                        Vars.WhiteListMenu.Add($"{enemy.ChampionName.ToLower()}.jaxcounterstrike", new CheckBox($"Shield: {enemy.ChampionName}'s E", true));
                    }

                    if (enemy.ChampionName.Equals("KogMaw"))
                    {
                        Vars.WhiteListMenu.Add($"{enemy.ChampionName.ToLower()}.kogmawicathiansurprise", new CheckBox($"Shield: {enemy.ChampionName}'s Passive", true));
                    }

                    if (enemy.ChampionName.Equals("Udyr"))
                    {
                        Vars.WhiteListMenu.Add($"{enemy.ChampionName.ToLower()}.udyrbearattack", new CheckBox($"Shield: {enemy.ChampionName}'s E"));
                    }

                    foreach (var spell in SpellDatabase.Get().Where(
                        s =>
                            !s.SpellName.Equals("KatarinaE") &&
                            !s.SpellName.Equals("TalonCutthroat") &&
                            s.ChampionName.Equals(enemy.ChampionName)))
                    {
                        if ((enemy.IsMelee &&
                            spell.CastType.Contains(CastType.Activate) &&
                            spell.SpellType.HasFlag(SpellType.Activated) &&
                            AutoAttack.IsAutoAttackReset(spell.SpellName)) ||

                            ((spell.SpellType.HasFlag(SpellType.Targeted) ||
                            spell.SpellType.HasFlag(SpellType.TargetedMissile)) &&
                            spell.CastType.Contains(CastType.EnemyChampions)))
                        {
                            Vars.WhiteListMenu.Add($"{enemy.ChampionName.ToLower()}.{spell.SpellName.ToLower()}", new CheckBox($"Shield: {enemy.ChampionName}'s {spell.Slot}"));
                        }
                    }
                }
            }

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            Vars.DrawingsMenu = Vars.Menu.AddSubMenu("Drawings");
            {
                Vars.DrawingsMenu.Add("q", new CheckBox("Q Range"));
            }
        }
    }
}