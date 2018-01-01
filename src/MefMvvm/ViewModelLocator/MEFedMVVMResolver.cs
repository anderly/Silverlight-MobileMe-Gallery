using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition;

namespace MefMvvm.ViewModelLocator
{
    /// <summary>
    /// Import resolver for the MefMvvm lib
    /// </summary>
    public class MefMvvmResolver
    {
        CompositionContainer _container;
        public MefMvvmResolver(CompositionContainer container)
        {
            this._container = container;
        }

        public void SatisfyImports(object attributedPart, object contextToInject)
        {
            SetContextToExportProvider(contextToInject);
            _container.SatisfyImportsOnce(attributedPart);
            SetContextToExportProvider(null);
        }

        /// <summary>
        /// Gets teh ViewModel export 
        /// </summary>
        /// <param name="vmContractName">The contract for the view model to get</param>
        /// <returns></returns>
        public Export GetViewModelByContract(string vmContractName, object contextToInject)
        {
            if(_container == null)
                return null;

            var viewModelTypeIdentity = AttributedModelServices.GetTypeIdentity(typeof(object));
            var requiredMetadata = new Dictionary<string, Type>();
            requiredMetadata[ExportViewModel.NameProperty] = typeof(string);
            requiredMetadata[ExportViewModel.IsDataContextAwareProperty] = typeof(bool);


            var definition = new ContractBasedImportDefinition(vmContractName, viewModelTypeIdentity,
                                                               requiredMetadata, ImportCardinality.ZeroOrMore, false,
                                                               false, CreationPolicy.Any);

            SetContextToExportProvider(contextToInject);
            var vmExports = _container.GetExports(definition);
            SetContextToExportProvider(null);

            var vmExport = vmExports.FirstOrDefault(e => e.Metadata[ExportViewModel.NameProperty].Equals(vmContractName));
            if (vmExport != null)
                return vmExport;
            return null;
        }

        public object GetExportedValue(Export export)
        {
            return _container.GetExportedValue<object>(export.Definition.ContractName);
        }

        internal void SetContextToExportProvider(object contextToInject)
        {
            if (_container.Providers != null && _container.Providers.Count >= 1)
            {
                //try to find the MefMvvmExportProvider
                foreach (var item in _container.Providers)
                {
                    var mefedProvider = item as MefMvvmExportProvider;
                    if (mefedProvider != null)
                        mefedProvider.SetContextToInject(contextToInject);
                }
            }
        }
    }
}
