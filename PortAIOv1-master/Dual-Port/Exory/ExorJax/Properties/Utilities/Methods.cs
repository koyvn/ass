using EloBuddy;
using LeagueSharp;
using LeagueSharp.SDK;

 namespace ExorAIO.Champions.Jax
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
            Game.OnUpdate += Jax.OnUpdate;
            Obj_AI_Base.OnSpellCast += Jax.OnDoCast;
            Events.OnGapCloser += Jax.OnGapCloser;
        }
    }
}