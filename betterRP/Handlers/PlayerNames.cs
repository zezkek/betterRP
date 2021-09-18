using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace betterRP
{
    public class PlayerNames
    {
        public PlayerNames NameHandler;
        Random rnd = new Random();
        List<string> ScientistAndSecuritySurname = new List<string>();
        List<string> MTFCHINickname = new List<string>();
        string selectedname;
        public void OnRoundStarted()
        {
            ScientistAndSecuritySurname = File.ReadAllLines("Names/ScientistAndSecuritySurname.txt").ToList();
            ScientistAndSecuritySurname.ShuffleList();
            MTFCHINickname = File.ReadAllLines("Names/Nickname").ToList();
            MTFCHINickname.ShuffleList();
        }
        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.DisplayNickname = ev.Target.Nickname;
        }
        public void OnChangedRole(ChangedRoleEventArgs ev)
        {
            if (Plugin.PluginItem.Config.PlayerNamesEnabled && ev.Player != null && ev.Player.Role != RoleType.None)
                if (ev.Player.IsHuman)
                    switch (ev.Player.Role)
                    {
                        case RoleType.ClassD:
                            int DClassId = rnd.Next(rnd.Next(10000, 99999));
                            ev.Player.DisplayNickname = ("D-" + DClassId.ToString());
                            ev.Player.ShowHint("\n\nВы сотрудник <color=orange>D-Класса</color>, Ваш номер <color=orange>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;
                        case RoleType.Scientist:
                            var MaxKeycard = ItemType.KeycardO5;
                            if (ev.Player.HasItem(ItemType.KeycardScientist))
                                MaxKeycard = ItemType.KeycardScientist;
                            if (ev.Player.HasItem(ItemType.KeycardZoneManager))
                                MaxKeycard = ItemType.KeycardZoneManager;
                            if (ev.Player.HasItem(ItemType.KeycardScientistMajor))
                                MaxKeycard = ItemType.KeycardScientistMajor;
                            if (ev.Player.HasItem(ItemType.KeycardContainmentEngineer))
                                MaxKeycard = ItemType.KeycardContainmentEngineer;
                            if (ev.Player.HasItem(ItemType.KeycardFacilityManager))
                                MaxKeycard = ItemType.KeycardFacilityManager;
                            switch (MaxKeycard)
                            {
                                case ItemType.KeycardO5:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Д-р " + selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardScientist:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardZoneManager:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>член руководства комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardScientistMajor:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardContainmentEngineer:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>инженер комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardFacilityManager:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>член руководства комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                            }
                            break;

                        case RoleType.FacilityGuard:

                            var MaxKeycardSec = ItemType.KeycardO5;
                            if (ev.Player.HasItem(ItemType.KeycardGuard))
                                MaxKeycardSec = ItemType.KeycardGuard;
                            if (ev.Player.HasItem(ItemType.KeycardNTFLieutenant))
                                MaxKeycardSec = ItemType.KeycardNTFLieutenant;
                            if (ev.Player.HasItem(ItemType.KeycardNTFCommander))
                                MaxKeycardSec = ItemType.KeycardNTFCommander;
                            switch (MaxKeycardSec)
                            {
                                case ItemType.KeycardO5:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Мл. Охранник " + selectedname);
                                    break;
                                case ItemType.KeycardGuard:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Охранник " + selectedname);
                                    break;
                                case ItemType.KeycardNTFLieutenant:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Ст. Охранник " + selectedname);
                                    break;
                                case ItemType.KeycardNTFCommander:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Офицер " + selectedname);
                                    break;
                            }
                            ev.Player.ShowHint("\n\nВы сотрудник <color=gray>СБ комплекса</color>, Ваша должность <color=gray>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCadet:
                            selectedname = MTFCHINickname[rnd.Next(MTFCHINickname.Count)];
                            MTFCHINickname.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Кадет '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#00BFFF>МОГ Эпсилон-11</color>, Ваша должность <color=#00BFFF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfLieutenant:
                            selectedname = MTFCHINickname[rnd.Next(MTFCHINickname.Count)];
                            MTFCHINickname.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Лейтенант '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#1E90FF>МОГ Эпсилон-11</color>, Ваша должность <color=#1E90FF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCommander:
                            selectedname = MTFCHINickname[rnd.Next(MTFCHINickname.Count)];
                            MTFCHINickname.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Командир '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#000080>МОГ Эпсилон-11</color>, Ваша должность <color=#000080>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfScientist:
                            selectedname = MTFCHINickname[rnd.Next(MTFCHINickname.Count)];
                            MTFCHINickname.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Специалист по ВУС '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#0000CD>МОГ Эпсилон-11</color>, Ваша должность <color=#0000CD>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.ChaosInsurgency:
                            selectedname = MTFCHINickname[rnd.Next(MTFCHINickname.Count)];
                            MTFCHINickname.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Агент " + selectedname);
                            ev.Player.ShowHint("\n\nВы агент <color=green>Хаоса</color>, Ваш позывной <color=green>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        default:
                            ev.Player.DisplayNickname = ev.Player.Nickname;
                            break;
                    }
                else
                    ev.Player.DisplayNickname = "[ДАННЫЕ УДАЛЕНЫ]";
            selectedname = null;
        }
    }
}
