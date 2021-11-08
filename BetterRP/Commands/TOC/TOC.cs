// #define ShowHints
namespace BetterRP.Commands.TOC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Permissions.Extensions;
    using MEC;
    using UnityEngine;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class TOC : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TOC"/> class.
        /// </summary>
        public TOC() => this.LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "toc";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = { };

        /// <inheritdoc/>
        public override string Description { get; } = "Центр Тактических Операций";

        private static readonly Vector3 mtfPoint = new Vector3(178, 992.5f, -60);
        private static readonly Vector3 chiPoint = new Vector3(5, 988.5f, -58);
        public static float OnEvacuateCooldownMTF;
        public static float OnSupportCooldownMTF;
        public static float OnEvacuateCooldownCHI;
        public static float OnSupportCooldownCHI;
        private static readonly int[,] EvacQueryMTF = new int[,]
        {
            { (int)RoleType.Scp173, 0 },
            { (int)RoleType.Scp049, 0 },
            { (int)RoleType.Scp096, 0 },
            { (int)RoleType.Scientist, 0 },
            { (int)RoleType.NtfCaptain, 0 },
            { (int)RoleType.NtfSpecialist, 0 },
            { (int)RoleType.NtfSergeant, 0 },
            { (int)RoleType.NtfPrivate, 0 },
            { (int)RoleType.Scientist, 1 },
            { (int)RoleType.NtfCaptain, 1 },
            { (int)RoleType.NtfSpecialist, 1 },
            { (int)RoleType.NtfSergeant, 1 },
            { (int)RoleType.NtfPrivate, 1 },
            { (int)RoleType.ChaosMarauder, 1 },
            { (int)RoleType.ChaosRepressor, 1 },
            { (int)RoleType.ChaosRifleman, 1 },
            { (int)RoleType.ChaosConscript, 1 },
            { (int)RoleType.ClassD, 1 },
            { (int)RoleType.Scp93989, 1 },
            { (int)RoleType.Scp93953, 1 },
            { (int)RoleType.Scp0492, 1 },
        };

        private static readonly int[,] EvacQueryCHI = new int[,]
        {
            { (int)RoleType.Scp173, 0 },
            { (int)RoleType.Scp049, 0 },
            { (int)RoleType.Scp096, 0 },
            { (int)RoleType.ChaosMarauder, 1 },
            { (int)RoleType.ChaosRepressor, 1 },
            { (int)RoleType.ChaosRifleman, 1 },
            { (int)RoleType.ChaosConscript, 1 },
            { (int)RoleType.ClassD, 1 },
            { (int)RoleType.Scientist, 1 },
            { (int)RoleType.NtfCaptain, 1 },
            { (int)RoleType.NtfSpecialist, 1 },
            { (int)RoleType.NtfSergeant, 1 },
            { (int)RoleType.NtfPrivate, 1 },
            { (int)RoleType.ClassD, 0 },
            { (int)RoleType.Scp93989, 0 },
            { (int)RoleType.Scp93953, 0 },
            { (int)RoleType.Scp0492, 0 },
        };

        private static readonly Dictionary<RoleType, sbyte> PlayerWeight = new Dictionary<RoleType, sbyte>
        {
            { RoleType.Scp173, 3 },
            { RoleType.Scp049, 3 },
            { RoleType.Scp0492, 1 },
            { RoleType.Scp096, 3 },
            { RoleType.Scp93953, 3 },
            { RoleType.Scp93989, 3 },
            { RoleType.Scientist, 1 },
            { RoleType.NtfCaptain, 1 },
            { RoleType.NtfSpecialist, 1 },
            { RoleType.NtfSergeant, 1 },
            { RoleType.NtfPrivate, 1 },
            { RoleType.ChaosMarauder, 1 },
            { RoleType.ChaosRepressor, 1 },
            { RoleType.ChaosRifleman, 1 },
            { RoleType.ChaosConscript, 1 },
            { RoleType.ClassD, 1 },
        };

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
        }

        public static void OnRoundStarted()
        {
            OnEvacuateCooldownMTF = Time.time + 60;
            OnSupportCooldownMTF = Time.time + 60;
            OnEvacuateCooldownCHI = Time.time + 60;
            OnSupportCooldownCHI = Time.time + 60;
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerRequester = Player.Get((sender as CommandSender)?.SenderId);

            CommandMethods.LogCommandUsed((CommandSender)sender, CommandMethods.FormatArguments(arguments, 0));
            if (!((CommandSender)sender).CheckPermission("betterRP.toc"))
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: В ДОСТУПЕ ОТКАЗАНО</color>";
                return false;
            }

            if (!Round.IsStarted)
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: СЕССИЯ НЕ НАЧАЛАСЬ</color>";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = string.Join(
                    "\n",
                    "\n<color=#C1B5B5>СТАТУС: ИНФО</color>",
                    "<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ:</color><color=#990000>toc [ЗАДАЧА]</color>",
                    "<color=#C1B5B5>ЗАДАЧИ:</color>",
                    "<color=#990000>evacuate</color> <color=#FFD966>(СИНОНИМЫ: evac)</color> - Запрос эвакуационного транспорта.",
                    "<color=#990000>support</color> <color=#FFD966>(СИНОНИМЫ: sup)</color> - Запрос группы поддержки.");
                return false;
            }

            if (!playerRequester.IsHuman)
            {
#if ShowHints
                player_requester.ShowHint("<i>Вы не человек</i>", 5);
#endif
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ДОСТУП ОГРАНИЧЕН ДЛЯ ТЕКУЩЕЙ РОЛИ</color>";
                return true;
            }

#if UseRadio
            if (player_requester.CurrentItem.Type != ItemType.Radio)
            {
#if ShowHints
                player_requester.ShowHint("<i>Нужно достать рацию</i>", 5);
#endif
                response = "Нужно достать рацию";
                return true;
            }
#endif
            if (playerRequester.CurrentRoom.Type != RoomType.Surface)
            {
#if ShowHints
                player_requester.ShowHint("[Нет сигнала]", 5);
#endif
                response = string.Join(
                    "\n",
                    "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>",
                    "<color=#C1B5B5>ВЫВОД: НЕТ СИГНАЛА</color>");
                return true;
            }

            var args = arguments.Array;
            {
                if (args[1].ToLower().Equals("evac") || args[1].ToLower().Equals("evacuate"))
                {
                    if ((OnEvacuateCooldownMTF > Time.time && playerRequester.Team == Team.MTF) ||
                        (OnEvacuateCooldownCHI > Time.time && playerRequester.Team == Team.CHI))
                    {
                        response = "\"Говорит TOC. Транспорт ещё не готов. Ждите. Конец связи.\"";
                        return true;
                    }

                    if (Warhead.IsInProgress && Warhead.DetonationTimer < 36)
                    {
#if ShowHints
                        //TODO: Передать группу вызывающего
                        player_requester.ShowHint("\"Говорит Пилот Эпсилон-11. Вылет невозможен, буду в зоне поражения Альфа-боеголовки\"", 5);
#endif
                        response = "\"Говорит TOC. Эвакуация невозможна, вы в зоне поражения Альфа-боеголовки. Конец связи.\"";
                        return true;
                    }
#if ShowHints
                    player_requester.ShowHint("\"Говорит Пилот Эпсилон-11. Буду на месте через 30 секунд\"", 5);
#endif
                    if (playerRequester.Team == Team.CHI)
                    {
                        OnEvacuateCooldownCHI = Time.time + Plugin.Instance.Config.CooldownEv;
                        Timing.RunCoroutine(this.Evacuate(Team.CHI));
                    }
                    else if (playerRequester.Team == Team.MTF)
                    {
                        OnEvacuateCooldownMTF = Time.time + Plugin.Instance.Config.CooldownEv;
                        Timing.RunCoroutine(this.Evacuate(Team.MTF));
                    }

                    response = $"\"Говорит TOC. Транспорт будет на месте через {Plugin.Instance.Config.WaitForSupport / 2} секунд. Конец связи.\"";
                    return true;
                }

                if (args[1].ToLower().Equals("sup") || args[1].ToLower().Equals("support"))
                {
                    if ((OnSupportCooldownMTF > Time.time && playerRequester.Team == Team.MTF) ||
                        (OnSupportCooldownCHI > Time.time && playerRequester.Team == Team.CHI))
                    {
                        response = "\"Говорит TOC. Группа ещё не готова. Ждите. Конец связи.\"";
                        return true;
                    }

                    if (playerRequester.Team == Team.CHI)
                    {
                        OnSupportCooldownCHI = Time.time + Plugin.Instance.Config.CooldownSup;
                        Timing.RunCoroutine(this.WaitForSupport(Team.CHI));
                    }
                    else if (playerRequester.Team == Team.MTF)
                    {
                        OnSupportCooldownMTF = Time.time + Plugin.Instance.Config.CooldownSup;
                        Timing.RunCoroutine(this.WaitForSupport(Team.MTF));
                    }

                    response = $"\"Говорит TOC. Группа будет на месте через {Plugin.Instance.Config.WaitForSupport} секунд. Конец связи.\"";
                    return true;
                }
            }

            response = string.Join(
                    "\n",
                    "\n<color=#C1B5B5>СТАТУС:</color> <color=#990000>ОШИБКА</color>",
                    "<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ:</color><color=#990000>toc [ЗАДАЧА]</color>",
                    "<color=#C1B5B5>ЗАДАЧИ:</color>",
                    "<color=#990000>evacuate</color> <color=#FFD966>(СИНОНИМЫ: evac)</color> - Запрос эвакуационного транспорта.",
                    "<color=#990000>support</color> <color=#FFD966>(СИНОНИМЫ: sup)</color> - Запрос группы поддержки.");
            return false;
        }

        private IEnumerator<float> Evacuate(Team team)
        {
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.WaitForSupport/2);
            Vector3 point = Vector3.zero;
            List<Player> readyToEvac = new List<Player> { };
            int[,] evacQuery = new int[,] { };

            if (team == Team.MTF)
            {
                Respawn.PlayEffect(RespawnEffectType.SummonNtfChopper);
                yield return Timing.WaitForSeconds(18f);
                point = TOC.mtfPoint;
                readyToEvac = Player.List.Where(x => Vector3.Distance(x.Position, point) <= Plugin.Instance.Config.Distance).ToList();
                if (!Warhead.IsInProgress)
                {
                    readyToEvac = Player.List.Where(x => x.Team != Team.MTF || (x.Team == Team.MTF && x.IsCuffed)).ToList();
                }

                evacQuery = EvacQueryMTF;
            }
            else if (team == Team.CHI)
            {
                Respawn.PlayEffect(RespawnEffectType.SummonChaosInsurgencyVan);
                yield return Timing.WaitForSeconds(13f);
                point = TOC.chiPoint;
                readyToEvac = Player.List.Where(x => Vector3.Distance(x.Position, point) <= Plugin.Instance.Config.Distance).ToList();
                if (!Warhead.IsInProgress)
                {
                    readyToEvac = readyToEvac.Where(x => x.Team != Team.CHI || (x.Team == Team.CHI && x.IsCuffed)).ToList();
                }

                evacQuery = EvacQueryCHI;
            }

            sbyte capacity = 10;
            List<Pickup> itemsToEvac = Map.Pickups.ToList().Where(x => Vector3.Distance(x.Position, point) <= Plugin.Instance.Config.Distance && Plugin.Instance.Config.MTFEvacItems.Contains(x.Type)).ToList();
            foreach (var item in itemsToEvac)
            {
                item.Destroy();
            }

            for (int role = 0; role < evacQuery.GetLength(0); role++)
            {
                foreach (Player ply in readyToEvac.Where(x => x.Role == (RoleType)evacQuery[role, 0] && x.IsCuffed == Convert.ToBoolean(evacQuery[role, 1])))
                {
                    if (PlayerWeight[ply.Role] > capacity)
                    {
#if ShowHints
                        if(ply.Team == Team.MTF)
                            ply.ShowHint("\"Говорит Пилот Эпсилон-11. Погрузка окончена. Улетаем\"\n<i>Вам не хватило места</i>");
                        else
                            ply.ShowHint("<i>Вам не хватило места</i>");
                        continue;
                        //huita.
#endif
                    }

                    capacity -= PlayerWeight[ply.Role];
                    if (ply.Team == Team.SCP)
                    {
                        switch (ply.Role)
                        {
                            case RoleType.Scp049:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 0 4 9"));
                                break;
                            case RoleType.Scp096:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 0 9 6"));
                                break;
                            case RoleType.Scp173:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 1 7 3"));
                                break;
                            case RoleType.Scp93953:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 9 3 9"));
                                break;
                            case RoleType.Scp93989:
                                Cassie.Message(Plugin.Instance.Config.ScpEvacuate.Replace("{scpnumber}", "scp 9 3 9"));
                                break;
                        }
                    }
#if ShowHints
                    if (ply.Team == Team.MTF)
                        ply.ShowHint("\"Говорит Пилот Эпсилон-11. Погрузка окончена. Улетаем\"\n<i>Вы успешно эвакуировались из Комплекса</i>");
                    else
                        ply.ShowHint("<i>Вы успешно эвакуировались из Комплекса</i>");
#endif
                    ply.ClearInventory();
                    ply.SetRole(RoleType.Spectator);
                }
            }
        }

        private IEnumerator<float> WaitForSupport(Team team)
        {
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.WaitForSupport);
            if (team == Team.MTF)
            {
                Respawn.PlayEffect(RespawnEffectType.SummonNtfChopper);
                yield return Timing.WaitForSeconds(18f);
                Respawn.ForceWave(Respawning.SpawnableTeamType.NineTailedFox, false);
            }
            else if (team == Team.CHI)
            {
                Respawn.PlayEffect(RespawnEffectType.SummonChaosInsurgencyVan);
                yield return Timing.WaitForSeconds(13f);
                Respawn.ForceWave(Respawning.SpawnableTeamType.ChaosInsurgency, false);
            }
        }
    }
}
