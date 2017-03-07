﻿ namespace ElXerath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    using Color = System.Drawing.Color;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Enumerations;
    internal enum Spells
    {
        Q,

        W,

        E,

        R
    }

    internal class Xerath
    {
        #region Static Fields

        public static Dictionary<Spells, LeagueSharp.Common.Spell> spells = new Dictionary<Spells, LeagueSharp.Common.Spell>
                                                             {
                                                                 { Spells.Q, new LeagueSharp.Common.Spell(SpellSlot.Q, 1600) },
                                                                 { Spells.W, new LeagueSharp.Common.Spell(SpellSlot.W, 1000) },
                                                                 { Spells.E, new LeagueSharp.Common.Spell(SpellSlot.E, 1150) },
                                                                 { Spells.R, new LeagueSharp.Common.Spell(SpellSlot.R, 3200) }
                                                             };

        private static SpellSlot _ignite;

        private static int lastNotification;

        #endregion

        #region Public Properties

        public static bool CastingR
        {
            get
            {
                return ObjectManager.Player.HasBuff("XerathLocusOfPower2")
                       || (ObjectManager.Player.LastCastedSpellName() == "XerathLocusOfPower2"
                           && Environment.TickCount - ObjectManager.Player.LastCastedSpellT() < 500);
            }
        }

        #endregion

        #region Properties

        private static EloBuddy.SDK.Enumerations.HitChance CustomHitChance
        {
            get
            {
                return GetHitchance();
            }
        }

        private static AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static void Game_OnGameLoad()
        {
            if (ObjectManager.Player.CharData.BaseSkinName != "Xerath")
            {
                return;
            }

            spells[Spells.Q].SetCharged(750, 1550, 1.5f, 0.6f, int.MaxValue, 100);
            spells[Spells.W].SetSkillshot(0.25f, 100f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            spells[Spells.E].SetSkillshot(0.25f, 60f, 1400f, true, SkillshotType.SkillshotLine);
            spells[Spells.R].SetSkillshot(0.5f, 120f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            _ignite = Player.GetSpellSlot("summonerdot");

            ElXerathMenu.Initialize();
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += Drawings.Drawing_OnDraw;
            EloBuddy.Player.OnIssueOrder += AIHeroClient_OnIssueOrder;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Game.OnWndProc += Game_OnWndProc;
        }

        #endregion

        #region Methods

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

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (!gapcloser.Sender.LSIsValidTarget(spells[Spells.E].Range)
                || gapcloser.Sender.LSDistance(ObjectManager.Player) > spells[Spells.E].Range)
            {
                return;
            }

            if (gapcloser.Sender.LSIsValidTarget(spells[Spells.E].Range)
                && (getCheckBoxItem(ElXerathMenu.miscMenu, "ElXerath.misc.Antigapcloser") && spells[Spells.E].IsReady()))
            {
                spells[Spells.E].Cast(gapcloser.Sender);
            }
        }

        private static void AutoHarassMode()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].ChargedMaxRange, DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(
                spells[Spells.W].Range + spells[Spells.W].Width * 0.5f,
                DamageType.Magical);

            if (target == null || !target.LSIsValidTarget())
            {
                return;
            }

            if (getKeyBindItem(ElXerathMenu.hMenu, "ElXerath.AutoHarass"))
            {
                var q = getCheckBoxItem(ElXerathMenu.hMenu, "ElXerath.UseQAutoHarass");
                var w = getCheckBoxItem(ElXerathMenu.hMenu, "ElXerath.UseWAutoHarass");
                var mana = getSliderItem(ElXerathMenu.hMenu, "ElXerath.harass.mana");

                if (Player.ManaPercent < mana)
                {
                    return;
                }

                if (q && spells[Spells.Q].IsReady() && target.LSIsValidTarget(spells[Spells.Q].ChargedMaxRange))
                {
                    if (!spells[Spells.Q].IsCharging)
                    {
                        spells[Spells.Q].StartCharging();
                        return;
                    }

                    if (spells[Spells.Q].IsCharging)
                    {
                        var pred = spells[Spells.Q].GetPrediction(target);
                        if (pred.HitChance >= CustomHitChance)
                        {
                            spells[Spells.Q].Cast(target);
                        }
                    }
                }
                if (wTarget != null && w && spells[Spells.W].IsReady())
                {
                    var pred = spells[Spells.W].GetPrediction(wTarget);
                    if (pred.HitChance >= CustomHitChance)
                    {
                        spells[Spells.W].Cast(wTarget);
                    }
                }
            }
        }

        private static void CastR(Obj_AI_Base target)
        {
            var useR = getCheckBoxItem(ElXerathMenu.rMenu, "ElXerath.R.AutoUseR");
            var tapkey = getKeyBindItem(ElXerathMenu.rMenu, "ElXerath.R.OnTap");
            var ultRadius = getSliderItem(ElXerathMenu.rMenu, "ElXerath.R.Radius");
            var drawROn = getCheckBoxItem(ElXerathMenu.miscMenu, "ElXerath.Draw.RON");

            if (!useR)
            {
                return;
            }

            if (target == null || !target.LSIsValidTarget())
            {
                return;
            }

            var ultType = getBoxItem(ElXerathMenu.rMenu, "ElXerath.R.Mode");

            switch (ultType)
            {
                case 0:
                    spells[Spells.R].Cast(target);
                    break;

                case 1:
                    var d = getSliderItem(ElXerathMenu.rMenu, "Delay" + (RCombo._index + 1));
                    if (Utils.TickCount - RCombo.CastSpell > d)
                    {
                        spells[Spells.R].Cast(target);
                    }
                    break;

                case 2:
                    //if (tapkey)
                    if (RCombo._tapKey)
                    {
                        spells[Spells.R].Cast(target);
                    }
                    break;

                case 3:
                    if (spells[Spells.R].GetPrediction(target).HitChance >= CustomHitChance)
                    {
                        spells[Spells.R].Cast(target);
                    }

                    break;

                case 4:

                    if (Game.CursorPos.LSDistance(target.ServerPosition) < ultRadius
                        && ObjectManager.Player.LSDistance(target.ServerPosition) < spells[Spells.R].Range)
                    {
                        spells[Spells.R].Cast(target);
                    }

                    if (drawROn)
                    {
                        Render.Circle.DrawCircle(Game.CursorPos, ultRadius, Color.White);
                    }

                    break;
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].ChargedMaxRange, DamageType.Magical);
            if (!target.LSIsValidTarget())
            {
                return;
            }

            var comboQ = getCheckBoxItem(ElXerathMenu.cMenu, "ElXerath.Combo.Q");
            var comboW = getCheckBoxItem(ElXerathMenu.cMenu, "ElXerath.Combo.W");
            var comboE = getCheckBoxItem(ElXerathMenu.cMenu, "ElXerath.Combo.E");

            if (comboE && spells[Spells.E].IsReady() && spells[Spells.E].IsInRange(target))
            {
                spells[Spells.E].Cast(target);
            }

            if (comboW && spells[Spells.W].IsReady())
            {
                var prediction = spells[Spells.W].GetPrediction(target);
                if (prediction.HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
                {
                    spells[Spells.W].Cast(target);
                }
            }

            var predictionQ = spells[Spells.Q].GetPrediction(target);
            if (comboQ && spells[Spells.Q].IsReady() && target.LSIsValidTarget(spells[Spells.Q].ChargedMaxRange))
            {
                if (!spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].StartCharging();
                    return;
                }

                if (spells[Spells.Q].Range == spells[Spells.Q].ChargedMaxRange)
                {
                    spells[Spells.Q].Cast(target);
                }
                else
                {
                    if (Player.IsInRange(predictionQ.UnitPosition + 200 * (predictionQ.UnitPosition - Player.ServerPosition).Normalized(), spells[Spells.Q].Range))
                    {
                        if (spells[Spells.Q].Cast(predictionQ.CastPosition))
                        {
                            return;
                        }
                    }
                }
            }

            if (Player.LSDistance(target) <= 600 && IgniteDamage(target) >= target.Health
                && getCheckBoxItem(ElXerathMenu.miscMenu, "ElXerath.Ignite"))
            {
                Player.Spellbook.CastSpell(_ignite, target);
            }
        }

        //Thanks to Esk0r for the R
        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint)WindowsMessages.WM_KEYUP)
            {
                RCombo._tapKey = true;
            }
        }

        private static EloBuddy.SDK.Enumerations.HitChance GetHitchance()
        {
            switch (getBoxItem(ElXerathMenu.miscMenu, "ElXerath.hitChance"))
            {
                case 0:
                    return EloBuddy.SDK.Enumerations.HitChance.Low;
                case 1:
                    return EloBuddy.SDK.Enumerations.HitChance.Medium;
                case 2:
                    return EloBuddy.SDK.Enumerations.HitChance.High;
                case 3:
                    return EloBuddy.SDK.Enumerations.HitChance.High;
                default:
                    return EloBuddy.SDK.Enumerations.HitChance.Medium;
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].ChargedMaxRange, DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(
                spells[Spells.W].Range + spells[Spells.W].Width * 0.5f,
                DamageType.Magical);

            if (target == null || !target.LSIsValidTarget())
            {
                return;
            }

            var harassQ = getCheckBoxItem(ElXerathMenu.hMenu, "ElXerath.Harass.Q");
            var harassW = getCheckBoxItem(ElXerathMenu.hMenu, "ElXerath.Harass.W");

            if (wTarget != null && harassW && spells[Spells.W].IsReady())
            {
                spells[Spells.W].CastIfHitchanceEquals(wTarget, CustomHitChance);
            }

            if (harassQ && spells[Spells.Q].IsReady() && spells[Spells.Q].IsInRange(target)
                && target.LSIsValidTarget(spells[Spells.Q].ChargedMaxRange))
            {
                if (!spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].StartCharging();
                    return;
                }

                if (spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].CastIfHitchanceEquals(target, CustomHitChance);
                }
            }
        }

        private static float IgniteDamage(Obj_AI_Base target)
        {
            if (_ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(_ignite) != SpellState.Ready)
            {
                return 0f;
            }
            return (float)Player.GetSummonerSpellDamage(target, LeagueSharp.Common.Damage.SummonerSpell.Ignite);
        }

        private static void JungleClear()
        {
            var clearQ = getCheckBoxItem(ElXerathMenu.lMenu, "ElXerath.jclear.Q");
            var clearW = getCheckBoxItem(ElXerathMenu.lMenu, "ElXerath.jclear.W");
            var clearE = getCheckBoxItem(ElXerathMenu.lMenu, "ElXerath.jclear.E");
            var minmana = getSliderItem(ElXerathMenu.lMenu, "minmanaclear");

            if (Player.ManaPercent < minmana)
            {
                return;
            }

            var minions = MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition,
                spells[Spells.W].Range,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (minions.Count <= 0)
            {
                return;
            }

            if (spells[Spells.Q].IsCharging)
            {
                if (minions.Max(x => x.LSDistance(Player, true)) < spells[Spells.Q].RangeSqr)
                {
                    if (minions.Max(x => x.LSDistance(Player, true)) < spells[Spells.Q].RangeSqr)
                    {
                        spells[Spells.Q].Cast(spells[Spells.Q].GetLineFarmLocation(minions).Position);
                    }
                }
            }

            if (spells[Spells.Q].IsCharging)
            {
                return;
            }

            if (spells[Spells.Q].IsReady() && clearQ)
            {
                if (spells[Spells.Q].GetLineFarmLocation(minions).MinionsHit >= 1)
                {
                    spells[Spells.Q].StartCharging();
                    return;
                }
            }

            if (spells[Spells.W].IsReady() && clearW)
            {
                var farmLocation = spells[Spells.W].GetCircularFarmLocation(minions);
                spells[Spells.W].Cast(farmLocation.Position);
            }

            if (spells[Spells.E].IsReady() && clearE)
            {
                spells[Spells.E].Cast();
            }
        }

        private static void KsMode()
        {
            var useKs = getCheckBoxItem(ElXerathMenu.miscMenu, "ElXerath.misc.ks");
            if (!useKs)
            {
                return;
            }

            var target =
                HeroManager.Enemies.FirstOrDefault(
                    x =>
                    !x.HasBuffOfType(BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield)
                    && spells[Spells.Q].CanCast(x) && (x.Health + (x.HPRegenRate / 2)) <= spells[Spells.Q].GetDamage(x));

            if (spells[Spells.Q].CanCast(target) && spells[Spells.Q].IsReady())
            {
                if (!spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].StartCharging();
                }
                if (spells[Spells.Q].IsCharging)
                {
                    spells[Spells.Q].Cast(target);
                }
            }
        }

        private static void LaneClear()
        {
            var clearQ = getCheckBoxItem(ElXerathMenu.lMenu, "ElXerath.clear.Q");
            var clearW = getCheckBoxItem(ElXerathMenu.lMenu, "ElXerath.clear.W");
            var minmana = getSliderItem(ElXerathMenu.lMenu, "minmanaclear");

            if (Player.ManaPercent < minmana)
            {
                return;
            }

            var minions = MinionManager.GetMinions(Player.ServerPosition, spells[Spells.Q].ChargedMaxRange);
            if (minions.Count <= 0)
            {
                return;
            }

            if (clearQ && spells[Spells.Q].IsReady())
            {
                if (spells[Spells.Q].IsCharging)
                {
                    var bestFarmPos = spells[Spells.Q].GetLineFarmLocation(minions);
                    if (minions.Count == minions.Count(x => Player.LSDistance(x) < spells[Spells.Q].Range)
                        && bestFarmPos.Position.IsValid() && bestFarmPos.MinionsHit > 0)
                    {
                        spells[Spells.Q].Cast(bestFarmPos.Position);
                    }
                }
                else if (minions.Count > 0)
                {
                    spells[Spells.Q].StartCharging();
                }
            }

            if (spells[Spells.W].IsReady() && clearW)
            {
                var farmLocation = spells[Spells.W].GetCircularFarmLocation(minions);
                spells[Spells.W].Cast(farmLocation.Position);
            }
        }

        private static void AIHeroClient_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            var blockMovement = getCheckBoxItem(ElXerathMenu.rMenu, "ElXerath.R.Block");
            if (CastingR && blockMovement)
            {
                args.Process = false;
            }
        }

        private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "XerathLocusOfPower2")
                {
                    RCombo.CastSpell = 0;
                    RCombo._index = 0;
                    RCombo._position = new Vector3();
                    RCombo._tapKey = false;
                }
                else if (args.SData.Name == "xerathlocuspulse")
                {
                    RCombo.CastSpell = Utils.TickCount;
                    RCombo._index++;
                    RCombo._position = args.End;
                    RCombo._tapKey = false;
                }
            }
        }
        
        private static void OnUpdate(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            var utarget = TargetSelector.GetTarget(spells[Spells.R].Range, DamageType.Magical);
            spells[Spells.R].Range = 2000 + spells[Spells.R].Level * 1200;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                LaneClear();
                JungleClear();
            }

            var showNotifications = getCheckBoxItem(ElXerathMenu.miscMenu, "ElXerath.misc.Notifications");

            if (spells[Spells.R].IsReady() && showNotifications && Environment.TickCount - lastNotification > 5000)
            {
                foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(h => h.LSIsValidTarget() && !h.IsAlly && (float)Player.GetSpellDamage(h, SpellSlot.R) * 3 > h.Health))
                {
                    Chat.Print(enemy.ChampionName + ": is killable", Color.White, 4000);
                    lastNotification = Environment.TickCount;
                }
            }

            AutoHarassMode();
            KsMode();

            if (CastingR)
            {
                CastR(utarget);
            }

            if (spells[Spells.E].IsReady())
            {
                var useE = getKeyBindItem(ElXerathMenu.miscMenu, "ElXerath.Misc.E");
                var eTarget = TargetSelector.GetTarget(spells[Spells.E].Range, DamageType.Magical);

                if (useE)
                {
                    spells[Spells.E].Cast(eTarget);
                }
            }
        }

        #endregion

        private static class RCombo
        {
            #region Static Fields

            public static int _index;

            public static Vector3 _position;

            public static bool _tapKey;

            public static int CastSpell;

            #endregion
        }
    }
}