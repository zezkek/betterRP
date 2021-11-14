// #define Cuff
namespace BetterRP.Commands.Cuff
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Cuff : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cuff"/> class.
        /// </summary>
        public Cuff() => this.LoadGeneratedCommands();

        /// <inheritdoc/>
        public override string Command { get; } = "raycastinfo";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = new string[] { };

        /// <inheritdoc/>
        public override string Description { get; } = "GetRayCastInfo";

        /// <inheritdoc/>
        public override void LoadGeneratedCommands()
        {
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerRequester = Player.Get((sender as CommandSender)?.SenderId);
#if Cuff
            float distance = 3f;
#else
            float distance = 1000f;
#endif

            CommandMethods.LogCommandUsed((CommandSender)sender, CommandMethods.FormatArguments(arguments, 0));
            if (!((CommandSender)sender).CheckPermission("betterRP.raycastinfo"))
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: В ДОСТУПЕ ОТКАЗАНО</color>";
                return false;
            }

            if (!playerRequester.IsAlive || playerRequester.Team == Team.SCP)
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ДОСТУП ОГРАНИЧЕН ДЛЯ НАБЛЮДАТЕЛЕЙ</color>";
                return false;
            }

            if (!Physics.Raycast(playerRequester.CameraTransform.position, playerRequester.CameraTransform.forward, out RaycastHit raycastthit, distance))
            {
                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ОБЪЕКТ НЕ НАЙДЕН В ЗОНЕ ДОСЯГАЕМОСТИ</color>";
                return false;
            }
#if Cuff
            else if (Player.Get(raycastthit.transform.gameObject) == null)
            {
                response = string.Join(
                    "\n",
                    $"\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>",
                    $"<color=#C1B5B5>ВЫВОД: ОБЪЕКТ НЕ РАСПОЗНАН</color>");
#else
            else
            {
                response = string.Join(
                    "\n",
                    $"Current raycast object:",
                    $"raycastthit.barycentricCoordinate: {raycastthit.barycentricCoordinate}",
                    $"raycastthit.collider: {raycastthit.collider}",
                    $"raycastthit.distance: {raycastthit.distance}",
                    $"raycastthit.GetType(): {raycastthit.GetType()}",
                    $"raycastthit.lightmapCoord: {raycastthit.lightmapCoord}",
                    $"raycastthit.normal: {raycastthit.normal}",
                    $"raycastthit.point: {raycastthit.point}",
                    $"raycastthit.rigidbody: {raycastthit.rigidbody}",
                    $"raycastthit.textureCoord: {raycastthit.textureCoord}",
                    $"raycastthit.textureCoord2: {raycastthit.textureCoord2}",
                    $"raycastthit: {raycastthit}",
                    $"raycastthit.transform: {raycastthit.transform}",
                    $"raycastthit.triangleIndex: {raycastthit.triangleIndex}",
                    $"raycastthit.transform.gameObject.name: {raycastthit.transform.gameObject.name}",
                    $"raycastthit.transform.gameObject.gameObject.GetComponent<Player>()?.Nickname: {raycastthit.transform.gameObject.GetComponent<Player>()?.Nickname}",
                    $"Player.Get(raycastthit.transform.gameObject)?.Nickname: {Player.Get(raycastthit.transform.gameObject)?.Nickname}",
                    $"raycastthit.transform.gameObject.tag: {raycastthit.transform.gameObject.tag}",
                    $"raycastthit.transform.gameObject.GetType(): {raycastthit.transform.gameObject.GetType()}");
#endif

                return false;
            }
#if Cuff
            else
            {
                var target = Player.Get(raycastthit.transform.gameObject);
                if (!target.IsAlive)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ МЕРТВА</color>";
                    return false;
                }
                else if (target.Team == Team.SCP)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ НЕВОЗМОЖНО ОБЕЗВРЕДИТЬ</color>";
                    return false;
                }
                else if (target.CurrentItem.Base.Category == ItemCategory.Firearm || target.CurrentItem.Base.Category == ItemCategory.Grenade || target.CurrentItem.Base.Category == ItemCategory.MicroHID)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ ВООРУЖЕНА</color>";
                    return false;
                }
                else if (playerRequester.CurrentItem.Base.Category == ItemCategory.Firearm || playerRequester.CurrentItem.Base.Category == ItemCategory.MicroHID)
                {
                    response = "\n<color=#C1B5B5>СТАТУС: </color><color=#990000>ОШИБКА</color>\n<color=#C1B5B5>ВЫВОД: ВЫ НЕ ВООРУЖЕНЫ</color>";
                    return false;
                }

                target.Cuffer = playerRequester;

                response = "\n<color=#C1B5B5>СТАТУС: </color><color=#6aa84f>УСПЕШНО</color>\n<color=#C1B5B5>ВЫВОД: ЦЕЛЬ ОБЕЗВРЕЖЕНА</color>";
                return true;
            }
#endif
        }
    }
}
