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
        private bool isSurnames = false;
        private bool isCallSigns = false;

        public void OnWaiting()
        {
            if (Plugin.PluginItem.SurnameBase.Count > 0)
            {
                this.isSurnames = true;
                this.surnames = Plugin.PluginItem.SurnameBase;
                this.surnames.ShuffleList();
            }

            if (Plugin.PluginItem.CallSignsBase.Count > 0)
            {
                this.isCallSigns = true;
                this.callSigns = Plugin.PluginItem.CallSignsBase;
                this.callSigns.ShuffleList();
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

        public void OnChangedRole(ChangedRoleEventArgs ev)
        {
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.Player.Role != RoleType.None)
            {
                string selectedname;

                if (ev.Player.IsHuman)
                {
                    ev.Player.CustomInfo = string.Empty;

                    // Pattern
                    string message = "<b><size=35>" +
                        "<color=#C1B5B5>Вы сотрудник</color> " +
                        "<color=%color>%post</color>, " +
                        "<color=#C1B5B5>Ваша должность</color> - " +
                        "<color=%color>%subclass</color>." +
                        "</size></b>";

                    switch (ev.Player.Role)
                    {
                        case RoleType.ClassD:
                            ev.Player.DisplayNickname = "D-" + this.rnd.Next(10000, 99999).ToString();
                            message = message.Replace("%color", "#CE7E00").Replace("Ваша должность", "Ваш номер").Replace("%subclass", ev.Player.DisplayNickname);
                            break;
                        case RoleType.Scientist:
                            if (this.isSurnames)
                            {
                                selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                                this.surnames.Remove(selectedname);
                            }
                            else
                            {
                                selectedname = ev.Player.Nickname;
                            }

                            ev.Player.DisplayNickname = "д-р " + selectedname;

                            if (ev.Player.HasItem(ItemType.KeycardZoneManager) || ev.Player.HasItem(ItemType.KeycardFacilityManager))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Член руководства Комплекса</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardScientistMajor))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Старший научный сотрудник</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardContainmentEngineer))
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Инженер Комплекса</color>";
                            }
                            else
                            {
                                ev.Player.CustomInfo = "<color=#FFD966>Научный сотрудник</color>";
                            }

                            message = message.Replace("<color=%color>%subclass</color>", ev.Player.CustomInfo);
                            message = message.Replace("%color", "#FFD966").Replace("сотрудник", string.Empty).Replace("%post", ev.Player.DisplayNickname);
                            break;

                        case RoleType.FacilityGuard:
                            if (this.isSurnames)
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
                                ev.Player.CustomInfo = "<color=#757575>Офицер</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardNTFLieutenant))
                            {
                                ev.Player.CustomInfo = "<color=#757575>Старший Охранник</color>";
                            }
                            else if (ev.Player.HasItem(ItemType.KeycardSeniorGuard))
                            {
                                ev.Player.CustomInfo = "<color=#757575>Охранник</color>";
                            }
                            else
                            {
                                ev.Player.CustomInfo = "<color=#757575>Младший Охранник</color>";
                            }

                            message = message.Replace("<color=%color>%subclass</color>", ev.Player.CustomInfo + ev.Player.DisplayNickname);
                            message = message.Replace("%color", "#757575").Replace("%post", "СБ Комплекса");
                            break;

                        case RoleType.ChaosInsurgency:
                            if (this.isCallSigns)
                            {
                                selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                this.callSigns.Remove(selectedname);
                            }
                            else
                            {
                                selectedname = ev.Player.Nickname;
                            }

                            ev.Player.DisplayNickname = selectedname;
                            message = message.Replace("%subclass", ev.Player.DisplayNickname).Replace("сотрудник", "агент <color=%color>Хаоса</color>");
                            message = message.Replace("%color", "#6aa84f");
                            break;

                        default:
                            if (ev.Player.Team == Team.MTF)
                            {
                                message = message.Replace("сотрудник</color>", "член</color> <color=%color>МОГ Эпсилон-11</color>");
                                if (this.isCallSigns)
                                {
                                    selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                    this.callSigns.Remove(selectedname);
                                }
                                else
                                {
                                    selectedname = ev.Player.Nickname;
                                }

                                if (ev.Player.Role == RoleType.NtfCommander)
                                {
                                    ev.Player.DisplayNickname = $"Командир '{selectedname}' ";
                                    message = message.Replace("%color", "#000080");
                                }
                                else if (ev.Player.Role == RoleType.NtfScientist)
                                {
                                    ev.Player.DisplayNickname = $"Специалист по ВУС '{selectedname}'";
                                    message = message.Replace("%color", "#0000CD");
                                }
                                else if (ev.Player.Role == RoleType.NtfLieutenant)
                                {
                                    ev.Player.DisplayNickname = $"Лейтенант '{selectedname}' ";
                                    message = message.Replace("%color", "#1E90FF");
                                }
                                else
                                {
                                    ev.Player.DisplayNickname = $"Кадет '{selectedname}' ";
                                    message = message.Replace("%color", "#00BFFF");
                                }

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
