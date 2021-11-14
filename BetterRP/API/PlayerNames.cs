namespace BetterRP.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Sets custom player name, role, etc.
    /// </summary>
    public class PlayerNames
    {
        /// <summary>
        /// Random Number Generator.
        /// </summary>
        private readonly Random rnd = new Random();
        private Dictionary<string, string> militaryRole = new Dictionary<string, string> { };
        private List<string> surnames = new List<string>();
        private List<string> callSigns = new List<string>();
        private List<string> sponsorsNames = new List<string>();
        private string names = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ";
        private int classDPart;

        /// <summary>
        /// Types of name bases.
        /// </summary>
        private enum NameBase
        {
            /// <summary>
            /// Used for Guards and Scientists.
            /// </summary>
            Surnames = 1,

            /// <summary>
            /// Used for MTF and CHI.
            /// </summary>
            CallSigns = 2,

            /// <summary>
            /// Used for MTF and CHI capitans.
            /// </summary>
            SponsorsNames = 3,
        }

        /// <summary>
        /// Gets or sets numbers of class-D, which have been spawned during one round.
        /// </summary>
        public static HashSet<string> ClassDperRound { get; set; } = new HashSet<string> { };

        /// <summary>
        /// Calls function which loads name bases before round starts.
        /// </summary>
        public void OnWaiting()
        {
            this.LoadNames();
            this.classDPart = UnityEngine.Random.Range(10, 100);
        }

        /// <summary>
        /// Calls function to set player name.
        /// </summary>
        /// <param name="ev">ChangingRoleEventArgs.</param>
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            Log.Debug("Starting SetName coroutine.", Plugin.Instance.Config.Debug);
            Plugin.Instance.NewCoroutine(this.SetName(ev.Player, ev.NewRole));
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            Log.Debug("Starting SetName coroutine.", Plugin.Instance.Config.Debug);
            Plugin.Instance.NewCoroutine(this.SetName(ev.Player, ev.RoleType));
        }

        private void LoadNames(List<NameBase> nameBase = null)
        {
            nameBase = nameBase ?? new List<NameBase> { NameBase.Surnames, NameBase.CallSigns, NameBase.SponsorsNames };
            foreach (var item in nameBase)
            {
                switch (item)
                {
                    case NameBase.Surnames:
                        Log.Debug($"Total surnames count out of handler: {Plugin.SurnameBase.Count()}", Plugin.Instance.Config.Debug);
                        if (Plugin.SurnameBase.Count > 0)
                        {
                            this.surnames = Plugin.SurnameBase;
                            this.surnames.ShuffleList();
                            Log.Debug($"Total surnames count inside handler: {this.surnames.Count()}", Plugin.Instance.Config.Debug);
                        }

                        break;
                    case NameBase.CallSigns:
                        Log.Debug($"Total callsigns count out of handler: {Plugin.CallSignsBase.Count()}", Plugin.Instance.Config.Debug);
                        if (Plugin.CallSignsBase.Count > 0)
                        {
                            this.callSigns = Plugin.CallSignsBase;
                            this.callSigns.ShuffleList();
                            Log.Debug($"Total callsigns count inside handler: {this.callSigns.Count()}", Plugin.Instance.Config.Debug);
                        }

                        break;
                    case NameBase.SponsorsNames:
                        Log.Debug($"Total sponsors names count out of handler: {Plugin.SponsorsNamesBase.Count()}", Plugin.Instance.Config.Debug);
                        if (Plugin.CallSignsBase.Count > 0)
                        {
                            this.sponsorsNames = Plugin.SponsorsNamesBase;
                            this.sponsorsNames.ShuffleList();
                            Log.Debug($"Total sponsors names count inside handler: {this.sponsorsNames.Count()}", Plugin.Instance.Config.Debug);
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        private IEnumerator<float> NameChangeDelay(Player player)
        {
            string customInfo = player.CustomInfo;
            string displayNickname = player.DisplayNickname;
            player.CustomInfo = string.Empty;
            player.DisplayNickname = string.Empty;
            yield return MEC.Timing.WaitForSeconds(10f);
            if (player == null)
            {
                yield break;
            }

            if (player.IsAlive && player.Role != RoleType.Scp0492)
            {
                yield break;
            }

            if (!player.IsAlive)
            {
                player.CustomInfo = string.Empty;
                player.DisplayNickname = string.Empty;
                this.militaryRole.Remove(player.UserId);

                yield break;
            }

            if (this.militaryRole.TryGetValue(player.UserId, out string cInfo))
            {
                player.CustomInfo = cInfo;
                this.militaryRole.Remove(player.UserId);
            }
            else
            {
                player.CustomInfo = customInfo;
            }

            player.DisplayNickname = displayNickname;
            yield break;
        }

        private IEnumerator<float> SetName(Player player, RoleType role)
        {
            yield return MEC.Timing.WaitForSeconds(0.1f);
            try
            {
                if (Plugin.Instance.Config.PlayerNamesEnabled && player != null && role != RoleType.None)
                {
                    string selectedname;
                    this.militaryRole.Remove(player.UserId);
                    if (player.Team != Team.SCP)
                    {
                        switch (role)
                        {
                            case RoleType.ClassD:
                                if (ClassDperRound.Count > 800)
                                {
                                    this.classDPart++;
                                    ClassDperRound.Clear();
                                }

                                string number;
                                for (; ; )
                                {
                                    number = $"{UnityEngine.Random.Range(0, 1000)}";
                                    if (UnityEngine.Random.Range(0, 100) < 90)
                                    {
                                        number = "D-" + this.classDPart + string.Join(string.Empty, new string('0', 3 - number.Length), number);
                                    }
                                    else
                                    {
                                        number = "D-" + this.rnd.Next(10000, 99999).ToString();
                                    }

                                    if (!ClassDperRound.Contains(number))
                                    {
                                        break;
                                    }
                                }

                                ClassDperRound.Add(number);
                                player.DisplayNickname = number;
                                break;
                            case RoleType.Scientist:
                            case RoleType.FacilityGuard:
                                if (this.surnames.Count <= 0)
                                {
                                    this.LoadNames(new List<NameBase> { NameBase.Surnames, });
                                }

                                selectedname = this.surnames[this.rnd.Next(this.surnames.Count)];
                                this.surnames.Remove(selectedname);

                                player.DisplayNickname = (player.Role == RoleType.Scientist ? "д-р " : string.Empty) +
                                    $"{selectedname} {this.names[UnityEngine.Random.Range(0, this.names.Length)]}.";
                                player.CustomInfo = Plugin.Instance.Config.RoleNames[player.Role][player.Inventory.UserInventory.Items.Values.Where(
                                    i => i.Category == ItemCategory.Keycard).Count() == 0 ? ItemType.None :
                                    player.Inventory.UserInventory.Items.Values.Where(
                                    i => i.Category == ItemCategory.Keycard).First().ItemTypeId];
                                break;
                            default:
                                if (player.Team == Team.CHI || player.Team == Team.MTF)
                                {
                                    if ((player.Role == RoleType.NtfCaptain || player.Role == RoleType.ChaosMarauder) && this.sponsorsNames.Count > 0)
                                    {
                                        selectedname = this.sponsorsNames[this.rnd.Next(this.sponsorsNames.Count)];
                                        this.sponsorsNames.Remove(selectedname);
                                    }
                                    else
                                    {
                                        if (this.callSigns.Count <= 0)
                                        {
                                            this.LoadNames(new List<NameBase> { NameBase.CallSigns, });
                                        }

                                        selectedname = this.callSigns[this.rnd.Next(this.callSigns.Count)];
                                        this.callSigns.Remove(selectedname);
                                    }

                                    string roleName = Plugin.Instance.Config.RoleNames[player.Role][player.Inventory.UserInventory.Items.Values.Where(
                                        i => i.Category == ItemCategory.Keycard).Count() == 0 ? ItemType.None :
                                        player.Inventory.UserInventory.Items.Values.Where(
                                        i => i.Category == ItemCategory.Keycard).First().ItemTypeId].Replace("%UNIT", player.UnitName);

                                    if (this.militaryRole.TryGetValue(player.UserId, out string prevRole))
                                    {
                                        this.militaryRole[player.UserId] = roleName;
                                    }
                                    else
                                    {
                                        this.militaryRole.Add(player.UserId, roleName);
                                    }

                                    player.DisplayNickname = $"'{selectedname}'";
                                    break;
                                }
                                else
                                {
                                    Plugin.Instance.NewCoroutine(this.NameChangeDelay(player));
                                }

                                break;
                        }

                        Log.Debug($"Player Name: {player.Nickname}\nPlayer Nickname: {player.DisplayNickname}\nPlayer Custom Info: {player.CustomInfo}", Plugin.Instance.Config.Debug);
                    }
                    else
                    {
                        if (role != RoleType.Scp0492)
                        {
                            player.CustomInfo = string.Empty;
                            player.DisplayNickname =
                                Plugin.Instance.Config.RoleNames[player.Role][player.Inventory.UserInventory.Items.Values.Where(
                                i => i.Category == ItemCategory.Keycard).Count() == 0 ? ItemType.None :
                                player.Inventory.UserInventory.Items.Values.Where(
                                i => i.Category == ItemCategory.Keycard).First().ItemTypeId];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"An error occured while trying to set player info:\n{e}");
            }
        }
    }
}
