﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRCWindow.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// Расположение обновлений
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Расположение обновлений")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://vladimirkhil.com/circe/")]
        public string UpdatePath {
            get {
                return ((string)(this["UpdatePath"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2011-01-06")]
        public global::System.DateTime AppVersion {
            get {
                return ((global::System.DateTime)(this["AppVersion"]));
            }
            set {
                this["AppVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int DefUserIndex {
            get {
                return ((int)(this["DefUserIndex"]));
            }
            set {
                this["DefUserIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int ObsoleteTime {
            get {
                return ((int)(this["ObsoleteTime"]));
            }
            set {
                this["ObsoleteTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("About me")]
        public string UserInfoString {
            get {
                return ((string)(this["UserInfoString"]));
            }
            set {
                this["UserInfoString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Checked")]
        public global::System.Windows.Forms.CheckState SearchForUpdates {
            get {
                return ((global::System.Windows.Forms.CheckState)(this["SearchForUpdates"]));
            }
            set {
                this["SearchForUpdates"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("(?<nick>[^ =*]*)(?<op>\\*?)\\=(?<plus>[+-])\\~(?<host>[^ ]*)")]
        public string RplUserHostRegexPattern {
            get {
                return ((string)(this["RplUserHostRegexPattern"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Checked")]
        public global::System.Windows.Forms.CheckState SearchForAddons {
            get {
                return ((global::System.Windows.Forms.CheckState)(this["SearchForAddons"]));
            }
            set {
                this["SearchForAddons"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2011-01-06")]
        public global::System.DateTime DataVersion {
            get {
                return ((global::System.DateTime)(this["DataVersion"]));
            }
            set {
                this["DataVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UseAppDataFolder {
            get {
                return ((bool)(this["UseAppDataFolder"]));
            }
            set {
                this["UseAppDataFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DataFolder {
            get {
                return ((string)(this["DataFolder"]));
            }
            set {
                this["DataFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FirstRun {
            get {
                return ((bool)(this["FirstRun"]));
            }
            set {
                this["FirstRun"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ServersVisibility {
            get {
                return ((bool)(this["ServersVisibility"]));
            }
            set {
                this["ServersVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool NickListVisibility {
            get {
                return ((bool)(this["NickListVisibility"]));
            }
            set {
                this["NickListVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ChannelsVisibility {
            get {
                return ((bool)(this["ChannelsVisibility"]));
            }
            set {
                this["ChannelsVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool NicksVisibility {
            get {
                return ((bool)(this["NicksVisibility"]));
            }
            set {
                this["NicksVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PersonVisibility {
            get {
                return ((bool)(this["PersonVisibility"]));
            }
            set {
                this["PersonVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SettingsVisibility {
            get {
                return ((bool)(this["SettingsVisibility"]));
            }
            set {
                this["SettingsVisibility"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Uri Proxy {
            get {
                return ((global::System.Uri)(this["Proxy"]));
            }
            set {
                this["Proxy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool UseProxy {
            get {
                return ((bool)(this["UseProxy"]));
            }
            set {
                this["UseProxy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Net.NetworkCredential ProxyCredentials {
            get {
                return ((global::System.Net.NetworkCredential)(this["ProxyCredentials"]));
            }
            set {
                this["ProxyCredentials"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IsFirstRun {
            get {
                return ((bool)(this["IsFirstRun"]));
            }
            set {
                this["IsFirstRun"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::IRCProviders.KeysStringDictionary HotKeys {
            get {
                return ((global::IRCProviders.KeysStringDictionary)(this["HotKeys"]));
            }
            set {
                this["HotKeys"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("&lt;?xml version=\"1.0\" encoding=\"utf-16\"?&gt;&lt;Colors&gt;&lt;item&gt;-1&lt;/ite" +
            "m&gt;&lt;item&gt;-16777216&lt;/item&gt;&lt;item&gt;-16777077&lt;/item&gt;&lt;ite" +
            "m&gt;-16751616&lt;/item&gt;&lt;item&gt;-65536&lt;/item&gt;&lt;item&gt;-5952982&l" +
            "t;/item&gt;&lt;item&gt;-8388480&lt;/item&gt;&lt;item&gt;-23296&lt;/item&gt;&lt;i" +
            "tem&gt;-256&lt;/item&gt;&lt;item&gt;-16711808&lt;/item&gt;&lt;item&gt;-14634326&" +
            "lt;/item&gt;&lt;item&gt;-7876870&lt;/item&gt;&lt;item&gt;-16776961&lt;/item&gt;&" +
            "lt;item&gt;-65281&lt;/item&gt;&lt;item&gt;-11184811&lt;/item&gt;&lt;item&gt;-230" +
            "2756&lt;/item&gt;&lt;item&gt;-2180985&lt;/item&gt;&lt;item&gt;-7722014&lt;/item&" +
            "gt;&lt;item&gt;-5171&lt;/item&gt;&lt;item&gt;-657956&lt;/item&gt;&lt;item&gt;-55" +
            "73131&lt;/item&gt;&lt;item&gt;-9177611&lt;/item&gt;&lt;item&gt;-12782091&lt;/ite" +
            "m&gt;&lt;item&gt;-16404561&lt;/item&gt;&lt;item&gt;-16422551&lt;/item&gt;&lt;ite" +
            "m&gt;-8323104&lt;/item&gt;&lt;item&gt;-13382503&lt;/item&gt;&lt;item&gt;-1424756" +
            "5&lt;/item&gt;&lt;item&gt;-15112628&lt;/item&gt;&lt;item&gt;-15977690&lt;/item&g" +
            "t;&lt;item&gt;-677131&lt;/item&gt;&lt;item&gt;-691211&lt;/item&gt;&lt;item&gt;-7" +
            "05291&lt;/item&gt;&lt;item&gt;-5306961&lt;/item&gt;&lt;item&gt;-9894551&lt;/item" +
            "&gt;&lt;item&gt;-2064129&lt;/item&gt;&lt;item&gt;-6736948&lt;/item&gt;&lt;item&g" +
            "t;-9230695&lt;/item&gt;&lt;item&gt;-11789978&lt;/item&gt;&lt;item&gt;-14283725&l" +
            "t;/item&gt;&lt;item&gt;-658006&lt;/item&gt;&lt;item&gt;-658061&lt;/item&gt;&lt;i" +
            "tem&gt;-658116&lt;/item&gt;&lt;item&gt;-5263611&lt;/item&gt;&lt;item&gt;-9869051" +
            "&lt;/item&gt;&lt;item&gt;-8064&lt;/item&gt;&lt;item&gt;-3368653&lt;/item&gt;&lt;" +
            "item&gt;-6720730&lt;/item&gt;&lt;item&gt;-10073063&lt;/item&gt;&lt;item&gt;-1342" +
            "5140&lt;/item&gt;&lt;item&gt;-5592331&lt;/item&gt;&lt;item&gt;-9210891&lt;/item&" +
            "gt;&lt;item&gt;-12829451&lt;/item&gt;&lt;item&gt;-16448081&lt;/item&gt;&lt;item&" +
            "gt;-16448151&lt;/item&gt;&lt;item&gt;-8331009&lt;/item&gt;&lt;item&gt;-13395508&" +
            "lt;/item&gt;&lt;item&gt;-14257255&lt;/item&gt;&lt;item&gt;-15119258&lt;/item&gt;" +
            "&lt;item&gt;-15981005&lt;/item&gt;&lt;item&gt;-5573206&lt;/item&gt;&lt;item&gt;-" +
            "9177741&lt;/item&gt;&lt;item&gt;-12782276&lt;/item&gt;&lt;item&gt;-16404731&lt;/" +
            "item&gt;&lt;item&gt;-16422651&lt;/item&gt;&lt;item&gt;-2031744&lt;/item&gt;&lt;i" +
            "tem&gt;-6697933&lt;/item&gt;&lt;item&gt;-9201370&lt;/item&gt;&lt;item&gt;-117703" +
            "43&lt;/item&gt;&lt;item&gt;-14273780&lt;/item&gt;&lt;item&gt;-677206&lt;/item&gt" +
            ";&lt;item&gt;-691341&lt;/item&gt;&lt;item&gt;-705476&lt;/item&gt;&lt;item&gt;-53" +
            "07131&lt;/item&gt;&lt;item&gt;-9894651&lt;/item&gt;&lt;item&gt;-32544&lt;/item&g" +
            "t;&lt;item&gt;-3394663&lt;/item&gt;&lt;item&gt;-6740365&lt;/item&gt;&lt;item&gt;" +
            "-10086068&lt;/item&gt;&lt;item&gt;-13431770&lt;/item&gt;&lt;item&gt;-986896&lt;/" +
            "item&gt;&lt;item&gt;-4276546&lt;/item&gt;&lt;item&gt;-7566196&lt;/item&gt;&lt;it" +
            "em&gt;-10855846&lt;/item&gt;&lt;item&gt;-14145496&lt;/item&gt;&lt;item&gt;-20316" +
            "48&lt;/item&gt;&lt;item&gt;-6697831&lt;/item&gt;&lt;item&gt;-9201293&lt;/item&gt" +
            ";&lt;item&gt;-11770292&lt;/item&gt;&lt;item&gt;-14273754&lt;/item&gt;&lt;item&gt" +
            ";-4343957&lt;/item&gt;&lt;item&gt;-7667712&lt;/item&gt;&lt;item&gt;-16741493&lt;" +
            "/item&gt;&lt;item&gt;-9728477&lt;/item&gt;&lt;item&gt;-16724271&lt;/item&gt;&lt;" +
            "item&gt;-7077677&lt;/item&gt;&lt;item&gt;-13676721&lt;/item&gt;&lt;item&gt;-8355" +
            "840&lt;/item&gt;&lt;item&gt;-6632142&lt;/item&gt;&lt;/Colors&gt;")]
        public global::IRCProviders.Colors Colors {
            get {
                return ((global::IRCProviders.Colors)(this["Colors"]));
            }
            set {
                this["Colors"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"&lt;?xml version=""1.0"" encoding=""utf-16""?&gt;&lt;table&gt;&lt;item&gt;&lt;key&gt;o&lt;/key&gt;&lt;value&gt;@&lt;/value&gt;&lt;/item&gt;&lt;item&gt;&lt;key&gt;h&lt;/key&gt;&lt;value&gt;%&lt;/value&gt;&lt;/item&gt;&lt;item&gt;&lt;key&gt;v&lt;/key&gt;&lt;value&gt;+&lt;/value&gt;&lt;/item&gt;&lt;item&gt;&lt;key&gt;a&lt;/key&gt;&lt;value&gt;&amp;amp;&lt;/value&gt;&lt;/item&gt;&lt;item&gt;&lt;key&gt;b&lt;/key&gt;&lt;value&gt;~&lt;/value&gt;&lt;/item&gt;&lt;/table&gt;")]
        public global::IRCProviders.CharCharDictionary ModesTable {
            get {
                return ((global::IRCProviders.CharCharDictionary)(this["ModesTable"]));
            }
            set {
                this["ModesTable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfAddonInformation xmlns:xsi=\"http" +
            "://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSche" +
            "ma\" />")]
        public global::CIRCe.Base.AddonInformationList AddonsNew {
            get {
                return ((global::CIRCe.Base.AddonInformationList)(this["AddonsNew"]));
            }
            set {
                this["AddonsNew"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfChannel xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#chgk</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#luz</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#mafbot</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#saveicq</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#sns</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#svoya_igra</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#svoyak</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#vdi</Name>
  </Channel>
  <Channel>
    <Sticked>false</Sticked>
    <AutoOpen>false</AutoOpen>
    <Name>#vdi-red</Name>
  </Channel>
</ArrayOfChannel>")]
        public global::IRCConnection.ChannelList Channels {
            get {
                return ((global::IRCConnection.ChannelList)(this["Channels"]));
            }
            set {
                this["Channels"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfNickName xmlns:xsi=\"http://www.w" +
            "3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />")]
        public global::IRCConnection.NickList Nicks {
            get {
                return ((global::IRCConnection.NickList)(this["Nicks"]));
            }
            set {
                this["Nicks"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<User xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <UserName>User</UserName>
  <Host>Host</Host>
  <Name>Real name</Name>
  <EMail>me@host.ru</EMail>
  <Info />
</User>")]
        public global::IRCConnection.User User {
            get {
                return ((global::IRCConnection.User)(this["User"]));
            }
            set {
                this["User"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfServer xmlns:xsi=\"http://www.w3." +
            "org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <S" +
            "erver>\r\n    <Sticked>false</Sticked>\r\n    <AutoOpen>false</AutoOpen>\r\n    <Descr" +
            "iption>IzhIrc</Description>\r\n    <Name>irc.izhirc.ru</Name>\r\n    <Port>6667</Por" +
            "t>\r\n    <Channels>\r\n      <Channel>\r\n        <Sticked>false</Sticked>\r\n        <" +
            "AutoOpen>false</AutoOpen>\r\n        <Name>#sns</Name>\r\n      </Channel>\r\n      <C" +
            "hannel>\r\n        <Sticked>false</Sticked>\r\n        <AutoOpen>false</AutoOpen>\r\n " +
            "       <Name>#svoya_igra</Name>\r\n      </Channel>\r\n      <Channel>\r\n        <Sti" +
            "cked>false</Sticked>\r\n        <AutoOpen>false</AutoOpen>\r\n        <Name>#svoyak<" +
            "/Name>\r\n      </Channel>\r\n    </Channels>\r\n    <Passwords />\r\n  </Server>\r\n  <Se" +
            "rver>\r\n    <Sticked>false</Sticked>\r\n    <AutoOpen>false</AutoOpen>\r\n    <Descri" +
            "ption>ForestNet</Description>\r\n    <Name>irc.forestnet.org</Name>\r\n    <Port>666" +
            "7</Port>\r\n    <Channels>\r\n      <Channel>\r\n        <Sticked>false</Sticked>\r\n   " +
            "     <AutoOpen>false</AutoOpen>\r\n        <Name>#saveicq</Name>\r\n      </Channel>" +
            "\r\n    </Channels>\r\n    <Passwords />\r\n  </Server>\r\n  <Server>\r\n    <Sticked>fals" +
            "e</Sticked>\r\n    <AutoOpen>false</AutoOpen>\r\n    <Description>2777</Description>" +
            "\r\n    <Name>irc.2777.ru</Name>\r\n    <Port>6667</Port>\r\n    <Channels>\r\n      <Ch" +
            "annel>\r\n        <Sticked>false</Sticked>\r\n        <AutoOpen>false</AutoOpen>\r\n  " +
            "      <Name>#mafbot</Name>\r\n      </Channel>\r\n    </Channels>\r\n    <Passwords />" +
            "\r\n  </Server>\r\n  <Server>\r\n    <Sticked>false</Sticked>\r\n    <AutoOpen>false</Au" +
            "toOpen>\r\n    <Description>ChgkInfo</Description>\r\n    <Name>irc.chgk.info</Name>" +
            "\r\n    <Port>6667</Port>\r\n    <Channels>\r\n      <Channel>\r\n        <Sticked>false" +
            "</Sticked>\r\n        <AutoOpen>false</AutoOpen>\r\n        <Name>#vdi</Name>\r\n     " +
            " </Channel>\r\n      <Channel>\r\n        <Sticked>false</Sticked>\r\n        <AutoOpe" +
            "n>false</AutoOpen>\r\n        <Name>#vdi-red</Name>\r\n      </Channel>\r\n    </Chann" +
            "els>\r\n    <Passwords />\r\n  </Server>\r\n  <Server>\r\n    <Sticked>false</Sticked>\r\n" +
            "    <AutoOpen>false</AutoOpen>\r\n    <Description>MGTS</Description>\r\n    <Name>i" +
            "rc.mgts.by</Name>\r\n    <Port>6667</Port>\r\n    <Channels />\r\n    <Passwords />\r\n " +
            " </Server>\r\n  <Server>\r\n    <Sticked>false</Sticked>\r\n    <AutoOpen>false</AutoO" +
            "pen>\r\n    <Description>QuakeNet</Description>\r\n    <Name>irc.quakenet.org</Name>" +
            "\r\n    <Port>6667</Port>\r\n    <Channels />\r\n    <Passwords />\r\n  </Server>\r\n</Arr" +
            "ayOfServer>")]
        public global::IRCConnection.ServerList Servers {
            get {
                return ((global::IRCConnection.ServerList)(this["Servers"]));
            }
            set {
                this["Servers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfWindowInfo xmlns:xsi=\"http://www" +
            ".w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />" +
            "")]
        public global::IRCProviders.WindowInfoList Windows {
            get {
                return ((global::IRCProviders.WindowInfoList)(this["Windows"]));
            }
            set {
                this["Windows"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfLogProvider xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <LogProvider>
    <Title>Txt</Title>
  </LogProvider>
  <LogProvider>
    <Title>Html</Title>
    <Header>&lt;html&gt;&lt;head&gt;&lt;title&gt;[[WindowName]]&lt;/title&gt;&lt;/head&gt;&lt;body&gt;</Header>
    <OpenB>&lt;b&gt;</OpenB>
    <CloseB>&lt;/b&gt;</CloseB>
    <OpenU>&lt;u&gt;</OpenU>
    <CloseU>&lt;/u&gt;</CloseU>
    <OpenK>&lt;span style=""color:[[Color]]""&gt;</OpenK>
    <CloseK>&lt;/span&gt;</CloseK>
    <OpenL>&lt;span style=""background-color:[[Color]]""&gt;</OpenL>
    <CloseL>&lt;/span&gt;</CloseL>
    <Bottom>&lt;/body&gt;&lt;/html&gt;</Bottom>
  </LogProvider>
  <LogProvider>
    <Title>Rtf</Title>
    <Header>{\rtf1\ansi\ansicpg1251\deff0\deflang1049{\fonttbl{\f0\fnil\fcharset204{\*\fname Courier New;}Courier New CYR;}}[[RTFCT]]
                \viewkind4\uc1\pard\f0\fs20
</Header>
    <OpenB>\b</OpenB>
    <CloseB>\b0</CloseB>
    <OpenU>\ul</OpenU>
    <CloseU>\ul0</CloseU>
    <OpenK>\cf[[Color]]</OpenK>
    <OpenL>\chshdng0\chcbpat[[Color]]</OpenL>
    <Bottom>\r\n}\r\n</Bottom>
  </LogProvider>
</ArrayOfLogProvider>")]
        public global::IRCProviders.LogProvidersList LogProviders {
            get {
                return ((global::IRCProviders.LogProvidersList)(this["LogProviders"]));
            }
            set {
                this["LogProviders"] = value;
            }
        }
    }
}
