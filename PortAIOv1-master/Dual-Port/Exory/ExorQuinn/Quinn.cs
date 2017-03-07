using System;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using LeagueSharp.SDK.Core.Utils;
using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using LeagueSharp.Data;
using LeagueSharp.Data.DataTypes;

namespace ExorAIO.Champions.Quinn
{
    /// <summary>
    ///     The champion class.
    /// </summary>
    internal class Quinn
    {
        /// <summary>
        ///     Loads Quinn.
        /// </summary>
        public void OnLoad()
        {
            /// <summary>
            ///     Initializes the menus.
            /// </summary>
            Menus.Initialize();

            /// <summary>
            ///     Initializes the spells.
            /// </summary>
            Spells.Initialize();

            /// <summary>
            ///     Initializes the methods.
            /// </summary>
            Methods.Initialize();

            /// <summary>
            ///     Initializes the drawings.
            /// </summary>
            Drawings.Initialize();
        }
        
        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void OnUpdate(EventArgs args)
        {
            if (GameObjects.Player.IsDead)
            {
                return;
            }

            if (GameObjects.Player.Spellbook.IsAutoAttacking ||
                GameObjects.Player.LSIsRecalling() ||
                Vars.R.Instance.Name.Equals("QuinnRFinale"))
            {
                return;
            }

            /// <summary>
            ///     Initializes the Automatic actions.
            /// </summary>
            Logics.Automatic(args);

            /// <summary>
            ///     Initializes the Killsteal events.
            /// </summary>
            Logics.Killsteal(args);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Logics.Combo(args);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Logics.Harass(args);
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Logics.Clear(args);
            }
        }

        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe &&
                AutoAttack.IsAutoAttack(args.SData.Name))
            {
                /// <summary>
                ///     Initializes the orbwalkingmodes.
                /// </summary>
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Logics.Weaving(sender, args);
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    Logics.JungleClear(sender, args);
                }
            }
        }

        /// <summary>
        ///     Fired on an incoming gapcloser.
        /// </summary>
        /// <param name="sender">The object.</param>
        /// <param name="args">The <see cref="Events.GapCloserEventArgs" /> instance containing the event data.</param>
        public static void OnGapCloser(object sender, Events.GapCloserEventArgs args)
        {
            if (Vars.E.IsReady() &&
                args.Sender.LSIsValidTarget(Vars.E.Range) &&
                !Invulnerable.Check(args.Sender, DamageType.Physical, false) &&
                Vars.getCheckBoxItem(Vars.EMenu, "gapcloser"))
            {
                Vars.E.CastOnUnit(args.Sender);
            }
        }

        /// <summary>
        ///     Called on interruptable spell.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Events.InterruptableTargetEventArgs" /> instance containing the event data.</param>
        public static void OnInterruptableTarget(object sender, Events.InterruptableTargetEventArgs args)
        {
            if (Vars.E.IsReady() &&
                args.Sender.LSIsValidTarget(Vars.E.Range) &&
                !Invulnerable.Check(args.Sender, DamageType.Physical, false) &&
                Vars.getCheckBoxItem(Vars.EMenu, "interrupter"))
            {
                Vars.E.CastOnUnit(args.Sender);
            }
        }

        public static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            /// <summary>
            ///     Check for R Instance.
            /// </summary>
            if (Vars.R.Instance.Name.Equals("QuinnRFinale"))
            {
                args.Process = false;
            }

            /// <summary>
            ///     The Target Forcing Logic.
            /// </summary>
            if (args.Target is AIHeroClient &&
                            Vars.GetRealHealth(args.Target as AIHeroClient) >
                                GameObjects.Player.GetAutoAttackDamage(args.Target as AIHeroClient) * 3)
            {
                if (GameObjects.EnemyHeroes.Any(
                    t =>
                        t.IsValidTarget(Vars.AARange) &&
                        t.HasBuff("quinnw")))
                {
                    args.Process = false;
                    Orbwalker.ForcedTarget = GameObjects.EnemyHeroes.Where(
                        t =>
                            t.IsValidTarget(Vars.AARange) &&
                            t.HasBuff("quinnw")).OrderByDescending(
                                o =>
                                    Data.Get<ChampionPriorityData>().GetPriority(o.ChampionName)).First();
                    return;
                }

                Orbwalker.ForcedTarget = null;
            }
        }
    }
}