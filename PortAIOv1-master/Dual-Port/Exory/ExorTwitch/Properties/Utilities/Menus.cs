using EloBuddy.SDK.Menu.Values;
using ExorAIO.Utilities;

 namespace ExorAIO.Champions.Twitch
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
            Vars.QMenu = Vars.Menu.AddSubMenu("Use Q to:");
            {
                Vars.QMenu.Add("combo", new CheckBox("Combo", true));
                Vars.QMenu.Add("logical", new CheckBox("Logical", true));
                Vars.QMenu.Add("buildings", new Slider("Buildings / If Mana >= x%", 50, 0, 101));
                Vars.QMenu.Add("jungleclear", new Slider("JungleClear / if Mana >= x%", 50, 0, 101));
            }

            /// <summary>
            ///     Sets the menu for the W.
            /// </summary>
            Vars.WMenu = Vars.Menu.AddSubMenu("Use W to:");
            {
                Vars.WMenu.Add("combo", new CheckBox("Combo", true));
                Vars.WMenu.Add("harass", new Slider("Harass / if Mana >= x%", 50, 0, 101));
                Vars.WMenu.Add("clear", new Slider("Clear / if Mana >= x%", 50, 0, 101));
            }

            /// <summary>
            ///     Sets the menu for the E.
            /// </summary>
            Vars.EMenu = Vars.Menu.AddSubMenu("Use E to:");
            {
                Vars.EMenu.Add("logical", new CheckBox("Logical", true));
                Vars.EMenu.Add("killsteal", new CheckBox("KillSteal", true));
                Vars.EMenu.Add("junglesteal", new CheckBox("JungleSteal", true));
                Vars.EMenu.Add("laneclear", new Slider("LaneClear / if Mana >= x%", 50, 0, 101));
            }

            /// <summary>
            ///     Sets the miscellaneous menu.
            /// </summary>
            Vars.MiscMenu = Vars.Menu.AddSubMenu("Miscellaneous");
            {
                Vars.MiscMenu.Add("stealthtime", new Slider("Stay in stealth-mode for at least x (ms) [1000 ms = 1 second]", 0, 0, 8000));
            }

            /// <summary>
            ///     Sets the drawings menu.
            /// </summary>
            Vars.DrawingsMenu = Vars.Menu.AddSubMenu("Drawings");
            {
                Vars.DrawingsMenu.Add("w", new CheckBox("W Range"));
                Vars.DrawingsMenu.Add("e", new CheckBox("E Range"));
                Vars.DrawingsMenu.Add("r", new CheckBox("R Range"));
            }
        }
    }
}