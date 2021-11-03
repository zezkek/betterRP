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
        private List<string> sponsorsNames = new List<string>();
        private string names = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ";
        private int classDPart;
        private HashSet<string> classDperRound = new HashSet<string> { };

        public enum NameBase
        {
            Surnames = 1,
            CallSigns = 2,
            SponsorsNames = 3,
        }

        public void OnWaiting()
        {
            this.LoadNames();
        }

        public void OnRoundStarted()
        {
            this.classDPart = UnityEngine.Random.Range(10, 100);
            this.LoadNames();
            Plugin.PluginItem.NewCoroutine(this.BadgeDisplay());
        }

        private void LoadNames(List<NameBase> nameBase = null)
        {
            nameBase = nameBase ?? new List<NameBase> { NameBase.Surnames, NameBase.CallSigns, NameBase.SponsorsNames };
            foreach (var item in nameBase)
            {
                switch (item)
                {
                    case NameBase.Surnames:
                        Log.Debug($"Total surnames count out of handler: {Plugin.SurnameBase.Count()}", Plugin.PluginItem.Config.Debug);
                        if (Plugin.SurnameBase.Count > 0)
                        {
                            this.surnames = Plugin.SurnameBase;
                            this.surnames.ShuffleList();
                            Log.Debug($"Total surnames count inside handler: {this.surnames.Count()}", Plugin.PluginItem.Config.Debug);
                        }

                        break;
                    case NameBase.CallSigns:
                        Log.Debug($"Total callsigns count out of handler: {Plugin.CallSignsBase.Count()}", Plugin.PluginItem.Config.Debug);
                        if (Plugin.CallSignsBase.Count > 0)
                        {
                            this.callSigns = Plugin.CallSignsBase;
                            this.callSigns.ShuffleList();
                            Log.Debug($"Total callsigns count inside handler: {this.callSigns.Count()}", Plugin.PluginItem.Config.Debug);
                        }

                        break;
                    case NameBase.SponsorsNames:
                        Log.Debug($"Total sponsors names count out of handler: {Plugin.SponsorsNamesBase.Count()}", Plugin.PluginItem.Config.Debug);
                        if (Plugin.CallSignsBase.Count > 0)
                        {
                            this.sponsorsNames = Plugin.SponsorsNamesBase;
                            this.sponsorsNames.ShuffleList();
                            Log.Debug($"Total sponsors names count inside handler: {this.sponsorsNames.Count()}", Plugin.PluginItem.Config.Debug);
                        }

                        break;
                    default:
                        break;
                }
            }
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
                            if (player.Role == RoleType.Scp096 || player.Role == RoleType.Scp106 || player.Role == RoleType.Scp079)
                            {
                                continue;
                            }

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
            this.classDperRound.Clear();
        }

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

            if (this.militaryRole.TryGetValue(player.UserId, out string cInfo))
            {
                player.CustomInfo = cInfo;
                this.militaryRole.Remove(player.UserId);
            }
            else
            {
                player.CustomInfo = customInfo;
            }

            player.DisplayNickname = displayNickname;
            yield break;
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
                this.militaryRole.Remove(player.UserId);
                if (player.Team != Team.SCP)
                {
                    switch (role)
                    {
                        case RoleType.ClassD:
                            if (this.classDperRound.Count > 800)
                            {
                                this.classDPart++;
                                this.classDperRound.Clear();
                            }

                            string number;
                            for (; ;)
                            {
                                number = $"{UnityEngine.Random.Range(0, 1000)}";
                                if (UnityEngine.Random.Range(0, 100) < 90)
                                {
                                    number = "D-" + this.classDPart + string.Join(string.Empty, new string('0', 3 - number.Length), number);
                                }
                                else
                                {
                                    number = "D-" + this.rnd.Next(10000, 99999).ToString();
                                }

                                if (!this.classDperRound.Contains(number))
                                {
                                    break;
                                }
                            }

                            this.classDperRound.Add(number);
                            player.DisplayNickname = number;
                            break;
                        case RoleType.Scientist:
                            if (this.surnames.Count <= 0)
                            {
                                this.LoadNames(new List<NameBase> { NameBase.Surnames, });
                            }

                            selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                            this.surnames.Remove(selectedname);

                            player.DisplayNickname = $"д-р {selectedname} {this.names[UnityEngine.Random.Range(0, this.names.Length)]}. {this.names[UnityEngine.Random.Range(0, this.names.Length)]}.";

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
                            if (this.surnames.Count <= 0)
                            {
                                this.LoadNames(new List<NameBase> { NameBase.Surnames, });
                            }

                            selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                            this.surnames.Remove(selectedname);

                            player.DisplayNickname = $"{selectedname} {this.names[UnityEngine.Random.Range(0, this.names.Length)]}. {this.names[UnityEngine.Random.Range(0, this.names.Length)]}.";

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
                                if ((player.Role == RoleType.NtfCaptain || player.Role == RoleType.ChaosMarauder) && this.sponsorsNames.Count > 0)
                                {
                                    selectedname = this.sponsorsNames[this.rnd.Next(this.sponsorsNames.Count)];
                                    this.sponsorsNames.Remove(selectedname);
                                }
                                else
                                {
                                    if (this.callSigns.Count <= 0)
                                    {
                                        this.LoadNames(new List<NameBase> { NameBase.CallSigns, });
                                    }

                                    selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                    this.callSigns.Remove(selectedname);
                                }

                                string roleName = "<color=%color>%post</color>";
                                switch (player.Role)
                                {
                                    case RoleType.NtfSergeant:
                                        roleName += roleName.Replace("%post", $"Сержант {player.UnitName}");
                                        roleName = roleName.Replace("%color", "#0180DA");
                                        break;
                                    case RoleType.NtfCaptain:
                                        roleName += roleName.Replace("%post", $"Командир {player.UnitName}");
                                        roleName = roleName.Replace("%color", "#000080");
                                        break;
                                    case RoleType.NtfPrivate:
                                        roleName += roleName.Replace("%post", $"Капрал {player.UnitName}");
                                        roleName = roleName.Replace("%color", "#8BC5FF");
                                        break;
                                    case RoleType.NtfSpecialist:
                                        roleName += roleName.Replace("%post", $"Специалист ВУС {player.UnitName}");
                                        roleName = roleName.Replace("%color", "#0180DA");
                                        break;
                                    case RoleType.ChaosMarauder:
                                        roleName += roleName.Replace("%post", $"Мародёр Хаоса");
                                        roleName = roleName.Replace("%color", "#274e13");
                                        break;
                                    case RoleType.ChaosRepressor:
                                        roleName += roleName.Replace("%post", $"Репрессор Хаоса");
                                        roleName = roleName.Replace("%color", "#274e13");
                                        break;
                                    case RoleType.ChaosRifleman:
                                        roleName += roleName.Replace("%post", $"Стрелок Хаоса");
                                        roleName = roleName.Replace("%color", "#38761d");
                                        break;
                                    case RoleType.ChaosConscript:
                                        roleName += roleName.Replace("%post", $"Новобранец Хаоса");
                                        roleName = roleName.Replace("%color", "#6aa84f");
                                        break;
                                    default:
                                        break;
                                }

                                if (this.militaryRole.TryGetValue(player.UserId, out string prevRole))
                                {
                                    this.militaryRole[player.UserId] = roleName;
                                }
                                else
                                {
                                    this.militaryRole.Add(player.UserId, roleName);
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
