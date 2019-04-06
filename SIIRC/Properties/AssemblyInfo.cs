using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CIRCe.Base;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SIIRC")]
[assembly: AssemblyDescription("Компонент для ведения СИ")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Свояк-софт")]
[assembly: AssemblyProduct("SIIRC")]
[assembly: AssemblyCopyright("Copyright © Свояк-софт 2009-2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("6847ef93-40bb-4177-8717-4c370679cbac")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.6.4.0")]
[assembly: AssemblyFileVersion("1.6.4.0")]

[assembly: AddonInfo(StartMode = AddonStartMode.Manual, VisibleInMenu = true, AddonType = "SIIRC.SIAddon")]
[assembly: AddonLocalizationInfo(Title = "SIGame в IRC", Description = "Компонент для проведения SIGame в IRC", Author = "Vladimir Khil")]
[assembly: AddonLocalizationInfo(Culture = "en-US", Title = "Jeopardy in IRC", Description = "Addon for moderating SIGame", Author = "Vladimir Khil")]