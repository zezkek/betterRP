using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwoodLib.Pools;
using System.IO;
using Exiled.API.Features;

namespace betterRP
{
    public class CommandMethods
    {
        //Должно быть приватным, возможно но без static работать не будет. Вызывать нужно в классах команд
        private readonly Plugin plugin;
        public CommandMethods(Plugin plugin) => this.plugin = plugin;
        public static void LogCommandUsed(CommandSender sender, string Command)
        {
            string data =
                $"{DateTime.Now}: {sender.Nickname} ({sender.SenderId}) executed: {Command} {Environment.NewLine}";
            File.AppendAllText(Paths.Log, data);
        }
        public static string FormatArguments(ArraySegment<string> sentence, int index)
        {
            StringBuilder SB = StringBuilderPool.Shared.Rent();
            foreach (string word in sentence.Segment(index))
            {
                SB.Append(word);
                SB.Append(" ");
            }
            string msg = SB.ToString();
            StringBuilderPool.Shared.Return(SB);
            return msg;
        }
    }
}
