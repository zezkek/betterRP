namespace BetterRP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    public class PlayerNames
    {
        /// <summary>
        /// Random Number Generator.
        /// </summary>
        private readonly Random rnd = new Random();

        private List<string> surnames = new List<string>();
        private List<string> callSigns = new List<string>();

        public void OnWaiting()
        {
            Log.Debug($"Total surnames count out of handler: {Plugin.SurnameBase.Count()}", Plugin.PluginItem.Config.Debug);
            if (Plugin.SurnameBase.Count > 0)
            {
                this.surnames = Plugin.SurnameBase;
                this.surnames.ShuffleList();
                Log.Debug($"Total surnames count inside handler: {this.surnames.Count()}", Plugin.PluginItem.Config.Debug);
            }

            Log.Debug($"Total callsigns count out of handler: {Plugin.CallSignsBase.Count()}", Plugin.PluginItem.Config.Debug);
            if (Plugin.CallSignsBase.Count > 0)
            {
                this.callSigns = Plugin.CallSignsBase;
                this.callSigns.ShuffleList();
                Log.Debug($"Total callsigns count inside handler: {this.callSigns.Count()}", Plugin.PluginItem.Config.Debug);
            }
        }

        public void OnRoundStarted()
        {
            Plugin.PluginItem.NewCoroutine(this.BadgeDisplay());
        }

        private Dictionary<string, int> ConnectionTextTimer = new Dictionary<string, int> { };

        public IEnumerator<float> BadgeDisplay()
        {
            yield return MEC.Timing.WaitForSeconds(Plugin.PluginItem.Config.HintDisplayTime);
            while (Round.IsStarted)
            {
                string name = "<pos=-21%><b><size=20><color=#C1B5B5>ПОЛЬЗОВАТЕЛЬ</pos><pos=-7%>  :  </color>" +
                        "<color=%color>%name</color></pos>\n";
                string post = "<pos=-21%><b><size=20>" +
                        "<color=#C1B5B5>ДОЛЖНОСТЬ</pos>" +
                        "<pos=-7%>  :  </color><color=%color>%post</color>" +
                        "</size></pos>\n";
                string role = "<pos=-21%><b><size=20>" +
                        "<color=#C1B5B5>РОЛЬ" +
                        "<pos=-7%>  :  </color><color=%color>%role</color></size></pos>\n";
                foreach (var player in Player.List.Where(x => x != null && x.Role != RoleType.None))
                {
                    int lines = 11;
                    string display = string.Empty;
                    if (player.IsGodModeEnabled)
                    {
                        display += "<align=left><pos=-21%><b><size=20><color=#C1B5B5>РЕЖИМ БОГА</color> <color=#6aa84f>ВКЛЮЧЁН</color></b></pos></align>\n";
                        lines--;
                    }

                    if (player.NoClipEnabled)
                    {
                        display += "<align=left><pos=-21%><b><size=20><color=#C1B5B5>РЕЖИМ ПОЛЁТА</color> <color=#6aa84f>ДОСТУПЕН</color></b></pos></align>\n";
                        lines--;
                    }

                    if (player.IsBypassModeEnabled)
                    {
                        display += "<align=left><pos=-21%><b><size=20><color=#C1B5B5>РЕЖИМ ОБХОДА</color> <color=#6aa84f>ВКЛЮЧЁН</color></b></pos></align>\n";
                        lines--;
                    }

                    if (lines != 11)
                    {
                        display += "\n";
                    }

                    for (int i = 0; i < lines; i++)
                    {
                        display = "\n" + display;
                    }

                    display += "<align=left>";
                    if (this.ConnectionTextTimer.TryGetValue(player.UserId, out int seconds))
                    {
                        if (seconds > 0)
                        {
                            this.ConnectionTextTimer[player.UserId]--;
                            display += name.Replace("%name", player.Nickname);
                        }
                    }
                    else
                    {
                        this.ConnectionTextTimer.Add(player.UserId, 7);
                    }

                    switch (player.Team)
                    {
                        case Team.SCP:

                            if (player.Role == RoleType.Scp0492 && player.CustomInfo != string.Empty && player.CustomInfo != null)
                            {
                                display += post.Replace("<color=%color>%post</color>", player.CustomInfo);
                            }
                            else
                            {
                                display += role.Replace("%role", player.DisplayNickname);
                            }

                            display = display.Replace("%color", "#990000");
                            break;
                        case Team.MTF:
                            if (player.Role != RoleType.FacilityGuard)
                            {
                                display += role.Replace("%role", player.DisplayNickname).Replace("РОЛЬ", "ПОЗЫВНОЙ");
                            }
                            else
                            {
                                display += role.Replace("%role", player.DisplayNickname).Replace("РОЛЬ", "ФИО");
                            }

                            switch (player.Role)
                            {
                                case RoleType.NtfSergeant:
                                    display += post.Replace("%post", $"Сержант {player.UnitName}");
                                    display = display.Replace("%color", "#0180DA");
                                    break;
                                case RoleType.NtfCaptain:
                                    display += post.Replace("%post", $"Командир {player.UnitName}");
                                    display = display.Replace("%color", "#000080");
                                    break;
                                case RoleType.NtfPrivate:
                                    display += post.Replace("%post", $"Капрал {player.UnitName}");
                                    display = display.Replace("%color", "#8BC5FF");
                                    break;
                                case RoleType.NtfSpecialist:
                                    display += post.Replace("%post", $"Специалист ВУС {player.UnitName}");
                                    display = display.Replace("%color", "#0180DA");
                                    break;
                                case RoleType.FacilityGuard:
                                    display += post.Replace("<color=%color>%post</color>", player.CustomInfo);
                                    display = display.Replace("%color", "#757575");
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Team.CHI:
                            display += role.Replace("%role", player.DisplayNickname).Replace("РОЛЬ", "ПОЗЫВНОЙ");
                            switch (player.Role)
                            {
                                case RoleType.ChaosMarauder:
                                    display += post.Replace("%post", $"Мародёр Хаоса");
                                    display = display.Replace("%color", "#274e13");
                                    break;
                                case RoleType.ChaosRepressor:
                                    display += post.Replace("%post", $"Репрессор Хаоса");
                                    display = display.Replace("%color", "#274e13");
                                    break;
                                case RoleType.ChaosRifleman:
                                    display += post.Replace("%post", $"Стрелок Хаоса");
                                    display = display.Replace("%color", "#38761d");
                                    break;
                                case RoleType.ChaosConscript:
                                    display += post.Replace("%post", $"Новобранец Хаоса");
                                    display = display.Replace("%color", "#6aa84f");
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Team.RSC:
                            display += role.Replace("%role", player.DisplayNickname).Replace("РОЛЬ", "ФИО");
                            display += post.Replace("<color=%color>%post</color>", player.CustomInfo);
                            display = display.Replace("%color", "#FFD966");
                            break;
                        case Team.CDP:
                            display += role.Replace("%role", player.DisplayNickname).Replace("РОЛЬ", "НОМЕР");
                            display += post.Replace("%post", "Класс-D");
                            display = display.Replace("%color", "#CE7E00");
                            break;
                        case Team.TUT:
                            display += post.Replace("%post", "Обучение");
                            display = display.Replace("%color", "#6aa84f");
                            break;
                        default:
                            continue;
                    }

                    player.ShowHint(display + "</pos></size></b></align>", 1.5f);
                }

                yield return MEC.Timing.WaitForSeconds(1f);
            }

            this.ConnectionTextTimer.Clear();
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            if (ev.Killer.Role == RoleType.Scp049)
            {
                Plugin.PluginItem.NewCoroutine(this.NameChangeDelay(ev.Target));
            }

            if (this.ConnectionTextTimer.TryGetValue(ev.Target.UserId, out int seconds))
            {
                this.ConnectionTextTimer[ev.Target.UserId] = 7;
            }
            else
            {
                this.ConnectionTextTimer.Add(ev.Target.UserId, 7);
            }
        }

        public IEnumerator<float> NameChangeDelay(Player player)
        {
            yield return MEC.Timing.WaitForSeconds(10f);
            if (!player.IsAlive || player.Role != RoleType.Scp0492)
            {
                player.DisplayNickname = player.Nickname;
                player.CustomInfo = string.Empty;
            }

            yield break;
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            Plugin.PluginItem.NewCoroutine(this.SetName(ev.Player, ev.RoleType));
            if (this.ConnectionTextTimer.TryGetValue(ev.Player.UserId, out int seconds))
            {
                this.ConnectionTextTimer[ev.Player.UserId] = 7;
            }
            else
            {
                this.ConnectionTextTimer.Add(ev.Player.UserId, 7);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Plugin.PluginItem.NewCoroutine(this.SetName(ev.Player, ev.NewRole));
            if (this.ConnectionTextTimer.TryGetValue(ev.Player.UserId, out int seconds))
            {
                this.ConnectionTextTimer[ev.Player.UserId] = 7;
            }
            else
            {
                this.ConnectionTextTimer.Add(ev.Player.UserId, 7);
            }
        }

        private IEnumerator<float> SetName(Player player, RoleType role)
        {
            yield return MEC.Timing.WaitForSeconds(1f);
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && player != null && role != RoleType.None)
            {
                string selectedname;

                if (player.Team != Team.SCP)
                {
                    player.CustomInfo = string.Empty;

                    switch (role)
                    {
                        case RoleType.ClassD:
                            player.DisplayNickname = "D-" + this.rnd.Next(10000, 99999).ToString();
                            break;
                        case RoleType.Scientist:
                            if (this.surnames.Count > 0)
                            {
                                selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                                this.surnames.Remove(selectedname);
                            }
                            else
                            {
                                selectedname = player.Nickname;
                            }

                            player.DisplayNickname = "д-р " + selectedname;

                            if (player.HasItem(ItemType.KeycardFacilityManager))
                            {
                                player.CustomInfo = "<color=#FFD966>Заведующий Комплекса</color>";
                            }
                            else if (player.HasItem(ItemType.KeycardZoneManager))
                            {
                                player.CustomInfo = "<color=#FFD966>Заведующий Зоны</color>";
                            }
                            else if (player.HasItem(ItemType.KeycardResearchCoordinator))
                            {
                                player.CustomInfo = "<color=#FFD966>Координатор исследований</color>";
                            }
                            else if (player.HasItem(ItemType.KeycardContainmentEngineer))
                            {
                                player.CustomInfo = "<color=#FFD966>Инженер Комплекса</color>";
                            }
                            else
                            {
                                player.CustomInfo = "<color=#FFD966>Научный сотрудник</color>";
                            }

                            break;

                        case RoleType.FacilityGuard:
                            if (this.surnames.Count > 0)
                            {
                                selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                                this.surnames.Remove(selectedname);
                            }
                            else
                            {
                                selectedname = player.Nickname;
                            }

                            player.DisplayNickname = selectedname;

                            if (player.HasItem(ItemType.KeycardNTFCommander))
                            {
                                player.CustomInfo = "<color=#757575>Начальник СБ</color>";
                            }
                            else if (player.HasItem(ItemType.KeycardNTFLieutenant))
                            {
                                player.CustomInfo = "<color=#757575>Старший сотрудник СБ</color>";
                            }
                            else if (player.HasItem(ItemType.KeycardNTFOfficer))
                            {
                                player.CustomInfo = "<color=#757575>Сотрудник СБ</color>";
                            }
                            else
                            {
                                player.CustomInfo = "<color=#757575>Младший сотрудник СБ</color>";
                            }

                            break;

                        default:
                            if (player.Team == Team.CHI || player.Team == Team.MTF)
                            {
                                if (this.callSigns.Count > 0)
                                {
                                    selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                    this.callSigns.Remove(selectedname);
                                }
                                else
                                {
                                    selectedname = player.Nickname;
                                }

                                player.DisplayNickname = $"'{selectedname}'";
                                break;
                            }
                            else
                            {
                                player.DisplayNickname = string.Empty;
                                break;
                            }
                    }

                    Log.Debug($"Player Name: {player.Nickname}\nPlayer Nickname: {player.DisplayNickname}\nPlayer Custom Info: {player.CustomInfo}", Plugin.PluginItem.Config.Debug);

                    // player.ShowHint(message, Plugin.PluginItem.Config.HintDisplayTime);
                }
                else
                {
                    if (role != RoleType.Scp0492)
                    {
                        player.CustomInfo = string.Empty;
                        player.DisplayNickname = "[ДАННЫЕ УДАЛЕНЫ]";
                    }
                    else if (player.DisplayNickname == string.Empty || player.DisplayNickname == player.Nickname || player.DisplayNickname == null)
                    {
                        player.CustomInfo = string.Empty;
                        player.DisplayNickname = player.Nickname;
                    }
                }
            }
        }
    }
}
