using System;
using System.Collections.Generic;
using System.Text;
using CIRCe.Base;

// заполните информацию о своём дополнении
[assembly: AddonInfo(StartMode = AddonStartMode.Manual, VisibleInMenu = true, AddonType = "CIRCeAddonTemplate.Addon")]
[assembly: AddonLocalizationInfo(Title = "Название дополнения", Description = "Описание дополнения", Author = "Автор")]

namespace CIRCeAddonTemplate
{
    /// <summary>
    /// Дополнение для Цирцеи
    /// </summary>
    public partial class MyAddon
    {
        /// <summary>
        /// Здесь можно создать новые объекты и пункты меню, добавить обработчики событий у окон Цирцеи
        /// </summary>
        private void Init()
        {
            
        }

        /// <summary>
        /// Здесь можно удалить все созданные дополнением пункты меню и освободить ресурсы
        /// </summary>
        private void Finish()
        {
            
        }

        /// <summary>
        /// Нужно ли дополнению обновление
        /// </summary>
        /// <returns>Да, если нужно, и нет в противном случае</returns>
        public override bool IsUpdateNeeded()
        {
            return false;
        }

        /// <summary>
        /// Получить URI файла, который нужно скачать для обновления дополнения
        /// </summary>
        /// <returns>URI файла с обновлением (архив или SFX-архив)</returns>
        public override string GetUpdateUri()
        {
            return "http://mysite.ru/update.exe";
        }

        /// <summary>
        /// Здесь нужно выполнить действия, завершающие работу дополнения
        /// </summary>
        public override void Stop()
        {

        }
    }
}