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

        private List<string> scientistAndSecuritySurname = new List<string>();
        private List<string> callSigns = new List<string>();
        private string selectedname;

        public void OnRoundStarted()
        {
            this.scientistAndSecuritySurname = File.ReadAllLines("Names/ScientistAndSecuritySurname.txt").ToList();
            this.scientistAndSecuritySurname.ShuffleList();
            this.callSigns = File.ReadAllLines("Names/CallSigns.txt").ToList();
            this.callSigns.ShuffleList();
        }

        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.DisplayNickname = ev.Target.Nickname;
        }

        public void OnChangedRole(ChangedRoleEventArgs ev)
        {
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.Player.Role != RoleType.None)
            {
                if (ev.Player.IsHuman)
                {
                    switch (ev.Player.Role)
                    {
                        case RoleType.ClassD:
                            int dClassId = this.rnd.Next(this.rnd.Next(10000, 99999));
                            ev.Player.DisplayNickname = "D-" + dClassId.ToString();
                            ev.Player.ShowHint("\n\nВы сотрудник <color=orange>D-Класса</color>, Ваш номер <color=orange>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;
                        case RoleType.Scientist:
                            var maxKeycard = ItemType.KeycardO5;
                            if (ev.Player.HasItem(ItemType.KeycardScientist))
                            {
                                maxKeycard = ItemType.KeycardScientist;
                            }

                            if (ev.Player.HasItem(ItemType.KeycardZoneManager))
                            {
                                maxKeycard = ItemType.KeycardZoneManager;
                            }

                            if (ev.Player.HasItem(ItemType.KeycardScientistMajor))
                            {
                                maxKeycard = ItemType.KeycardScientistMajor;
                            }

                            if (ev.Player.HasItem(ItemType.KeycardContainmentEngineer))
                            {
                                maxKeycard = ItemType.KeycardContainmentEngineer;
                            }

                            if (ev.Player.HasItem(ItemType.KeycardFacilityManager))
                            {
                                maxKeycard = ItemType.KeycardFacilityManager;
                            }

                            switch (maxKeycard)
                            {
                                case ItemType.KeycardO5:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.DisplayNickname = "Д-р " + this.selectedname;
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardScientist:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardZoneManager:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>член руководства комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardScientistMajor:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardContainmentEngineer:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>инженер комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardFacilityManager:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>член руководства комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                            }

                            break;

                        case RoleType.FacilityGuard:

                            var maxKeycardSec = ItemType.KeycardO5;
                            if (ev.Player.HasItem(ItemType.KeycardGuard))
                            {
                                maxKeycardSec = ItemType.KeycardGuard;
                            }

                            if (ev.Player.HasItem(ItemType.KeycardNTFLieutenant))
                            {
                                maxKeycardSec = ItemType.KeycardNTFLieutenant;
                            }

                            if (ev.Player.HasItem(ItemType.KeycardNTFCommander))
                            {
                                maxKeycardSec = ItemType.KeycardNTFCommander;
                            }

                            switch (maxKeycardSec)
                            {
                                case ItemType.KeycardO5:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.DisplayNickname = "Мл. Охранник " + this.selectedname;
                                    break;
                                case ItemType.KeycardGuard:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.DisplayNickname = "Охранник " + this.selectedname;
                                    break;
                                case ItemType.KeycardNTFLieutenant:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.DisplayNickname = "Ст. Охранник " + this.selectedname;
                                    break;
                                case ItemType.KeycardNTFCommander:
                                    this.selectedname = this.scientistAndSecuritySurname[this.rnd.Next(this.scientistAndSecuritySurname.Count)];
                                    this.scientistAndSecuritySurname.Remove(this.selectedname);
                                    ev.Player.DisplayNickname = "Офицер " + this.selectedname;
                                    break;
                            }

                            ev.Player.ShowHint("\n\nВы сотрудник <color=#757575>СБ комплекса</color>, Ваша должность <color=#757575>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCadet:
                            this.selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                            this.callSigns.Remove(this.selectedname);
                            ev.Player.DisplayNickname = "Кадет '" + this.selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[this.rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".";
                            ev.Player.ShowHint("\n\nВы член <color=#00BFFF>МОГ Эпсилон-11</color>, Ваша должность <color=#00BFFF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfLieutenant:
                            this.selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                            this.callSigns.Remove(this.selectedname);
                            ev.Player.DisplayNickname = "Лейтенант '" + this.selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[this.rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".";
                            ev.Player.ShowHint("\n\nВы член <color=#1E90FF>МОГ Эпсилон-11</color>, Ваша должность <color=#1E90FF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCommander:
                            this.selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                            this.callSigns.Remove(this.selectedname);
                            ev.Player.DisplayNickname = "Командир '" + this.selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[this.rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".";
                            ev.Player.ShowHint("\n\nВы член <color=#000080>МОГ Эпсилон-11</color>, Ваша должность <color=#000080>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfScientist:
                            this.selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                            this.callSigns.Remove(this.selectedname);
                            ev.Player.DisplayNickname = "Специалист по ВУС '" + this.selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[this.rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".";
                            ev.Player.ShowHint("\n\nВы член <color=#0000CD>МОГ Эпсилон-11</color>, Ваша должность <color=#0000CD>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.ChaosInsurgency:
                            this.selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                            this.callSigns.Remove(this.selectedname);
                            ev.Player.DisplayNickname = "Агент " + this.selectedname;
                            ev.Player.ShowHint("\n\nВы агент <color=green>Хаоса</color>, Ваш позывной <color=green>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        default:
                            ev.Player.DisplayNickname = ev.Player.Nickname;
                            break;
                    }
                }
                else
                {
                    ev.Player.DisplayNickname = "[ДАННЫЕ УДАЛЕНЫ]";
                }
            }

            this.selectedname = null;
        }
    }
}
