﻿using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace betterRP
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("Enable or disable name change")]
        public bool PlayerNamesEnabled { get; set; } = true;
        [Description("Enable or disable player resize")]
        public bool PlayerResizeEnabled { get; set; } = true;
        [Description("New name hint message time")]
        public float HintDisplayTime { get; set; } = 10;
<<<<<<< HEAD
        [Description("Jumping drains stamina")]
        public float JumpDrainStamina { get; set; } = 20;
=======
        /* Отключено до исправления
         * [Description("Distance between players and receivers of DoMessage")]
         * public float DoDistance { get; set; } = 10;
         */
>>>>>>> parent of 8bf8491 (Merge branch 'master' into devM)
        [Description("New Science Names")]
        public List<string> ScientistAndSecuritySurname { get; set; } = new List<string> { 
            "Адамс","Элисон",
            "Андерсон","Бейкер",
            "Барретт","Белл",
            "Беннетт","Блэк",
            "Брукс","Картер",
            "Кларк","Коул",
            "Купер","Дэвис",
            "Эдвардс","Эллис",
            "Эванс","Фишер",
            "Фостер","Гибсон",
            "Харрис","Харт",
            "Говард","Льюис",
            "Линн","Митчел",
            "Петерс","Рэй",
            "Робертс"};
        [Description("New MTF Names")]
        public List<string> MTFNames { get; set; } = new List<string>
        {
            "Сансаныч",
            "Намбер",
            "Бомж",
            "Ангра",
            "Феня",
            "Кость",
            "Молоко",
            "Яникс",
            "Лем"
        };
        public List<string> CHINames { get; set; } = new List<string>
        {
            "Сансаныч",
            "Намбер",
            "Бомж",
            "Ангра",
            "Феня",
            "Кость",
            "Молоко",
            "Яникс",
            "Лем"
        };
    }
}
