namespace BetterRP.Commands.Do
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]

    public class Do : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Do"/> class.
        /// </summary>
        public Do() => this.LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "do";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public override string Description { get; } = "Вывод РП-сообщения игрокам поблизости";

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerRequester = Player.Get((sender as CommandSender)?.SenderId);

            CommandMethods.LogCommandUsed((CommandSender)sender, CommandMethods.FormatArguments(arguments, 0));
            if (!((CommandSender)sender).CheckPermission("betterRP.do"))
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: В ДОСТУПЕ ОТКАЗАНО</color>";
                return false;
            }

            if (!playerRequester.IsAlive)
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ДОСТУП ОГРАНИЧЕН ДЛЯ НАБЛЮДАТЕЛЕЙ</color>";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ:</color><color=#990000>do ВАШЕ СООБЩЕНИЕ</color>";
                return false;
            }

            switch (arguments.At(0))
            {
                default:
                    string msg = string.Empty;
                    if (playerRequester.DisplayNickname != null && playerRequester.DisplayNickname != string.Empty)
                    {
                        msg = $"{playerRequester.DisplayNickname}";
                    }
                    else
                    {
                        msg = $"{playerRequester.Nickname}";
                    }

                    msg += ": " + string.Join(" ", arguments.Segment(0));
                    msg = "Действие " + Regex.Replace(msg, "<[^>]*>", string.Empty);

                    if (msg.Length - 14 > 100)
                    {
                        response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ: ПРЕВЫШЕНО КОЛИЧЕСТВО СИМВОЛОВ (100)</color>";
                        return false;
                    }

                    Log.Info(msg);
                    foreach (Player player in Player.List.Where(p => (((p.Role != RoleType.Spectator || p.Role != RoleType.None) && UnityEngine.Vector3.Distance(playerRequester.Position, p.Position) <= 10) || p.IsOverwatchEnabled)))
                    {
                        player.Broadcast(7, "<size=25><color=#C1B5B5>" + msg + "</color></size>");
                    }

                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#6aa84f>УСПЕШНО</color>\n<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ: СООБЩЕНИЕ ОТПРАВЛЕНО</color>";
                    return true;
            }
        }
    }
}
