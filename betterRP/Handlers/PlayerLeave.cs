namespace BetterRP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BetterRP;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using Mirror;

    public class PlayerLeave
    {
        private readonly Random rnd = new Random();

        public void OnDisconnect(LeftEventArgs ev)
        {
            // Проверить и, в случае ошибки, добавить замену для SCP-079
            if (ev.Player.IsScp && Player.List.Where(p => p.IsDead).Count() > 0)
            {
                var scp = ev.Player.Role;
                Log.Debug(scp.ToString(), Plugin.PluginItem.Config.Debug);
                var position = ev.Player.Position;
                Log.Debug(position.y, Plugin.PluginItem.Config.Debug);
                List<Player> players = Player.List.Where(x => x.IsDead).ToList();
                int randomplayer = this.rnd.Next(players.Count);
                players[randomplayer].Role = scp;
                players[randomplayer].Position = position;
            }
            else if (!ev.Player.IsScp)
            {
                ev.Player.Kill(DamageTypes.Scp049);
            }
        }
    }
}
