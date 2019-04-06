using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace CIRCe.Base
{
    /// <summary>
    /// Информация о дополнении
    /// </summary>
    [Serializable]
    public sealed class AddonInformation
    {
        /// <summary>
        /// Является ли данное дополнение сборкой .NET
        /// </summary>
        public bool IsAssembly { get; set; }
        /// <summary>
        /// Путь к библиотеке дополнения
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Уникальный идентификатор дополнения
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// Информация об дополнении
        /// </summary>
        public AddonInfoAttribute Info { get; set; }
        /// <summary>
        /// Локализованная информация о дополнении
        /// </summary>
        public AddonLocalizationInfoAttribute[] LocalizedInfos { get; set; }

        public AddonInformation()
        {
            
        }

        public override string ToString()
        {
            return this.Path;
        }
    }
}
