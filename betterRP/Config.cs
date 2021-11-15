namespace BetterRP
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Enable or disable debug mode")]
        public bool Debug { get; set; } = false;

        [Description("Enable or disable name change")]
        public bool PlayerNamesEnabled { get; set; } = true;

        public Dictionary<RoleType, string> ColorsPerRoles { get; set; } = new Dictionary<RoleType, string>
        {
            [RoleType.ChaosConscript] = "6aa84f",
            [RoleType.ChaosMarauder] = "38761d",
            [RoleType.ChaosRepressor] = "274e13",
            [RoleType.ChaosRifleman] = "274e13",
            [RoleType.ClassD] = "CE7E00",
            [RoleType.FacilityGuard] = "757575",
            [RoleType.NtfCaptain] = "000080",
            [RoleType.NtfPrivate] = "8BC5FF",
            [RoleType.NtfSergeant] = "0180DA",
            [RoleType.NtfSpecialist] = "0180DA",
            [RoleType.Scientist] = "FFD966",
            [RoleType.Scp049] = "990000",
            [RoleType.Scp0492] = "990000",
            [RoleType.Scp079] = "990000",
            [RoleType.Scp096] = "990000",
            [RoleType.Scp106] = "990000",
            [RoleType.Scp173] = "990000",
            [RoleType.Scp93953] = "990000",
            [RoleType.Scp93989] = "990000",
            [RoleType.Tutorial] = "6aa84f",
            [RoleType.Spectator] = "C1B5B5",
            [RoleType.None] = "C1B5B5f",
        };

        public Dictionary<RoleType, Dictionary<ItemType, string>> RoleNames { get; set; } = new Dictionary<RoleType, Dictionary<ItemType, string>>
        {
            [RoleType.ChaosConscript] = new Dictionary<ItemType, string> { [ItemType.KeycardChaosInsurgency] = "Новобранец Хаоса" },
            [RoleType.ChaosMarauder] = new Dictionary<ItemType, string> { [ItemType.KeycardChaosInsurgency] = "Мародёр Хаоса" },
            [RoleType.ChaosRepressor] = new Dictionary<ItemType, string> { [ItemType.KeycardChaosInsurgency] = "Репрессор Хаоса" },
            [RoleType.ChaosRifleman] = new Dictionary<ItemType, string> { [ItemType.KeycardChaosInsurgency] = "Стрелок Хаоса" },
            [RoleType.ClassD] = new Dictionary<ItemType, string> { [ItemType.None] = "Класс-D", },
            [RoleType.FacilityGuard] = new Dictionary<ItemType, string>
            {
                [ItemType.KeycardGuard] = "Младший сотрудник СБ",
                [ItemType.KeycardNTFOfficer] = "Сотрудник СБ",
                [ItemType.KeycardNTFLieutenant] = "Старший сотрудник СБ",
                [ItemType.KeycardNTFCommander] = "Начальник СБ",
            },
            [RoleType.NtfCaptain] = new Dictionary<ItemType, string> { [ItemType.KeycardNTFCommander] = "Командир %UNIT", },
            [RoleType.NtfPrivate] = new Dictionary<ItemType, string> { [ItemType.KeycardNTFOfficer] = "Капрал %UNIT", },
            [RoleType.NtfSergeant] = new Dictionary<ItemType, string> { [ItemType.KeycardNTFLieutenant] = "Сержант %UNIT", },
            [RoleType.NtfSpecialist] = new Dictionary<ItemType, string>
            {
                [ItemType.KeycardContainmentEngineer] = "Специалист ВУС %UNIT",
                [ItemType.KeycardFacilityManager] = "Специалист ВУС %UNIT",
            },
            [RoleType.Scientist] = new Dictionary<ItemType, string>
            {
                [ItemType.None] = "Научный сотрудник",
                [ItemType.KeycardScientist] = "Научный сотрудник",
                [ItemType.KeycardResearchCoordinator] = "Координатор исследований",
                [ItemType.KeycardContainmentEngineer] = "Инженер Комплекса",
                [ItemType.KeycardZoneManager] = "Заведующий Зоны",
                [ItemType.KeycardFacilityManager] = "Заведующий Комплекса",
            },
            [RoleType.Scp049] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp0492] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp079] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp096] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp106] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp173] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp93953] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Scp93989] = new Dictionary<ItemType, string> { [ItemType.None] = "[ДАННЫЕ УДАЛЕНЫ]", },
            [RoleType.Tutorial] = new Dictionary<ItemType, string> { [ItemType.None] = "Обучение", },
            [RoleType.Spectator] = new Dictionary<ItemType, string> { [ItemType.None] = "Наблюдатель", },
            [RoleType.None] = new Dictionary<ItemType, string> { [ItemType.None] = "None", },
        };

        public Dictionary<RoleType, string> NameType { get; set; } = new Dictionary<RoleType, string>
        {
            [RoleType.ChaosConscript] = "ПОЗЫВНОЙ",
            [RoleType.ChaosMarauder] = "ПОЗЫВНОЙ",
            [RoleType.ChaosRepressor] = "ПОЗЫВНОЙ",
            [RoleType.ChaosRifleman] = "ПОЗЫВНОЙ",
            [RoleType.ClassD] = "НОМЕР",
            [RoleType.FacilityGuard] = "ФИО",
            [RoleType.NtfCaptain] = "ПОЗЫВНОЙ",
            [RoleType.NtfPrivate] = "ПОЗЫВНОЙ",
            [RoleType.NtfSergeant] = "ПОЗЫВНОЙ",
            [RoleType.NtfSpecialist] = "ПОЗЫВНОЙ",
            [RoleType.Scientist] = "ФИО",
            [RoleType.Scp049] = "РОЛЬ",
            [RoleType.Scp0492] = "РОЛЬ",
            [RoleType.Scp079] = "РОЛЬ",
            [RoleType.Scp096] = "РОЛЬ",
            [RoleType.Scp106] = "РОЛЬ",
            [RoleType.Scp173] = "РОЛЬ",
            [RoleType.Scp93953] = "РОЛЬ",
            [RoleType.Scp93989] = "РОЛЬ",
            [RoleType.Tutorial] = "РОЛЬ",
            [RoleType.Spectator] = "РОЛЬ",
            [RoleType.None] = "None",
        };

        public List<RoleType> WarheadWarning { get; set; } = new List<RoleType>
        {
            RoleType.NtfCaptain,
            RoleType.NtfSpecialist,
            RoleType.NtfSergeant,
            RoleType.NtfPrivate,
        };

        public List<RoleType> DirectTOC { get; set; } = new List<RoleType>
        {
            RoleType.NtfCaptain,
            RoleType.ChaosMarauder,
        };

        public List<RoomType> RoomsWithConnToTOC { get; set; } = new List<RoomType>
        {
            RoomType.Surface,
        };

        public Dictionary<RoleType, Dictionary<string, List<ItemType>>> ItemsPerSubroles { get; set; } = new Dictionary<RoleType, Dictionary<string, List<ItemType>>>
        {
            [RoleType.Scientist] = new Dictionary<string, List<ItemType>>
            {
                ["Заведующий Зоны"] = new List<ItemType>
                {
                    ItemType.Radio,
                },
                ["Заведующий Комплекса"] = new List<ItemType>
                {
                    ItemType.Radio,
                },
                ["Инженер Комплекса"] = new List<ItemType>
                {
                    ItemType.Radio,
                },
            },
        };

        [Description("Enable or disable player resize")]
        public bool PlayerResizeEnabled { get; set; } = true;

        public string ItemConfigFolder { get; set; } = Path.Combine(Paths.Configs, "BetterRP");

        public float BlackoutDelay { get; set; } = 15f;

        [Description("### TOC CONFIG THERE. Evacuation Cooldown ###")]
        public int CooldownEv { get; set; } = 150;

        [Description("Cooldown of support")]
        public int CooldownSup { get; set; } = 300;

        [Description("Time to wait for support")]
        public float TimeInTheWay { get; set; } = 60;

        [Description("Max distance between SCP and player while containming")]
        public float EvacuationDistance { get; set; } = 10f;

        [Description("Ignored roles")]
        public List<RoleType> Roles { get; set; } = new List<RoleType> { RoleType.Scp079, RoleType.Scp106 };

        [Description("Cassie after scp evacuation by MTF")]
        public string ScpEvacuate { get; set; } = "{scpnumber} Evacuate Successfully";

        [Description("Cassie after scp evacuation by CHI")]
        public string ScpSteal { get; set; } = "{scpnumber} is lost from possible scan zone";

        [Description("Items that mtf can evacuate")]
        public List<ItemType> MTFEvacItems { get; set; } = new List<ItemType>
        {
            ItemType.KeycardChaosInsurgency, ItemType.KeycardContainmentEngineer,
            ItemType.KeycardFacilityManager, ItemType.KeycardNTFCommander,
            ItemType.KeycardO5, ItemType.MicroHID,
            ItemType.SCP018, ItemType.SCP207,
            ItemType.SCP268, ItemType.SCP500,
        };

        [Description("Items that chaos can evacuate")]
        public List<ItemType> CHIEvacItems { get; set; } = new List<ItemType>
        {
            ItemType.KeycardFacilityManager, ItemType.KeycardO5,
            ItemType.SCP018, ItemType.SCP207,
            ItemType.SCP268, ItemType.SCP500,
            ItemType.MicroHID,
        };

        [Description("### TESLA CONFIG THERE.Are cards diabling tesla ###")]
        public bool IsCardDisbaleTesla { get; set; } = true;

        [Description("List of cards that disables tesla")]
        public List<ItemType> Cards { get; set; } = new List<ItemType>
        {
            ItemType.KeycardResearchCoordinator, ItemType.KeycardGuard,
            ItemType.KeycardNTFOfficer, ItemType.KeycardNTFLieutenant,
            ItemType.KeycardNTFCommander, ItemType.KeycardFacilityManager,
            ItemType.KeycardContainmentEngineer,
        };

        [Description("Distance between tesla and player with card to prevent tesla from charging")]
        public float DistanceToDisable { get; set; } = 10;
    }
}
