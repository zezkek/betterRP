using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirror;
using betterRP;

namespace betterRP.Handlers
{
    public class ReplaceSCP
    {
        Random rnd = new Random();
        public void OnDisconnect(LeftEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                var SCP = ev.Player.Role;
                Log.Info(SCP.ToString());
                var Position = ev.Player.Position;
                Log.Info(Position.y);
                List<Player> Players = Player.List.Where(x => x.IsDead).ToList();
                int randomplayer = rnd.Next(Players.Count);
                Players[randomplayer].Role = SCP;
                Players[randomplayer].Position = Position;
            }
        }
    }
}
