using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace RolePlayNames
{
    public class Handlers
    {
        public Handlers handlers;
        Random rnd = new Random();
        string[] ScientistSurname = File.ReadAllLines(@"Names\ScientistSurnames.txt");
        public void OnChangedRole(ChangedRoleEventArgs ev)
        { 
            switch (ev.Player.Role)
            {
                case RoleType.ClassD:
                    ev.Player.DisplayNickname = ("D-" + rnd.Next(10000, 99999));
                    break;
                case RoleType.Scientist:
                    var MaxKeycard = ItemType.KeycardO5;
                    if (ev.Player.HasItem(ItemType.KeycardScientist))
                        MaxKeycard = ItemType.KeycardScientist;
                    if (ev.Player.HasItem(ItemType.KeycardZoneManager))
                        MaxKeycard = ItemType.KeycardZoneManager;
                    if (ev.Player.HasItem(ItemType.KeycardScientistMajor))
                        MaxKeycard = ItemType.KeycardScientistMajor;
                    if (ev.Player.HasItem(ItemType.KeycardContainmentEngineer))
                        MaxKeycard = ItemType.KeycardContainmentEngineer;
                    if (ev.Player.HasItem(ItemType.KeycardFacilityManager))
                        MaxKeycard = ItemType.KeycardFacilityManager;
                    switch (MaxKeycard)
                    {
                        case ItemType.KeycardO5:
                            ev.Player.DisplayNickname = ("Стажер " + ScientistSurname[rnd.Next(ScientistSurname.Length)]);
                            break;
                        case ItemType.KeycardScientist:
                            ev.Player.DisplayNickname = ("Д-р " + ScientistSurname[rnd.Next(ScientistSurname.Length)]);
                            break;
                        case ItemType.KeycardZoneManager:
                            ev.Player.DisplayNickname = ("Руководитель зоны д-р " + ScientistSurname[rnd.Next(ScientistSurname.Length)]);
                            break;
                        case ItemType.KeycardScientistMajor:
                            ev.Player.DisplayNickname = ("Ст. научный сотрудник " + ScientistSurname[rnd.Next(ScientistSurname.Length)]);
                            break;
                        case ItemType.KeycardContainmentEngineer:
                            ev.Player.DisplayNickname = ("Инженер камер содержания " + ScientistSurname[rnd.Next(ScientistSurname.Length)]);
                            break;
                        case ItemType.KeycardFacilityManager:
                            ev.Player.DisplayNickname = ("Руководитель комплекса д-р " + ScientistSurname[rnd.Next(ScientistSurname.Length)]);
                            break;
                    }
                    break;
                case RoleType.Spectator:
                    ev.Player.DisplayNickname = ev.Player.Nickname;
                    break;
            }
        }
    }
}
