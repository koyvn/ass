using EloBuddy;
using LeagueSharp;

 namespace ExorAIO.Champions.Evelynn
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal class Methods
    {
        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public static void Initialize()
        {
            Game.OnUpdate += Evelynn.OnUpdate;
        }
    }
}