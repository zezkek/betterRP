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
            Log.Debug($"Total surnames count out of handler: {Plugin.SurnameBase.Count()}", Plugin.PluginItem.Config.Debug);
            if (Plugin.SurnameBase.Count > 0)
            {
                this.isSurnames = true;
                this.surnames = Plugin.SurnameBase;
                this.surnames.ShuffleList();
                Log.Debug($"Total surnames count inside handler: {this.surnames.Count()}", Plugin.PluginItem.Config.Debug);
            }

            Log.Debug($"Total callsigns count out of handler: {Plugin.CallSignsBase.Count()}", Plugin.PluginItem.Config.Debug);
            if (Plugin.CallSignsBase.Count > 0)
            {
                this.isCallSigns = true;
                this.callSigns = Plugin.CallSignsBase;
                this.callSigns.ShuffleList();
                Log.Debug($"Total callsigns count inside handler: {this.callSigns.Count()}", Plugin.PluginItem.Config.Debug);
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

                            message = message.Replace(" сотрудник", string.Empty);
                            message = message.Replace("<color=%color>%subclass</color>", ev.Player.CustomInfo);
                            message = message.Replace("%color", "#FFD966").Replace("%post", ev.Player.DisplayNickname);
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

                            message = message.Replace("<color=%color>%subclass</color>", ev.Player.CustomInfo + $" <color=%color>{ev.Player.DisplayNickname}</color>");
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

                            ev.Player.DisplayNickname = $"'{selectedname}'";
                            message = message.Replace("%subclass", ev.Player.DisplayNickname).Replace("сотрудник</color> ", "агент</color> <color=%color>Хаоса</color>").Replace("<color=%color>%post</color>", string.Empty).Replace("Ваша должность", "Ваш позывной");
                            message = message.Replace("%color", "#6aa84f");
                            break;

                        default:
                            if (ev.Player.Team == Team.MTF)
                            {
                                message = message.Replace("сотрудник</color> ", "член</color> <color=%color>МОГ Эпсилон-11</color>").Replace("<color=%color>%post</color>", string.Empty).Replace("Ваша должность", "Ваш позывной");
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
                                    message = message.Replace("%color", "#000080");
                                }
                                else if (ev.Player.Role == RoleType.NtfScientist)
                                {
                                    ev.Player.CustomInfo = "<color=#0000CD>Специалист по ВУС</color>";
                                    message = message.Replace("%color", "#0000CD");
                                }
                                else if (ev.Player.Role == RoleType.NtfLieutenant)
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
