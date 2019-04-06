using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CIRCe.Base;
using System.Reflection;
using System.IO;

namespace AddonsChecker
{
    public sealed class AddonsChecker : MarshalByRefObject
    {
        public bool CheckAssembly(string assemblyPath, out Guid guid, out AddonInfoAttribute info, out AddonLocalizationInfoAttribute[] localizationInfos)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                // Новая логика загрузки информации об аддонах - через атрибуты сборки
                var assembly = Assembly.LoadFrom(assemblyPath);
                var guidAttr = (GuidAttribute)Attribute.GetCustomAttribute(assembly, typeof(GuidAttribute));
                if (guidAttr == null || guidAttr.Value == null)
                    guid = Guid.Empty;
                else
                    Guid.TryParse(guidAttr.Value, out guid);

                info = (AddonInfoAttribute)Attribute.GetCustomAttribute(assembly, typeof(AddonInfoAttribute));
                var localizations = Attribute.GetCustomAttributes(assembly, typeof(AddonLocalizationInfoAttribute));
                localizationInfos = localizations != null ? Array.ConvertAll(localizations, attr => (AddonLocalizationInfoAttribute)attr) : null;

                return true;
            }
            catch (BadImageFormatException)
            {
                guid = Guid.Empty;
                info = null;
                localizationInfos = null;
                return false;
            }
            catch (Exception)
            {
                guid = Guid.Empty;
                info = null;
                localizationInfos = null;
                return false;
            }
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(fileName))
                    return Assembly.LoadFrom(fileName);

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
