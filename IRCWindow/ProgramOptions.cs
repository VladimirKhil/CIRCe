using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using IRCWindow.Properties;
using IRCProviders;
using System.IO;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Net;

namespace IRCWindow
{
    /// <summary>
    /// Настройки программы, которые можно редактировать
    /// </summary>
    [TypeConverter(typeof(ProgramOptionsConverter))]
    public sealed class ProgramOptions
    {
        [LocalizedCategory(LocalizedCategories.LCSpecial)]
        [LocalizedDisplayName("Proxy")]
        [Editor(typeof(ProxyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ProxyTypeConverter))]
        public Uri Proxy
        {
            get { return Settings.Default.Proxy; }
            set { Settings.Default.Proxy = value; }
        }

        public bool ShouldSerializeProxy()
        {
            return Settings.Default.UseProxy;
        }

        [LocalizedCategory(LocalizedCategories.LCSpecial)]
        [LocalizedDisplayName("WaitServer")]
        [LocalizedDescription("WaitServerDescription")]
        [DefaultValue(300)]
        public int WaitServer
        {
            get { return (int)UISettings.Default.WaitServer; }
            set { UISettings.Default.WaitServer = (uint)value; }
        }

        [LocalizedCategory(LocalizedCategories.LCSpecial)]
        [LocalizedDisplayName("PingServer")]
        [LocalizedDescription("PingServerDescription")]
        [DefaultValue(150)]
        public int PingServer
        {
            get { return (int)UISettings.Default.PingServer; }
            set { UISettings.Default.PingServer = (uint)value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("QuitMessage")]
        [DefaultValue("CIRCe by Vladimir Khil")]
        //[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string QuitMessage
        {
            get { return UISettings.Default.QuitMessage; }
            set { UISettings.Default.QuitMessage = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("IRCWindowSize")]
        public Size IRCWindowSize
        {
            get { return UISettings.Default.IRCWindowSize; }
            set { UISettings.Default.IRCWindowSize = value; }
        }

        public bool ShouldSerializeIRCWindowSize()
        {
            return this.IRCWindowSize.Width != 819 || this.IRCWindowSize.Height != 509;
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("LogMode")]
        [LocalizedDescription("LogModeDescription")]
        [DefaultValue(LogMode.Txt)]
        [TypeConverter(typeof(EnumTypeConverter))]
        public LogMode LogMode
        {
            get { return UISettings.Default.LogMode; }
            set { UISettings.Default.LogMode = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCSpecial)]
        [LocalizedDisplayName("FlashingParams")]
        [Editor(typeof(FlashEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ConfigTypeConverter))]
        public FlashParams FlashingParams
        {
            get { return UISettings.Default.FlashingParams; }
            set { UISettings.Default.FlashingParams = value; }
        }

        public bool ShouldSerializeFlashingParams()
        {
            return false;
        }
        
        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("PlayMusic")]
        [LocalizedDescription("PlayMusicDescription")]
        [DefaultValue(PlayMode.All)]
        [TypeConverter(typeof(EnumTypeConverter))]
        public PlayMode PlayMusic
        {
            get { return UISettings.Default.PlayMusicExt; }
            set { UISettings.Default.PlayMusicExt = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("OpenPrivateVisible")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool OpenPrivateVisible
        {
            get { return UISettings.Default.OpenPrivateVisible; }
            set { UISettings.Default.OpenPrivateVisible = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("Извещать о приходящих CTCP-запросах")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool NotifyOnCtcp
        {
            get { return UISettings.Default.NotifyOnCtcp; }
            set { UISettings.Default.NotifyOnCtcp = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("Извещать об отсутствии мультимедиа-файлов для проигрывания")]
        [DefaultValue(false)]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool NotifyOnNoMedia
        {
            get { return UISettings.Default.NotifyOnNoMedia; }
            set { UISettings.Default.NotifyOnNoMedia = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("SavePasswords")]
        [DefaultValue(true)]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool SavePasswords
        {
            get { return UISettings.Default.SavePasswords; }
            set { UISettings.Default.SavePasswords = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("ShowUrlOnCmd")]
        [LocalizedDescription("ShowUrlOnCmdDescription")]
        [DefaultValue(PlayMode.All)]
        [TypeConverter(typeof(EnumTypeConverter))]
        public PlayMode ShowUrlOnCmd
        {
            get { return UISettings.Default.UrlExt; }
            set { UISettings.Default.UrlExt = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("ShowReadSplitters")]
        [LocalizedDescription("ShowReadSplittersDescription")]
        [DefaultValue(true)]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool ShowReadSplitters
        {
            get { return UISettings.Default.ShowReadSplitters; }
            set { UISettings.Default.ShowReadSplitters = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("PutRegisterMessage")]
        [LocalizedDescription("PutRegisterMessageDescription")]
        [DefaultValue(true)]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public bool PutRegisterMessage
        {
            get { return UISettings.Default.PutRegisterMessage; }
            set { UISettings.Default.PutRegisterMessage = value; }
        }

        public enum Languages
        {
            [Description("LanguageRu")]
            Russian,
            [Description("LanguageEn")]
            English
        }

        [LocalizedCategory(LocalizedCategories.LCCommon)]
        [LocalizedDisplayName("AppLanguage")]
        [LocalizedDescription("AppLanguageDescription")]
        [TypeConverter(typeof(EnumTypeConverter))]
        [DefaultValue(Languages.Russian)]
        public Languages Language
        {
            get { return UISettings.Default.Language == CultureInfo.GetCultureInfo("en-US") ? Languages.English : Languages.Russian; }
            set { UISettings.Default.Language = value != Languages.Russian ? CultureInfo.GetCultureInfo("en-US") : CultureInfo.GetCultureInfo("ru-RU"); }
        }

        [LocalizedCategory(LocalizedCategories.LCSpecial)]
        [LocalizedDisplayName("AppDataFolder")]
        [LocalizedDescription("AppDataFolderDescription")]
        [Editor(typeof(AppDataEditor), typeof(UITypeEditor))]
        public string AppDataFolder
        {
            get 
            { 
                return Settings.Default.UseAppDataFolder ?
                Program.DefaultDataFolder()
                : Settings.Default.DataFolder; 
            }
        }

        public bool ShouldSerializeAppDataFolder()
        {
            return AppDataFolder != Program.DefaultDataFolder();
        }

        [LocalizedCategory(LocalizedCategories.LCTopic)]
        [LocalizedDisplayName("Font")]
        public Font TopicFont
        {
            get { return UISettings.Default.TopicFont; }
            set 
            { 
                UISettings.Default.TopicFont = value; 
            }
        }

        public bool ShouldSerializeTopicFont()
        {
            return !this.TopicFont.Equals(new FontConverter().ConvertFrom("Arial; 12,75pt"));
        }

        [LocalizedCategory(LocalizedCategories.LCTopic)]
        [LocalizedDisplayName("Background")]
        [DefaultValue(typeof(Color), "Lavender")]
        public Color TopicBackColor
        {
            get { return UISettings.Default.TopicBackColor; }
            set
            {
                UISettings.Default.TopicBackColor = value;
            }
        }

        [LocalizedCategory(LocalizedCategories.LCChat)]
        [LocalizedDisplayName("Font")]
        [LocalizedDescription("ChatFontDescription")]
        public Font ChatFont
        {
            get { return UISettings.Default.ChatFont; }
            set
            {
                UISettings.Default.ChatFont = value;
            }
        }

        public bool ShouldSerializeChatFont()
        {
            return !this.ChatFont.Equals(new FontConverter().ConvertFrom("Courier New; 11,25pt"));
        }

        [LocalizedCategory(LocalizedCategories.LCChat)]
        [LocalizedDisplayName("DateTimeFormat")]
        [LocalizedDescription("DateTimeFormatDescription")]
        [DefaultValue("[HH:mm:ss]")]
        public string DateTimeFormat
        {
            get { return UISettings.Default.DateTimeFormat; }
            set
            {
                UISettings.Default.DateTimeFormat = value;
            }
        }

        [LocalizedCategory(LocalizedCategories.LCChat)]
        [LocalizedDisplayName("Background")]
        [LocalizedDescription("ChatFontDescription")]
        [DefaultValue(typeof(Color), "GhostWhite")]
        public Color ChatBackColor
        {
            get { return UISettings.Default.ChatBackColor; }
            set
            {
                UISettings.Default.ChatBackColor = value;
            }
        }

        [LocalizedCategory(LocalizedCategories.LCInput)]
        [LocalizedDisplayName("Font")]
        public Font PrintFont
        {
            get { return UISettings.Default.PrintFont; }
            set
            {
                UISettings.Default.PrintFont = value;
            }
        }

        public bool ShouldSerializePrintFont()
        {
            return !this.PrintFont.Equals(new FontConverter().ConvertFrom("Arial; 12,75pt"));
        }

        [LocalizedCategory(LocalizedCategories.LCInput)]
        [LocalizedDisplayName("PrintForeColor")]
        [DefaultValue(1)]
        [Editor(typeof(IRCColorEditor), typeof(UITypeEditor))]
        public int PrintForeColor
        {
            get { return UISettings.Default.MessagesColor; }
            set
            {
                if (value < 0 || value > 98)
                    return;
                UISettings.Default.MessagesColor = value;
            }
        }

        [LocalizedCategory(LocalizedCategories.LCInput)]
        [LocalizedDisplayName("Background")]
        [DefaultValue(typeof(Color), "GhostWhite")]
        public Color PrintBackColor
        {
            get { return UISettings.Default.PrintBackColor; }
            set
            {
                UISettings.Default.PrintBackColor = value; 
            }
        }

        [LocalizedCategory(LocalizedCategories.LCUsersList)]
        [LocalizedDisplayName("Font")]
        public Font UsersFont
        {
            get { return UISettings.Default.UsersFont; }
            set
            {
                UISettings.Default.UsersFont = value;
            }
        }

        public bool ShouldSerializeUsersFont()
        {
            return !this.UsersFont.Equals(new FontConverter().ConvertFrom("Segoe UI; 11,25pt"));
        }

        [LocalizedCategory(LocalizedCategories.LCUsersList)]
        [LocalizedDisplayName("Background")]
        [DefaultValue(typeof(Color), "GhostWhite")]
        public Color UsersBackColor
        {
            get { return UISettings.Default.UsersBackColor; }
            set { UISettings.Default.UsersBackColor = value; }
        }

        [LocalizedCategory(LocalizedCategories.LCWindowsPanel)]
        [LocalizedDisplayName("Font")]
        public Font TreeFont
        {
            get { return UISettings.Default.TreeFont; }
            set
            {
                UISettings.Default.TreeFont = value;
            }
        }

        public bool ShouldSerializeTreeFont()
        {
            return !this.TreeFont.Equals(new FontConverter().ConvertFrom("Segoe UI; 9,75pt; style=Bold, Italic"));
        }

        [LocalizedCategory(LocalizedCategories.LCWindowsPanel)]
        [LocalizedDisplayName("Background")]
        [DefaultValue(typeof(Color), "Azure")]
        public Color TreeBackColor
        {
            get { return UISettings.Default.TreeBackColor; }
            set
            {
                UISettings.Default.TreeBackColor = value;
            }
        }
    }
}
