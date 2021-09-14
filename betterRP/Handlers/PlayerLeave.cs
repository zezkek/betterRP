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

using Exiled.CustomItems.API.Features;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Spawn;

namespace betterRP
{
    public class PlayerLeave
    {
        Random rnd = new Random();
        public void OnDisconnect(LeftEventArgs ev)
        {
            if (ev.Player.IsScp && Player.List.Where(p => p.IsDead).Count() > 0)
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
            else if(!ev.Player.IsScp)
            {
                ev.Player.Kill(DamageTypes.Scp049);
            }
                
        }
    }
}
