namespace BetterRP.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Display additional info for players: names, roles, rp-names, abilities, etc.
    /// </summary>
    public class UserInterface
    {
        private static readonly string Pattern = "<color=#C1B5B5>%PARAMNAME<pos=-7%>  :  </color><color=#%COLOR>%VALUE</color>";

        private static readonly List<RoleType> IgnoredRoles = new List<RoleType> { RoleType.None, RoleType.Spectator, RoleType.Scp096, RoleType.Scp079 };
        private readonly string stringStart = "<align=left><pos=-21%><b><size=20>";
        private readonly string stringEnd = "</size><b></pos></align>\n";
        private readonly Plugin plugin;

        /// <summary>
        /// Gets or sets amounts of second for display dealy.
        /// </summary>
        public static Dictionary<string, int> ConnectionTextTimer { get; set; } = new Dictionary<string, int> { };

        /// <summary>
        /// Activates user interface.
        /// </summary>
        public void OnRoundStarted()
        {
            Log.Debug("Starting DisplayInterface coroutine on round start.", Plugin.Instance.Config.Debug);
            Plugin.Instance.NewCoroutine(this.DisplayInterface());
        }

        /// <summary>
        /// Coroutine which uses hint display to show additional player info.
        /// </summary>
        /// <returns>Coroutine.</returns>
        public IEnumerator<float> DisplayInterface()
        {
            Log.Debug("DisplayInterface coroutine has been started.", Plugin.Instance.Config.Debug);
            bool nukeNotifSwitcher = false;
            while (Round.IsStarted)
            {
                try
                {
                    foreach (var player in Player.List.Where(x => x != null && !IgnoredRoles.Contains(x.Role)))
                    {
                        Log.Debug($"Configurating display info for player: {player.Nickname} with pseudo name: {player.DisplayNickname}, role: {player.Role} and custom info {player.CustomInfo ?? "null"}.", Plugin.Instance.Config.Debug);
                        List<string> display = new List<string> { };
                        display.AddRange(this.AbilitiesCheck(player));

                        Log.Debug($"Total count of rows after ability check: {display.Count}.", Plugin.Instance.Config.Debug);
                        if ((player.Team == Team.MTF && player.Role != RoleType.FacilityGuard) || player.Team == Team.CHI)
                        {
                            display.Add(this.TocCheck(player, ref nukeNotifSwitcher));
                        }

                        Log.Debug($"Total count of rows after toc check: {display.Count}.", Plugin.Instance.Config.Debug);
                        display.AddRange(this.VentCheck(player));

                        Log.Debug($"Total count of rows after vent check: {display.Count}.", Plugin.Instance.Config.Debug);
                        if (display.Count > 0)
                        {
                            display.Add("\n ");

                            Log.Debug($"Additional rows detected. Added \\n symbol. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                        }

                        Log.Debug($" Added \\n symbols for moving display lower. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                        if (ConnectionTextTimer.TryGetValue(player.UserId, out int seconds))
                        {
                            if (seconds > 7)
                            {
                                ConnectionTextTimer[player.UserId]--;

                                Log.Debug($"Started display delay detected. Seconds remaining: {ConnectionTextTimer[player.UserId]}. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                                continue;
                            }
                            else if (seconds > 0)
                            {
                                ConnectionTextTimer[player.UserId]--;
                                display.Add(Pattern.Replace("%PARAMNAME", "ПОЛЬЗОВАТЕЛЬ").Replace("%VALUE", player.Nickname));

                                Log.Debug($"Started name display. detected. Seconds remaining: {ConnectionTextTimer[player.UserId]}. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                            }
                            else
                            {
                                Log.Debug($"Skipping name display. Seconds remaining: {ConnectionTextTimer[player.UserId]}. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                            }
                        }
                        else
                        {
                            ConnectionTextTimer.Add(player.UserId, 14);

                            Log.Debug($"New player detected. Seconds remaining: {ConnectionTextTimer[player.UserId]}. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                            continue;
                        }

                        display.Add(Pattern.Replace("%PARAMNAME", Plugin.Instance.Config.NameType[player.Role]).Replace("%VALUE", player.DisplayNickname));

                        Log.Debug($"Adding NameType '{Plugin.Instance.Config.NameType[player.Role]}' and Role '{player.DisplayNickname}'. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                        if (player.Team != Team.SCP)
                        {
                            display.Add(Pattern.Replace("%PARAMNAME", "ДОЛЖНОСТЬ").Replace(
                                   "%VALUE",
                                   (player.CustomInfo == string.Empty || player.CustomInfo == null) ? Plugin.Instance.Config.RoleNames[player.Role].First().Value : player.CustomInfo).Replace(
                                "%UNIT", player.UnitName));

                            Log.Debug($"Adding Post '{((player.CustomInfo == string.Empty || player.CustomInfo == null) ? Plugin.Instance.Config.RoleNames[player.Role].First().Value : player.CustomInfo).Replace("%UNIT", player.UnitName)}'. Total count of rows: {display.Count}.", Plugin.Instance.Config.Debug);
                        }

                        for (int i = 0; i < 20 - display.Count; i++)
                        {
                            display.Insert(0, "\n ");
                        }

                        for (int i = 0; i < display.Count; i++)
                        {
                            display[i] = this.stringStart + display[i].Replace("%COLOR", Plugin.Instance.Config.ColorsPerRoles[player.Role]) + this.stringEnd;
                        }

                        Log.Debug($"Color has been changed to {Plugin.Instance.Config.ColorsPerRoles[player.Role]}. Total count of rows: {display.Count}", Plugin.Instance.Config.Debug);
                        string result = string.Join(string.Empty, display);

                        Log.Debug($"Total count of symbols/rows: {result.Length}/{display.Count}.\n___________________________________________________________", Plugin.Instance.Config.Debug);
                        player.ShowHint(result, 1.5f);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"An error occured while trying to display player info:\n{e}");
                }

                yield return MEC.Timing.WaitForSeconds(1f);
            }

            ConnectionTextTimer.Clear();
            API.PlayerNames.ClassDperRound.Clear();
        }

        private List<string> AbilitiesCheck(Player player)
        {
            List<string> result = new List<string> { };
            if (player.IsGodModeEnabled)
            {
                result.Add(Pattern.Replace("%PARAMNAME", "GODMOD").Replace("%COLOR", "6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            if (player.NoClipEnabled)
            {
                result.Add(Pattern.Replace("%PARAMNAME", "NOCLIP").Replace("%COLOR", "6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            if (player.IsBypassModeEnabled)
            {
                result.Add(Pattern.Replace("%PARAMNAME", "BYPASS").Replace("%COLOR", "6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            Log.Debug($"Total count of ability rows: {result.Count}.", Plugin.Instance.Config.Debug);
            return result;
        }

        private string TocCheck(Player player, ref bool showNukeWarning)
        {
            string result = string.Empty;
            if (Plugin.Instance.Config.RoomsWithConnToTOC.Contains(player.CurrentRoom.Type))
            {
                if (Plugin.Instance.Config.DirectTOC.Contains(player.Role))
                {
                    if ((player.Team == Team.MTF ? Commands.TOC.TOC.OnTheWayMTF : Commands.TOC.TOC.OnTheWayCHI) > UnityEngine.Time.time)
                    {
                        result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "0180DA").Replace("%VALUE", "В ПУТИ");
                    }
                    else if ((player.Team == Team.MTF ? Commands.TOC.TOC.OnSupportCooldownMTF : Commands.TOC.TOC.OnSupportCooldownCHI) > UnityEngine.Time.time)
                    {
                        result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "CE7E00").Replace("%VALUE", "СБОР");
                    }
                    else if (!Warhead.IsDetonated &&
                        (((!Warhead.IsInProgress ||
                        Warhead.DetonationTimer > (Plugin.Instance.Config.TimeInTheWay / 2) + 18f) && player.Team == Team.MTF) || player.Team == Team.CHI))
                    {
                        result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "6aa84f").Replace("%VALUE", "ТРАНСПОРТ ДОСТУПЕН");
                    }
                    else
                    {
                        result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "990000").Replace("%VALUE", string.Empty);
                    }
                }

                if (Warhead.IsInProgress)
                {
                    if (showNukeWarning && (Plugin.Instance.Config.WarheadWarning.Contains(player.Role) ||
                        (Plugin.Instance.Config.DirectTOC.Contains(player.Role) && Warhead.IsDetonated)))
                    {
                        result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "990000").Replace("%VALUE", "В ЗОНЕ ПОРАЖЕНИЯ БОЕГОЛОВКИ");
                    }
                    else if (Plugin.Instance.Config.WarheadWarning.Contains(player.Role) &&
                        !Plugin.Instance.Config.DirectTOC.Contains(player.Role))
                    {
                        result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "990000").Replace("%VALUE", string.Empty);
                    }

                    showNukeWarning = !showNukeWarning;
                }
            }
            else if (Plugin.Instance.Config.DirectTOC.Contains(player.Role))
            {
                result = Pattern.Replace("%PARAMNAME", "TOC").Replace("%COLOR", "990000").Replace("%VALUE", "НЕТ СИГНАЛА");
            }

            Log.Debug($"Total count of toc rows: {result.Length}.", Plugin.Instance.Config.Debug);
            return result;
        }

        private List<string> VentCheck(Player player)
        {
            List<string> result = new List<string> { };
            if (player.Role == RoleType.Scp173 && player.GetEffect(EffectType.Asphyxiated).Intensity > 0)
            {
                result.Add(Pattern.Replace("%PARAMNAME", "ВЕНТИЛЯЦИЯ").Replace("%COLOR", "6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            Log.Debug($"Total count of vent rows: {result.Count}.", Plugin.Instance.Config.Debug);
            return result;
        }
    }
}
