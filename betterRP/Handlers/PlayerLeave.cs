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

    class PlayerLastInfo
    {
        public string id { get; set; }

        public RoleType RoleType { get; set; }

        public UnityEngine.Vector3 Position { get; set; } = UnityEngine.Vector3.zero;

        public float Health { get; set; } = 0;

        public float MaxHealth { get; set; }

        public float AHP { get; set; }

        public float MaxAHP { get; set; }

        public byte Tier { get; set; } = 0;

        public int Exp { get; set; } = 0;

        public int Energy { get; set; } = 0;
    }

    public class PlayerLeave
    {
        private readonly Random rnd = new Random();
        private Dictionary<string, PlayerLastInfo> syncInfoDict = new Dictionary<string, PlayerLastInfo> { };

        private IEnumerator<float> SyncInfo()
        {
            while (Round.IsStarted)
            {
                foreach (var player in Player.List.Where(x => x.Team == Team.SCP && x.Role != RoleType.Scp0492))
                {
                    if (this.syncInfoDict.TryGetValue(player.UserId, out PlayerLastInfo info)
                    {
                        if (player.Role != RoleType.Scp079)
                        {
                            syncInfoDict[player.UserId].
                        }
                    }
                }

                yield return MEC.Timing.WaitForSeconds(1f);
            }

            this.syncInfoDict.Clear();
        }
        public void OnDisconnect(LeftEventArgs ev)
        {
            Log.Warn($"Nickname: {ev.Player.Nickname}\nRoom: {ev.Player.CurrentRoom}\nAbs.pos: {ev.Player.Position}\nRole: {ev.Player.Role}\nHP: {ev.Player.Health} AHP: {ev.Player.ArtificialHealth}");
            // Проверить и, в случае ошибки, добавить замену для SCP-079
            if (ev.Player.IsScp && Player.List.Where(p => p.IsDead).Count() > 0)
            {
                RoleType role = ev.Player.Role;

                var position = ev.Player.Position;
                List<Player> players = Player.List.Where(x => x.IsDead).ToList();
                int randomplayer = this.rnd.Next(players.Count);
                players[randomplayer].Role = role;
                players[randomplayer].Position = position;
            }
            else if (!ev.Player.IsScp)
            {
                ev.Player.Kill(DamageTypes.Scp049);
            }
        }
    }
}
