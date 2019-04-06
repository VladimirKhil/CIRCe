using System;
using System.Collections.Generic;
using System.Text;
using IRCProviders;
using IRCConnection;
using System.Windows.Forms;

namespace CIRCeAddonTemplate
{
    /// <summary>
    /// Аддон для Цирцеи
    /// </summary>
    public partial class Addon
    {
        /// <summary>
        /// Здесь можно создать новые объекты и пункты меню, добавить обработчики событий у окон Цирцеи
        /// </summary>
        private void Init()
        {
            
        }

        /// <summary>
        /// Здесь можно удалить все созданные аддоном пункты меню и освободить ресурсы
        /// </summary>
        private void Finish()
        {
            
        }

        /// <summary>
        /// Нужно ли аддону обновление
        /// </summary>
        /// <returns>Да, если нужно, и нет в противном случае</returns>
        public override bool IsUpdateNeeded()
        {
            return false;
        }

        /// <summary>
        /// Получить URI файла, который нужно скачать для обновления аддона
        /// </summary>
        /// <returns>URI файла с обновлением</returns>
        public override string GetUpdateUri()
        {
            return "http://mysite.ru/update.exe";
        }
    }
}