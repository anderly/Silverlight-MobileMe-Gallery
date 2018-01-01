using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MefMvvm.ViewModelLocator;
using MefMvvm.Common;
using System.ComponentModel.Composition.Hosting;

namespace MefMvvm.ViewModelLocator
{
    /// <summary>
    /// This is the bootstrapper that inializes the MEF catalogs
    /// </summary>
    public class LocatorBootstrapper
    {
        /// <summary>
        /// Checks if the bootstapper is inialized if now it inializes it
        /// </summary>
        public static CompositionContainer EnsureLocatorBootstrapper()
        {
            IComposer composer = null;

            if (Designer.IsInDesignMode)
            {
                if (designTimeComposer == null)
                    designTimeComposer = new DefaultDesignTimeComposer();
                composer = designTimeComposer;
            }
            else
            {
                if (runtimeComposer == null) // if the composer is not set then we should use the default one
                    runtimeComposer = new DefaultRuntimeComposer();
                composer = runtimeComposer;
            }

            var catalog = composer.InitializeContainer();
            MefMvvmExportProvider provider = new MefMvvmExportProvider(MefMvvmCatalog.CreateCatalog(catalog));
            CompositionContainer container = new CompositionContainer(provider);
            provider.SourceProvider = container;
            return container;

        }

        private static IComposer runtimeComposer { get; set; }
        private static IComposer designTimeComposer { get; set; }

        /// <summary>
        /// forces the Locator to use this composer
        /// </summary>
        ///<param name="designTimeComposer">The compose to create the CompositionContainer</param>
        ///<param name="runtimeComposer">The composer to use for runtime</param>
        public static void ApplyComposer(IComposer runtimeComposer)
        {
            LocatorBootstrapper.runtimeComposer = runtimeComposer;
        }
    }
}
