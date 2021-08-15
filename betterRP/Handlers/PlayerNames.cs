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
        List<string> ScientistAndSecuritySurname = Plugin.PluginItem.Config.ScientistAndSecuritySurname;
        string[] ChaosNickname = File.ReadAllLines(@"Names\ChaosNicknames.txt");
        string[] NTFNickName = File.ReadAllLines(@"Names\NTFNickNames.txt");
        string selectedname;
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
                            ev.Player.DisplayNickname = ("Кадет '" + NTFNickName[rnd.Next(NTFNickName.Length)] + "' " + ScientistSurname[rnd.Next(ScientistSurname.Length)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#00BFFF>МОГ Эпсилон-11</color>, Ваша должность <color=#00BFFF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfLieutenant:
                            ev.Player.DisplayNickname = ("Лейтенант '" + NTFNickName[rnd.Next(NTFNickName.Length)] + "' " + ScientistSurname[rnd.Next(ScientistSurname.Length)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#1E90FF>МОГ Эпсилон-11</color>, Ваша должность <color=#1E90FF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCommander:
                            ev.Player.DisplayNickname = ("Командир '" + NTFNickName[rnd.Next(NTFNickName.Length)] + "' " + ScientistSurname[rnd.Next(ScientistSurname.Length)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#000080>МОГ Эпсилон-11</color>, Ваша должность <color=#000080>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfScientist:
                            ev.Player.DisplayNickname = ("Специалист по ВУС '" + NTFNickName[rnd.Next(NTFNickName.Length)] + "' " + ScientistSurname[rnd.Next(ScientistSurname.Length)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#0000CD>МОГ Эпсилон-11</color>, Ваша должность <color=#0000CD>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.ChaosInsurgency:
                            ev.Player.DisplayNickname = ("Агент " + ChaosNickname[rnd.Next(ChaosNickname.Length)]);
                            ev.Player.ShowHint("\n\nВы агент <color=green>Хаоса</color>, Ваш позывной <color=green>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        default:
                            ev.Player.DisplayNickname = ev.Player.Nickname;
                            break;
                    }
                else
                    switch (ev.Player.Role)
                    {
                        
                    }
        }
    }
}
