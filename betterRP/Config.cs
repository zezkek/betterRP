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

        [Description("Enable or disable player resize")]
        public bool PlayerResizeEnabled { get; set; } = true;

        public string ItemConfigFolder { get; set; } = Path.Combine(Paths.Configs, "BetterRP");

        public float BlackoutDelay { get; set; } = 15f;

        [Description("TOC CONFIG THERE. Evacuation Cooldown")]
        public int CooldownEv { get; set; } = 150;

        [Description("Cooldown of support")]
        public int CooldownSup { get; set; } = 300;

        [Description("Time to wait for support")]
        public float TimeInTheWay { get; set; } = 60;

        [Description("Max distance between SCP and player while containming")]
        public float Distance { get; set; } = 10f;

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

        [Description("Are cards diabling tesla")]
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
