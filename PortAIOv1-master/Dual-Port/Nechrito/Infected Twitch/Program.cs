﻿#region

using System;
using LeagueSharp.SDK;

#endregion

 namespace Infected_Twitch
{
    internal class Program
    {

        public static void Load()
        {
            if (GameObjects.Player.ChampionName != "Twitch") return;

            LoadAssembly.OnGameLoad();
        }
    }
}
