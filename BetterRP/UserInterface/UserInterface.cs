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
        private Dictionary<string, int> connectionTextTimer = new Dictionary<string, int> { };

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
                        Log.Debug($"Configurating display info for player: {player.Nickname} with pseudo name: {player.DisplayNickname} and role: {player.Role}", Plugin.Instance.Config.Debug);
                        int lines = 11;
                        string[] display = new string[] { };
                        display.Concat(this.AbilitiesCheck(player));
                        display.Concat(this.TocCheck(player, ref nukeNotifSwitcher));
                        display.Concat(this.VentCheck(player));

                        if (display.Length > 0)
                        {
                            display.Append("\n");
                        }

                        for (int i = 0; i < lines - display.Length; i++)
                        {
                            display.Prepend("\n ");
                        }

                        if (this.connectionTextTimer.TryGetValue(player.UserId, out int seconds))
                        {
                            if (seconds > 7)
                            {
                                this.connectionTextTimer[player.UserId]--;
                                continue;
                            }
                            else if (seconds > 0)
                            {
                                this.connectionTextTimer[player.UserId]--;
                                display.Append(Pattern.Replace("%PARAMNAME", "ПОЛЬЗОВАТЕЛЬ").Replace("%VALUE", player.Nickname));
                            }
                            else
                            {
                                display.Append("\n");
                            }
                        }
                        else
                        {
                            this.connectionTextTimer.Add(player.UserId, 14);
                            continue;
                        }

                        display.Append(Pattern.Replace("%PARAMNAME", Plugin.Instance.Config.NameType[player.Role]).Replace("%VALUE", player.DisplayNickname));
                        if (player.Team != Team.SCP)
                        {
                            display.Append(Pattern.Replace("%PARAMNAME", "ДОЛЖНОСТЬ").Replace(
                                   "%VALUE",
                                   (player.CustomInfo == string.Empty || player.CustomInfo == null) ? Plugin.Instance.Config.RoleNames[player.Role].First().Value : player.CustomInfo).Replace(
                                "%UNIT", player.UnitName));
                        }

                        for (int i = 0; i < display.Length; i++)
                        {
                            display[i] = this.stringStart + display[i].Replace("%COLOR", Plugin.Instance.Config.ColorsPerRoles[player.Role]) + this.stringEnd;
                        }

                        string result = string.Join(string.Empty, display);
                        Log.Debug($"Total count of symbols/rows: {result.Length}/{display.Length}.", Plugin.Instance.Config.Debug);
                        player.ShowHint(result, 1.5f);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"An error occured while trying to display player info:\n{e}");
                }

                yield return MEC.Timing.WaitForSeconds(1f);
            }

            this.connectionTextTimer.Clear();
            API.PlayerNames.ClassDperRound.Clear();
        }

        private string[] AbilitiesCheck(Player player)
        {
            string[] result = new string[] { };
            if (player.IsGodModeEnabled)
            {
                result.Append(Pattern.Replace("%PARAMNAME", "GODMOD").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            if (player.NoClipEnabled)
            {
                result.Append(Pattern.Replace("%NOCLIP", "GODMOD").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            if (player.IsBypassModeEnabled)
            {
                result.Append(Pattern.Replace("%BYPASS", "GODMOD").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            return result;
        }

        private string[] TocCheck(Player player, ref bool showNukeWarning)
        {
            string[] result = new string[] { };
            if (player.CurrentRoom.Type == RoomType.Surface)
            {
                if (player.Team == Team.MTF && player.Role != RoleType.FacilityGuard)
                {
                    if (Warhead.IsInProgress)
                    {
                        if (showNukeWarning)
                        {
                            result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#990000").Replace("%VALUE", "В ЗОНЕ ПОРАЖЕНИЯ БОЕГОЛОВКИ"));
                        }
                        else if (Commands.TOC.TOC.OnEvacuateCooldownMTF < UnityEngine.Time.time && player.Role == RoleType.NtfCaptain && Warhead.DetonationTimer < (Plugin.Instance.Config.TimeInTheWay / 2) + 18f)
                        {
                            result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ТРАНСПОРТ ДОСТУПЕН"));
                        }
                        else
                        {
                            result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#6aa84f").Replace("%VALUE", string.Empty));
                        }

                        showNukeWarning = !showNukeWarning;
                    }
                    else if (Commands.TOC.TOC.OnEvacuateCooldownMTF < UnityEngine.Time.time && player.Role == RoleType.NtfCaptain)
                    {
                        result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ТРАНСПОРТ ДОСТУПЕН"));
                    }
                }
                else if (player.Team == Team.CHI)
                {
                    if (player.Role == RoleType.ChaosMarauder)
                    {
                        if (Commands.TOC.TOC.OnEvacuateCooldownCHI < UnityEngine.Time.time)
                        {
                            result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ТРАНСПОРТ ДОСТУПЕН"));
                        }
                    }
                }
            }
            else
            {
                if (player.Team == Team.MTF && player.Role != RoleType.FacilityGuard)
                {
                    if (player.Role == RoleType.NtfCaptain)
                    {
                        result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#990000").Replace("%VALUE", "НЕТ СИГНАЛА"));
                    }
                }
                else if (player.Team == Team.CHI)
                {
                    if (player.Role == RoleType.ChaosMarauder)
                    {
                        if (player.CurrentRoom.Type != RoomType.Surface)
                        {
                            result.Append(Pattern.Replace("%POSTNAME", "TOC").Replace("%COLOR", "#990000").Replace("%VALUE", "НЕТ СИГНАЛА"));
                        }
                    }
                }
            }

            return result;
        }

        private string[] VentCheck(Player player)
        {
            string[] result = new string[] { };
            if (player.Role == RoleType.Scp173 && player.GetEffect(EffectType.Asphyxiated).Intensity > 0)
            {
                result.Append(Pattern.Replace("%POSTNAME", "ВЕНТИЛЯЦИЯ").Replace("%COLOR", "#6aa84f").Replace("%VALUE", "ДОСТУПНО"));
            }

            return result;
        }
    }
}
