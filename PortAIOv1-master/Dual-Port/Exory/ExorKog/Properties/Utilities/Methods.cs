using EloBuddy;
using LeagueSharp;
using LeagueSharp.SDK;

 namespace ExorAIO.Champions.KogMaw
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
            Game.OnUpdate += KogMaw.OnUpdate;
            Events.OnGapCloser += KogMaw.OnGapCloser;
        }
    }
}