using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using MEC;
using UnityEngine;
using HarmonyLib;

namespace betterRP
{
    public class PlayerNames
    {
        public PlayerNames NameHandler;
        System.Random rnd = new System.Random();
        List<string> ScientistAndSecuritySurname;
        List<string> CHINickname;
        List<string> NTFNickName;
        string selectedname;

        public void OnRoundStarted()
        {
            ScientistAndSecuritySurname = Plugin.PluginItem.Config.ScientistAndSecuritySurname;
            CHINickname = Plugin.PluginItem.Config.CHINames;
            NTFNickName = Plugin.PluginItem.Config.MTFNames;
            ScientistAndSecuritySurname = File.ReadAllLines(@"NamesFolder\ScientistAndSecuritySurname.txt", Encoding.UTF8).ToList();
            ScientistAndSecuritySurname.ShuffleList();
            NTFNickName = File.ReadAllLines(@"NamesFolder\NTFNickName.txt", Encoding.UTF8).ToList();
            NTFNickName.ShuffleList();
            CHINickname= File.ReadAllLines(@"NamesFolder\CHINickname.txt", Encoding.UTF8).ToList();
            CHINickname.ShuffleList();
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
                                    ev.Player.DisplayNickname = ("Д-р " + selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardZoneManager:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Руководитель зоны д-р " + selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>член руководства комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardScientistMajor:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Ст. научный сотрудник " + selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>научный сотрудник</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardContainmentEngineer:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Инженер камер содержания " + selectedname);
                                    ev.Player.ShowHint("\n\nВы <color=yellow>инженер комплекса</color>, Ваша должность <color=yellow>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                                    break;
                                case ItemType.KeycardFacilityManager:
                                    selectedname = ScientistAndSecuritySurname[rnd.Next(ScientistAndSecuritySurname.Count)];
                                    ScientistAndSecuritySurname.Remove(selectedname);
                                    ev.Player.DisplayNickname = ("Руководитель комплекса д-р " + selectedname);
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
                            ev.Player.ShowHint("\n\nВы сотрудник <color=#6b6b6b>СБ комплекса</color>, Ваша должность <color=#6b6b6b>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCadet:
                            selectedname = NTFNickName[rnd.Next(NTFNickName.Count)];
                            NTFNickName.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Кадет '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#00BFFF>МОГ Эпсилон-11</color>, Ваша должность <color=#00BFFF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfLieutenant:
                            selectedname = NTFNickName[rnd.Next(NTFNickName.Count)];
                            NTFNickName.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Лейтенант '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#1E90FF>МОГ Эпсилон-11</color>, Ваша должность <color=#1E90FF>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfCommander:
                            selectedname = NTFNickName[rnd.Next(NTFNickName.Count)];
                            NTFNickName.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Командир '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#0300a3>МОГ Эпсилон-11</color>, Ваша должность <color=#000080>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.NtfScientist:
                            selectedname = NTFNickName[rnd.Next(NTFNickName.Count)];
                            NTFNickName.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Специалист по ВУС '" + selectedname + "' " + Plugin.PluginItem.Config.ScientistAndSecuritySurname[rnd.Next(Plugin.PluginItem.Config.ScientistAndSecuritySurname.Count)][0] + ".");
                            ev.Player.ShowHint("\n\nВы член <color=#0000CD>МОГ Эпсилон-11</color>, Ваша должность <color=#0000CD>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;

                        case RoleType.ChaosInsurgency:
                            selectedname = CHINickname[rnd.Next(CHINickname.Count)];
                            CHINickname.Remove(selectedname);
                            ev.Player.DisplayNickname = ("Агент " + selectedname);
                            ev.Player.ShowHint("\n\nВы агент <color=green>Хаоса</color>, Ваш позывной <color=green>" + ev.Player.DisplayNickname + "</color>", Plugin.PluginItem.Config.HintDisplayTime);
                            break;
                    }
                }
                if (ev.Player.IsScp)
                    ev.Player.DisplayNickname = "[ДАННЫЕ УДАЛЕНЫ]";
                if (ev.Player.Role == RoleType.Spectator)
                    ev.Player.DisplayNickname = ev.Player.Nickname;
            }
        }
        public void OnDying(DyingEventArgs ev)
        {
            ev.Target.DisplayNickname = ev.Target.Nickname;
        }


        //прыжки за стамину wip
        //public void OnSyncingData(SyncingDataEventArgs ev)
        //{
        //    var oldpos = ev.Player.Position;
        //    if (ev.Player.IsHuman && ev.Player.IsJumping)
        //        Log.Info(ev.Player.Stamina.RemainingStamina.ToString());
        //    else
        //        delaypos(oldpos, ev);
        //}
        //public IEnumerator<float> delaypos(Vector3 oldpos, SyncingDataEventArgs ev)
        //{
        //    //max stamina = 1 lol 
        //    Log.Info("malo stamina");
        //    yield return Timing.WaitForSeconds(1f);
        //    ev.Player.Position = oldpos;
        //}
    }
}
