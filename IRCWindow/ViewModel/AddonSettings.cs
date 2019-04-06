using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;

namespace IRCWindow.ViewModel
{
    public sealed class AddonSettings : ViewModelBase, IEquatable<AddonSettings>
    {
        private AddonInformation info = null;

        public string Header { get { return AddonsManager.GetLocalizationInfoForAddon(info).Title; } }

        [Obsolete]
        public string Name
        {
            get { return AddonsManager.GetLocalizationInfoForAddon(info).Title; }
        }

        public bool Visibility
        {
            get { return info.Info.VisibleInMenu; }
            set { this.info.Info.VisibleInMenu = value; }
        }

        public AddonStartMode StartMode
        {
            get { return this.info.Info.StartMode; }
            set { this.info.Info.StartMode = value; }
        }

        public AddonInformation Info { get { return this.info; } }

        public AddonSettings()
        {
            this.info = new AddonInformation();
        }

        public AddonSettings(AddonInformation info)
        {
            this.info = info;
        }

        public override object Clone()
        {
            return new AddonSettings(new AddonInformation
            {
                Guid = this.info.Guid,
                IsAssembly = this.info.IsAssembly,
                LocalizedInfos = this.info.LocalizedInfos,
                Path = this.info.Path,
                Info = new AddonInfoAttribute
                {
                    AddonType = this.info.Info.AddonType,
                    StartMode = this.info.Info.StartMode,
                    VisibleInMenu = this.info.Info.VisibleInMenu
                }
            });
        }

        public override bool Equals(object obj)
        {
            var other = obj as AddonSettings;
            if (other == null)
                return false;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.info.Guid.GetHashCode();
        }

        public bool Equals(AddonSettings other)
        {
            return this.info.Guid == other.info.Guid;
        }
    }
}
