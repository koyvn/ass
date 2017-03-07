﻿using System;
using DZLib.Core;
using LeagueSharp;
using LeagueSharp.Common;
using VayneHunter_Reborn.External;
using VayneHunter_Reborn.Skills.Tumble.VHRQ;
using VayneHunter_Reborn.Utility;
using VayneHunter_Reborn.Utility.MenuUtility;
using ActiveGapcloser = VayneHunter_Reborn.External.ActiveGapcloser;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

 namespace VayneHunter_Reborn.Skills.Condemn
{
    class InterrupterGapcloser
    {
        public static void OnLoad()
        {
            Interrupter2.OnInterruptableTarget += OnInterruptableTarget;
            //   CustomAntigapcloser.OnEnemyGapcloser += CustomAntigapcloser_OnEnemyGapcloser;
            DZAntigapcloserVHR.OnEnemyGapcloser += OnEnemyGapcloser;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            GameObject.OnCreate += GameObject_OnCreate;
        }

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


        private static void OnEnemyGapcloser(ActiveGapcloser gapcloser, SpellSlot slot)
        {
            if (getCheckBoxItem(MenuGenerator.miscMenu, "dz191.vhr.misc.general.antigp") && Variables.spells[slot].IsReady())
            {
                LeagueSharp.Common.Utility.DelayAction.Add(getSliderItem(MenuGenerator.miscMenu, "dz191.vhr.misc.general.antigpdelay"),
                    () =>
                    {
                        if (gapcloser.Sender.LSIsValidTarget(Variables.spells[SpellSlot.E].Range) &&
                            getCheckBoxItem(MenuGenerator.miscMenu, "dz191.vhr.misc.general.antigp")
                            && (Variables.spells[slot].IsReady()))
                        {
                            switch (slot)
                            {
                                case SpellSlot.Q:
                                    var senderPos = gapcloser.End;
                                    var backOut = ObjectManager.Player.ServerPosition.LSExtend(senderPos, 300f);
                                    if (backOut.IsSafe())
                                    {
                                        if (gapcloser.Start.LSDistance(ObjectManager.Player.ServerPosition) >
                                            gapcloser.End.LSDistance(ObjectManager.Player.ServerPosition))
                                        {
                                            Variables.spells[SpellSlot.Q].Cast(backOut);
                                        }
                                    }

                                    break;

                                case SpellSlot.E:
                                    if (gapcloser.Start.LSDistance(ObjectManager.Player.ServerPosition) >
                                        gapcloser.End.LSDistance(ObjectManager.Player.ServerPosition))
                                    {
                                        Variables.spells[SpellSlot.E].CastOnUnit(gapcloser.Sender);
                                    }
                                    break;
                            }
                        }
                    });
            }
        }

        private static void OnInterruptableTarget(AIHeroClient sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (getCheckBoxItem(MenuGenerator.miscMenu, "dz191.vhr.misc.general.interrupt"))
            {
                if (args.DangerLevel == Interrupter2.DangerLevel.High && sender.LSIsValidTarget(Variables.spells[SpellSlot.E].Range))
                {
                    Variables.spells[SpellSlot.E].CastOnUnit(sender);
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

            if (sender is AIHeroClient)
            {
                var s2 = (AIHeroClient)sender;
                if (s2.LSIsValidTarget() && s2.ChampionName == "Pantheon" && s2.GetSpellSlot(args.SData.Name) == SpellSlot.W)
                {
                    if (getCheckBoxItem(MenuGenerator.miscMenu, "dz191.vhr.misc.general.antigp") && args.Target.IsMe && Variables.spells[SpellSlot.E].IsReady())
                    {
                        if (s2.LSIsValidTarget(Variables.spells[SpellSlot.E].Range))
                        {
                            Variables.spells[SpellSlot.E].CastOnUnit(s2);
                        }
                    }
                }
            }

        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (getCheckBoxItem(MenuGenerator.miscMenu, "dz191.vhr.misc.general.antigp") && Variables.spells[SpellSlot.E].IsReady())
            {
                if (sender.IsEnemy && sender.Name == "Rengar_LeapSound.troy")
                {
                    var rengarEntity = HeroManager.Enemies.Find(h => h.ChampionName.Equals("Rengar") && h.LSIsValidTarget(Variables.spells[SpellSlot.E].Range));
                    if (rengarEntity != null)
                    {
                        Variables.spells[SpellSlot.E].CastOnUnit(rengarEntity);
                    }
                }
            }
        }
    }
}
