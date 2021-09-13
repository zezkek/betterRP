using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System.Text.RegularExpressions;


namespace betterRP.Commands.Do
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Do : ParentCommand
    {
        public Do() => LoadGeneratedCommands();

        public override string Command { get; } = "do";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Вывод РП-сообщения игрокам поблизости";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player_requester = Player.Get((sender as CommandSender)?.SenderId);

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
                    var msg = $"{player_requester.Nickname}: ";
                    msg += string.Join(" ", arguments.Segment(0));
                    msg = Regex.Replace(msg, "<[^>]*>", "");
                    foreach (Player Ply in Player.List.Where(p => (p.Role != RoleType.Spectator || p.Role != RoleType.None) 
                    && UnityEngine.Vector3.Distance(player_requester.Position, p.Position) <= 10 /*Ёбанный хардкод, хуй знает, как передать правильно сюда обьект класса Plugin*/))
                        Ply.Broadcast(15, msg);
                    Log.Info(msg);

                    response = "Сообщение отправлено";
                    return true;
            }
        }
    }
}
