using System;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Windows.Resources;
using System.ComponentModel.Composition.Primitives;

namespace MefMvvm.ViewModelLocator
{
    /// <summary>
    /// Default composer for Design time. This will load all assemblies that have the DesignTimeCatalog attibute
    /// </summary>
    public class DefaultDesignTimeComposer : IComposer
    {
        #region IComposer Members

        public ComposablePartCatalog InitializeContainer()
        {
            return GetCatalog();
        }

        #endregion 

        private static AggregateCatalog GetCatalog()
        {
            IList<AssemblyCatalog> assembliesLoadedCatalogs = (from assembly in
                                                                   (IEnumerable<Assembly>)typeof(AppDomain)
                                                                  .GetMethod("GetAssemblies")
                                                                  .Invoke(AppDomain.CurrentDomain, null)
                                                               where ((IEnumerable<AssemblyName>) typeof(Assembly)
                                                                  .GetMethod("GetReferencedAssemblies").Invoke(assembly, null)).Where(x => x.Name.Contains("MefMvvm.SL")).Count() > 0 ||
                                                                  assembly.ManifestModule.Name == "MefMvvm.SL.dll"
                                                               select new AssemblyCatalog(assembly)).ToList();

            if (assembliesLoadedCatalogs.Where(x => x.Assembly.ManifestModule.Name != "MefMvvm.SL.dll").Count() == 0)
            {
                Debug.WriteLine("No assemblies found for Design time. ");
                return null;
            }

            foreach (var item in assembliesLoadedCatalogs)
            {
                Debug.WriteLine(item.Assembly.FullName + " is being added for design time composition");
            }

            var catalog = new AggregateCatalog();

            foreach (var item in assembliesLoadedCatalogs)
                catalog.Catalogs.Add(item);
            return catalog;
        }
    }

    /// <summary>
    /// Implemenation for the default runtime composer. This composer doesn't do anything since it relies on the CompositionInializer.SatisyImports default implemenation
    /// </summary>
    public class DefaultRuntimeComposer : IComposer
    {
        #region IComposer Members

        public ComposablePartCatalog InitializeContainer()
        {
            return new AggregateCatalog((from assembly in GetAssemblyList() select new AssemblyCatalog(assembly)).Cast<ComposablePartCatalog>());
        }

        #endregion

        private static List<Assembly> GetAssemblyList()
        {
            List<Assembly> list = new List<Assembly>();
            foreach (AssemblyPart part in Deployment.Current.Parts)
            {
                StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(part.Source, UriKind.Relative));
                if (resourceStream != null)
                {
                    Assembly item = part.Load(resourceStream.Stream);
                    list.Add(item);
                }
            }
            return list;
        }

    }

}
