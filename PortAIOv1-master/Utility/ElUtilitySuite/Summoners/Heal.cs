 namespace ElUtilitySuite.Summoners
{
    using System;
    using System.Drawing;
    using System.Linq;

    using ElUtilitySuite.Vendor.SFX;
    using System.Runtime.CompilerServices;

    using EloBuddy;
    using LeagueSharp.Common;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public class Heal : IPlugin
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the heal spell.
        /// </summary>
        /// <value>
        ///     The heal spell.
        /// </value>
        public Spell HealSpell { get; set; }

        /// <summary>
        /// The Menu
        /// </summary>
        public Menu Menu { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player.
        /// </summary>
        /// <value>
        ///     The player.
        /// </value>
        private AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        #endregion

        public static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Creates the menu.
        /// </summary>
        /// <param name="rootMenu">The root menu.</param>
        /// <returns></returns>
        /// 

        public void CreateMenu(Menu rootMenu)
        {
            if (this.Player.GetSpellSlot("summonerheal") == SpellSlot.Unknown)
            {
                return;
            }

            this.Menu = rootMenu.AddSubMenu("Heal", "Heal");
            {
                this.Menu.Add("Heal.Activated", new CheckBox("Heal"));
                this.Menu.Add("PauseHealHotkey", new KeyBind("Don't use heal key", false, KeyBind.BindTypes.HoldActive, 'L'));
                this.Menu.Add("min-health", new Slider("Health percentage", 20, 1));
                this.Menu.Add("min-damage", new Slider("Heal on % incoming damage", 20, 1));
                foreach (var x in HeroManager.Allies)
                {
                    this.Menu.Add($"healon{x.ChampionName}", new CheckBox("Use for " + x.ChampionName));
                }
            }
        }


        /// <summary>
        ///     Loads this instance.
        /// </summary>
        public void Load()
        {
            try
            {
                var healSlot = this.Player.GetSpellSlot("summonerheal");

                if (healSlot == SpellSlot.Unknown)
                {
                    return;
                }
                
                this.HealSpell = new Spell(healSlot, 850);
                Game.OnUpdate += this.OnUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"An error occurred: '{0}'", e);
            }
        }

        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        /// <param name="args">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void OnUpdate(EventArgs args)
        {
            try
            {
                if (this.Player.IsDead || !this.HealSpell.IsReady() || this.Player.InFountain() || this.Player.LSIsRecalling())
                {
                    return;
                }

                if (!this.Menu["Heal.Activated"].Cast<CheckBox>().CurrentValue || this.Menu["PauseHealHotkey"].Cast<KeyBind>().CurrentValue)
                {
                    return;
                }

                foreach (var ally in HeroManager.Allies)
                {
                    if (!this.Menu[string.Format("healon{0}", ally.ChampionName)].Cast<CheckBox>().CurrentValue || ally.LSIsRecalling() || ally.IsInvulnerable || ally.HasBuff("ChronoShift"))
                    {
                        return;
                    }

                    var enemies = ally.LSCountEnemiesInRange(750f);
                    var totalDamage = IncomingDamageManager.GetDamage(ally) * 1.1f;

                    if (ally.HealthPercent <= this.Menu["min-health"].Cast<Slider>().CurrentValue && (this.HealSpell.IsInRange(ally) || ally.IsMe) && enemies >= 1 && !ally.IsDead)
                    {
                        if (ally.HealthPercent < getSliderItem(this.Menu, "min-health"))
                        {
                            this.Player.Spellbook.CastSpell(this.HealSpell.Slot);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(@"An error occurred: '{0}'", e);
            }
        }

        #endregion
    }
}
