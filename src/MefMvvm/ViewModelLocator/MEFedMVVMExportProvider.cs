using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using MefMvvm.Services.Contracts;

namespace MefMvvm.ViewModelLocator
{
    public class MefMvvmExportProvider : ExportProvider, IDisposable
    {
        private CatalogExportProvider _exportProvider;

        public MefMvvmExportProvider(ComposablePartCatalog catalog)
        {
            _exportProvider = new CatalogExportProvider(catalog);
            //support recomposition
            _exportProvider.ExportsChanged += (s, e) => OnExportsChanged(e); 
            _exportProvider.ExportsChanging += (s, e) => OnExportsChanging(e);
        }

        public ExportProvider SourceProvider
        {
            get
            {
                return _exportProvider.SourceProvider;
            }
            set
            {
                _exportProvider.SourceProvider = value;
            }
        }

        protected override System.Collections.Generic.IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var exports = _exportProvider.GetExports(definition, atomicComposition);
            foreach (var export in exports)
            {
                var wrappedExport = new Export(export.Definition, () => GetValue(export));
                yield return wrappedExport;
            }
        }

        private object GetValue(Export innerExport)
        {
            var value = innerExport.Value;
            var context = value as IContextAware;
            if (context != null)
            {
                context.InjectContext(_context);
            }
            return value;
        }

        private object _context;
        public void SetContextToInject(object context)
        {
            _context = context;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _exportProvider.Dispose();
        }

        #endregion
    }
}
