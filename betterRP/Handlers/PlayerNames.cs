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
        private List<char> names = new List<char>
        {
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Э', 'Ю', 'Я',
        };

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

        private Dictionary<string, int> connectionTextTimer = new Dictionary<string, int> { };

        private Dictionary<string, string> militaryRole = new Dictionary<string, string> { };

        public IEnumerator<float> BadgeDisplay()
        {
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
                    if (this.connectionTextTimer.TryGetValue(player.UserId, out int seconds))
                    {
                        if (seconds > 0)
                        {
                            this.connectionTextTimer[player.UserId]--;
                            display += name.Replace("%name", player.Nickname);
                        }
                    }
                    else
                    {
                        this.connectionTextTimer.Add(player.UserId, 7);
                    }

                    switch (player.Team)
                    {
                        case Team.SCP:
                            display += role.Replace("%role", "[ДАННЫЕ УДАЛЕНЫ]");
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
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Сержант {player.UnitName}").Replace("%color", "#0180DA"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Сержант {player.UnitName}").Replace("%color", "#0180DA");
                                    }

                                    display = display.Replace("%color", "#0180DA");
                                    break;
                                case RoleType.NtfCaptain:
                                    display += post.Replace("%post", $"Командир {player.UnitName}");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Командир {player.UnitName}").Replace("%color", "#000080"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Командир {player.UnitName}").Replace("%color", "#000080");
                                    }

                                    display = display.Replace("%color", "#000080");
                                    break;
                                case RoleType.NtfPrivate:
                                    display += post.Replace("%post", $"Капрал {player.UnitName}");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Капрал {player.UnitName}").Replace("%color", "#8BC5FF"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Капрал {player.UnitName}").Replace("%color", "#8BC5FF");
                                    }

                                    display = display.Replace("%color", "#8BC5FF");
                                    break;
                                case RoleType.NtfSpecialist:
                                    display += post.Replace("%post", $"Специалист ВУС {player.UnitName}");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Специалист ВУС {player.UnitName}").Replace("%color", "#0180DA"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Специалист ВУС {player.UnitName}").Replace("%color", "#0180DA");
                                    }

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
                            string postMessage;
                            switch (player.Role)
                            {
                                case RoleType.ChaosMarauder:
                                    postMessage = post.Replace("%post", $"Мародёр Хаоса").Replace("%color", "#274e13");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Мародёр Хаоса").Replace("%color", "#274e13"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Мародёр Хаоса").Replace("%color", "#274e13");
                                    }

                                    display = display.Replace("%color", "#274e13");
                                    break;
                                case RoleType.ChaosRepressor:
                                    postMessage =  post.Replace("%post", $"Репрессор Хаоса");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Репрессор Хаоса").Replace("%color", "#274e13"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Репрессор Хаоса").Replace("%color", "#274e13");
                                    }

                                    display = display.Replace("%color", "#274e13");
                                    break;
                                case RoleType.ChaosRifleman:
                                    postMessage =  post.Replace("%post", $"Стрелок Хаоса");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Стрелок Хаоса").Replace("%color", "#38761d"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Стрелок Хаоса").Replace("%color", "#38761d");
                                    }

                                    display = display.Replace("%color", "#38761d");
                                    break;
                                case RoleType.ChaosConscript:
                                    postMessage =  post.Replace("%post", $"Новобранец Хаоса");
                                    if (!this.militaryRole.ContainsKey(player.UserId))
                                    {
                                        this.militaryRole.Add(player.UserId, post.Replace("%post", $"Новобранец Хаоса").Replace("%color", "#6aa84f"));
                                    }
                                    else
                                    {
                                        this.militaryRole[player.UserId] = post.Replace("%post", $"Новобранец Хаоса").Replace("%color", "#6aa84f");
                                    }

                                    display = display.Replace("%color", "#6aa84f");
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Team.RSC:
                            display += role.Replace("%role", player.DisplayNickname).Replace("РОЛЬ", "ФИО");
                            if (player.CustomInfo != string.Empty || player.CustomInfo != null)
                            {
                                display += post.Replace("<color=%color>%post</color>", player.CustomInfo);
                            }
                            else
                            {
                                display += post.Replace("%post", "Научный Сотрудник");
                            }

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

            this.connectionTextTimer.Clear();
        }

        //public void OnDying(DyingEventArgs ev)
        //{
        //    if (!ev.IsAllowed)
        //    {
        //        return;
        //    }

        //    if (ev.Killer.Role == RoleType.Scp049)
        //    {
        //        Plugin.PluginItem.NewCoroutine(this.NameChangeDelay(ev.Target));
        //    }

        //    if (this.connectionTextTimer.TryGetValue(ev.Target.UserId, out int seconds))
        //    {
        //        this.connectionTextTimer[ev.Target.UserId] = 7;
        //    }
        //    else
        //    {
        //        this.connectionTextTimer.Add(ev.Target.UserId, 7);
        //    }
        //}

        public IEnumerator<float> NameChangeDelay(Player player)
        {
            string customInfo = player.CustomInfo;
            string displayNickname = player.DisplayNickname;
            player.CustomInfo = string.Empty;
            player.DisplayNickname = string.Empty;
            yield return MEC.Timing.WaitForSeconds(10f);
            if (player == null)
            {
                yield break;
            }

            if (player.IsAlive && player.Role != RoleType.Scp0492)
            {
                yield break;
            }

            if (!player.IsAlive)
            {
                player.CustomInfo = string.Empty;
                player.DisplayNickname = string.Empty;
                this.militaryRole.Remove(player.UserId);
                yield break;
            }

            if (player.CustomInfo != string.Empty || player.CustomInfo != null)
            {
                player.CustomInfo = customInfo;
            }
            else if (this.militaryRole.TryGetValue(player.UserId, out string cInfo))
            {
                player.CustomInfo = cInfo;
                this.militaryRole.Remove(player.UserId);
            }

            player.DisplayNickname = displayNickname;
            yield break;
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            Plugin.PluginItem.NewCoroutine(this.SetName(ev.Player, ev.RoleType));
            if (this.connectionTextTimer.TryGetValue(ev.Player.UserId, out int seconds))
            {
                this.connectionTextTimer[ev.Player.UserId] = 7;
            }
            else
            {
                this.connectionTextTimer.Add(ev.Player.UserId, 7);
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Plugin.PluginItem.NewCoroutine(this.SetName(ev.Player, ev.NewRole));
            if (this.connectionTextTimer.TryGetValue(ev.Player.UserId, out int seconds))
            {
                this.connectionTextTimer[ev.Player.UserId] = 7;
            }
            else
            {
                this.connectionTextTimer.Add(ev.Player.UserId, 7);
            }
        }

        private IEnumerator<float> SetName(Player player, RoleType role)
        {
            yield return MEC.Timing.WaitForSeconds(0.1f);
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && player != null && role != RoleType.None)
            {
                string selectedname;

                if (player.Team != Team.SCP)
                {
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

                            player.DisplayNickname = $"д-р {selectedname} {this.names[UnityEngine.Random.Range(0, this.names.Count)]}. {this.names[UnityEngine.Random.Range(0, this.names.Count)]}.";

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

                            player.DisplayNickname = $"{selectedname} {this.names[UnityEngine.Random.Range(0, this.names.Count)]}. {this.names[UnityEngine.Random.Range(0, this.names.Count)]}.";

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
                                Plugin.PluginItem.NewCoroutine(this.NameChangeDelay(player));
                            }

                            break;
                    }

                    Log.Debug($"Player Name: {player.Nickname}\nPlayer Nickname: {player.DisplayNickname}\nPlayer Custom Info: {player.CustomInfo}", Plugin.PluginItem.Config.Debug);
                }
                else
                {
                    if (role != RoleType.Scp0492)
                    {
                        player.CustomInfo = string.Empty;
                        player.DisplayNickname = "[ДАННЫЕ УДАЛЕНЫ]";
                    }
                }
            }
        }
    }
}
