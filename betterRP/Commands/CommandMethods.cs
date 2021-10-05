namespace BetterRP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Exiled.API.Features;
    using NorthwoodLib.Pools;

    public class CommandMethods
    {
        // Должно быть приватным, возможно но без static работать не будет. Вызывать нужно в классах команд
        private readonly Plugin plugin;

        public CommandMethods(Plugin plugin) => this.plugin = plugin;

        public static void LogCommandUsed(CommandSender sender, string command)
        {
            string data =
                $"{DateTime.Now}: {sender.Nickname} ({sender.SenderId}) executed: {command} {Environment.NewLine}";
            File.AppendAllText(Paths.Log, data);
        }

        public static string FormatArguments(ArraySegment<string> sentence, int index)
        {
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            foreach (string word in sentence.Segment(index))
            {
                sb.Append(word);
                sb.Append(" ");
            }

            StringBuilderPool.Shared.Return(sb);
            return sb.ToString();
        }
    }
}
