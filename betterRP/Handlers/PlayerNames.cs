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

        public IEnumerator<float> BadgeDisplay()
        {
            yield return MEC.Timing.WaitForSeconds(Plugin.PluginItem.Config.HintDisplayTime);
            while (Round.IsStarted)
            {
                string name = "<align=left><pos=-21%><b><size=20><color=#C1B5B5>ПОЛЬЗОВАТЕЛЬ</pos><pos=-7%> :  </color>" +
                        "<color=%color>%name</color></pos>\n";
                string post = "<pos=-21%><b><size=20>" +
                        "<color=#C1B5B5>ДОЛЖНОСТЬ</pos>" +
                        "<pos=-7%> :  </color><color=%color>%post</color>" +
                        "</size></pos>\n";
                string role = "<pos=-21%><b><size=20>" +
                        "<color=#C1B5B5>РОЛЬ" +
                        "<pos=-7%> :  </color><color=%color>%role</color></size></pos>\n";
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

                    display += name.Replace("%name", player.Nickname);
                    switch (player.Team)
                    {
                        case Team.SCP:
                            display += role.Replace("%role", player.DisplayNickname);
                            if (player.Role == RoleType.Scp0492 && player.CustomInfo != string.Empty && player.CustomInfo != null)
                            {
                                display += post.Replace("<color=%color>%post</color>", player.CustomInfo);
                                display = display.Replace("%color", "#FFD966");
                            }

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
                                    display += post.Replace("%post", $"Лейтенант {player.UnitName}");
                                    display = display.Replace("%color", "#1E90FF");
                                    break;
                                case RoleType.NtfCaptain:
                                    display += post.Replace("%post", $"Командир {player.UnitName}");
                                    display = display.Replace("%color", "#000080");
                                    break;
                                case RoleType.NtfPrivate:
                                    display += post.Replace("%post", $"Кадет {player.UnitName}");
                                    display = display.Replace("%color", "#00BFFF");
                                    break;
                                case RoleType.NtfSpecialist:
                                    display += post.Replace("<color=%color>%post</color>", $"{player.CustomInfo} <color=%color>{player.UnitName}</color>");
                                    display = display.Replace("%color", "#0000CD");
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
                            display += post.Replace("%post", "Агент Хаоса");
                            display = display.Replace("%color", "#6aa84f");
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

                    player.ShowHint(display + "</pos></size></b></align>", 10.5f);
                }

                yield return MEC.Timing.WaitForSeconds(10f);
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Killer.Role == RoleType.Scp049)
            {
                Plugin.PluginItem.NewCoroutine(this.NameChangeDelay(ev.Target));
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

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.Player.Role != RoleType.None)
            {
                string selectedname;

                if (ev.Player.IsHuman)
                {
                    ev.Player.CustomInfo = string.Empty;

                    // Pattern
                    string message = "\n\n<b><size=35>" +
                        "<color=#C1B5B5>Вы сотрудник</color> " +
                        "<color=%color>%post</color><color=#C1B5B5>,</color> " +
                        "<color=#C1B5B5>Ваша должность - </color>" +
                        "<color=%color>%subclass</color>" +
                        "</size></b>";

                    switch (ev.Player.Role)
                    {
                        case RoleType.ClassD:
                            ev.Player.DisplayNickname = "D-" + this.rnd.Next(10000, 99999).ToString();
                            message = message.Replace("%color", "#CE7E00").Replace("Ваша должность", "Ваш номер").Replace("%post", "класса-D").Replace("%subclass", ev.Player.DisplayNickname);
                            break;
                        case RoleType.Scientist:
                            if (this.surnames.Count > 0)
                            {
                                selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                                this.surnames.Remove(selectedname);
                            }
                            else
                            {
                                selectedname = ev.Player.Nickname;
                            }

                            ev.Player.DisplayNickname = "д-р " + selectedname;

                            if (ev.Player.HasItem(ItemType.KeycardFacilityManager))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Заведующий Комплекса</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardZoneManager))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Заведующий Зоны</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardResearchCoordinator))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Координатор исследований</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardContainmentEngineer))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Инженер Комплекса</color>";
                            }
                            else
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Научный сотрудник</color>";
                            }

                            message = message.Replace(" сотрудник", string.Empty);
                            message = message.Replace("<color=%color>%subclass</color>", ev.Player.CustomInfo);
                            message = message.Replace("%color", "#FFD966").Replace("%post", ev.Player.DisplayNickname);
                            break;

                        case RoleType.FacilityGuard:
                            if (this.surnames.Count > 0)
                            {
                                selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                                this.surnames.Remove(selectedname);
                            }
                            else
                            {
                                selectedname = ev.Player.Nickname;
                            }

                            ev.Player.DisplayNickname = selectedname;

                            if (ev.Player.HasItem(ItemType.KeycardNTFCommander))
                            {
                                ev.Player.CustomInfo = "<color=#757575>Начальник СБ</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardNTFLieutenant))
                            {
                                ev.Player.CustomInfo = "<color=#757575>Старший сотрудник СБ</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardGuard))
                            {
                                ev.Player.CustomInfo = "<color=#757575>Сотрудник СБ</color>";
                            }
                            else
                            {
                                ev.Player.CustomInfo = "<color=#757575>Младший сотрудник СБ</color>";
                            }

                            message = message.Replace("<color=%color>%subclass</color>", ev.Player.CustomInfo + $" <color=%color>{ev.Player.DisplayNickname}</color>");
                            message = message.Replace("%color", "#757575").Replace("%post", "СБ Комплекса");
                            break;

                        default:
                            if (ev.Player.Team == Team.CHI)
                            {
                                message = message.Replace("сотрудник</color> ", "агент</color> <color=%color>Хаоса</color>").Replace("<color=%color>%post</color>", string.Empty).Replace("Ваша должность", "Ваш позывной");
                                if (this.callSigns.Count > 0)
                                {
                                    selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                    this.callSigns.Remove(selectedname);
                                }
                                else
                                {
                                    selectedname = ev.Player.Nickname;
                                }

                                if (ev.Player.Role == RoleType.ChaosMarauder)
                                {
                                    message = message.Replace("%color", "#274e13");
                                }
                                else if (ev.Player.Role == RoleType.ChaosRepressor)
                                {
                                    message = message.Replace("%color", "#38761d");
                                }
                                else if (ev.Player.Role == RoleType.ChaosRifleman)
                                {
                                    message = message.Replace("%color", "#38761d");
                                }
                                else
                                {
                                    message = message.Replace("%color", "#6aa84f");
                                }

                                ev.Player.DisplayNickname = $"'{selectedname}'";
                                message = message.Replace("%subclass", ev.Player.DisplayNickname);
                                break;
                            }
                            else if (ev.Player.Team == Team.MTF)
                            {
                                message = message.Replace("сотрудник</color> ", "член</color> <color=%color>МОГ Эпсилон-11</color>").Replace("<color=%color>%post</color>", string.Empty).Replace("Ваша должность", "Ваш позывной");
                                if (this.callSigns.Count > 0)
                                {
                                    selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                    this.callSigns.Remove(selectedname);
                                }
                                else
                                {
                                    selectedname = ev.Player.Nickname;
                                }

                                if (ev.Player.Role == RoleType.NtfCaptain)
                                {
                                    message = message.Replace("%color", "#000080");
                                }
                                else if (ev.Player.Role == RoleType.NtfSpecialist)
                                {
                                    ev.Player.CustomInfo = "<color=#0000CD>Специалист по ВУС</color>";
                                    message = message.Replace("%color", "#0000CD");
                                }
                                else if (ev.Player.Role == RoleType.NtfSergeant)
                                {
                                    message = message.Replace("%color", "#1E90FF");
                                }
                                else
                                {
                                    message = message.Replace("%color", "#00BFFF");
                                }

                                ev.Player.DisplayNickname = $"'{selectedname}'";
                                message = message.Replace("%subclass", ev.Player.DisplayNickname);
                                break;
                            }
                            else
                            {
                                ev.Player.DisplayNickname = ev.Player.Nickname;
                                message = string.Empty;
                                break;
                            }
                    }

                    Log.Debug($"Player Name: {ev.Player.Nickname}\nPlayer Nickname: {ev.Player.DisplayNickname}\nPlayer Custom Info: {ev.Player.CustomInfo}", Plugin.PluginItem.Config.Debug);
                    ev.Player.ShowHint(message, Plugin.PluginItem.Config.HintDisplayTime);
                }
                else
                {
                    if (ev.Player.Role != RoleType.Scp0492)
                    {
                        ev.Player.CustomInfo = string.Empty;
                        ev.Player.DisplayNickname = "[ДАННЫЕ УДАЛЕНЫ]";
                    }
                    else if (ev.Player.DisplayNickname == string.Empty || ev.Player.DisplayNickname == ev.Player.Nickname || ev.Player.DisplayNickname == null)
                    {
                        ev.Player.CustomInfo = string.Empty;
                        ev.Player.DisplayNickname = ev.Player.Nickname;
                    }
                }
            }
        }
    }
}
