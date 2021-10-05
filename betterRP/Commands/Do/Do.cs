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
                response = "Ошибка. В доступе отказано";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Использование команды: do ВАШЕ СООБЩЕНИЕ";
                return false;
            }

            switch (arguments.At(0))
            {
                default:
                    var msg = $"{playerRequester.Nickname}: ";
                    msg += string.Join(" ", arguments.Segment(0));
                    msg = Regex.Replace(msg, "<[^>]*>", string.Empty);
                    foreach (Player player in Player.List.Where(p => (p.Role != RoleType.Spectator || p.Role != RoleType.None)
                    && UnityEngine.Vector3.Distance(playerRequester.Position, p.Position) <= 10 /*Ёбанный хардкод, хуй знает, как передать правильно сюда обьект класса Plugin*/))
                    {
                        player.Broadcast(15, msg);
                    }

                    Log.Info(msg);

                    response = "Сообщение отправлено";
                    return true;
            }
        }
    }
}
