﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Marksman.Common;
using EloBuddy;

 namespace Marksman.Champions
{
    internal static class PlayerSpells
    {
        public static void CastIfHitchanceGreaterOrEqual(this Spell spell, AIHeroClient t)
        {
            var nPrediction = spell.GetPrediction(t);
            var nHitPosition = nPrediction.CastPosition.LSExtend(ObjectManager.Player.Position, -130);
            if (nPrediction.HitChance >= spell.GetHitchance())
            {
                spell.Cast(nHitPosition);
            }
            
        }
    }
}
