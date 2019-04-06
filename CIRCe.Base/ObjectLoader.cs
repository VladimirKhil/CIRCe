using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace CIRCe.Base
{
    /// <summary>
    /// Обеспечивает корректную загрузку дополнений в Цирцею
    /// </summary>
    public sealed class ObjectLoader: MarshalByRefObject
    {
        public ObjectLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, new AssemblyName(args.Name).Name + ".dll");
            if (File.Exists(fileName))
                return Assembly.LoadFrom(fileName);

            return null;
        }

        public object LoadObject(string assemblyName, string typeName)
        {
            var asm = Assembly.LoadFrom(assemblyName);
            return Activator.CreateInstance(asm.GetType(typeName));
            //return AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap(assemblyName, typeName);
        }
    }
}
