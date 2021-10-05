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

        [Description("New name hint message time")]
        public float HintDisplayTime { get; set; } = 10;

        public string ItemConfigFolder { get; set; } = Path.Combine(Paths.Configs, "BetterRP");

        public float BlackoutDelay { get; set; } = 15f;

        [Description("New Science Names")]
        public List<string> ScientistAndSecuritySurname { get; set; } = new List<string>
        {
            "Адамс", "Элисон",
            "Андерсон", "Бейкер",
            "Барретт", "Белл",
            "Беннетт", "Блэк",
            "Брукс", "Картер",
            "Кларк", "Коул",
            "Купер", "Дэвис",
            "Эдвардс", "Эллис",
            "Эванс", "Фишер",
            "Фостер", "Гибсон",
            "Харрис", "Харт",
            "Говард", "Льюис",
            "Линн", "Митчел",
            "Петерс", "Рэй",
            "Робертс",
        };
    }
}
