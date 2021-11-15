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
                    string msgAuthorDisplayNickname = string.Empty;
                    string msgAdminInfo = $"пользователя: {playerRequester.Nickname}. ID: {playerRequester.Id}\nРОЛЬ: {playerRequester.Role}.";
                    if (playerRequester.DisplayNickname != null && playerRequester.DisplayNickname != string.Empty)
                    {
                        msgAuthorDisplayNickname = playerRequester.DisplayNickname;
                        msgAdminInfo += $" РП-ИМЯ: \"{playerRequester.DisplayNickname}\"";
                    }
                    else
                    {
                        msgAuthorDisplayNickname = playerRequester.Nickname;
                    }

                    string msg = Regex.Replace(string.Join(" ", arguments.Segment(0)), "<[^>]*>", string.Empty);

                    if (msg.Length > 100)
                    {
                        response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ: ПРЕВЫШЕНО КОЛИЧЕСТВО СИМВОЛОВ (100)</color>";
                        return false;
                    }

                    Log.Info($"Действие {msgAdminInfo}: {msg}");
                    foreach (Player player in Player.List.Where(p => ((p.Role != RoleType.Spectator || p.Role != RoleType.None) && UnityEngine.Vector3.Distance(playerRequester.Position, p.Position) <= 10) || p.IsOverwatchEnabled))
                    {
                        player.Broadcast(7, $"<size=30><color=#C1B5B5>Действие {(player.IsOverwatchEnabled ? msgAdminInfo + ":\n" : msgAuthorDisplayNickname + ": ")}{msg}</color></size>");
                    }

                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#6aa84f>УСПЕШНО</color>\n<color=#C1B5B5>ИСПОЛЬЗОВАНИЕ КОМАНДЫ: СООБЩЕНИЕ ОТПРАВЛЕНО</color>";
                    return true;
            }
        }
    }
}
