using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace MefMvvm.ViewModelLocator
{
    /// <summary>
    /// Interface for the entity responsable to creates the Composition Container that MefMvvm will use to resolve the ViewModels and services
    /// </summary>
    public interface IComposer
    {
        ComposablePartCatalog InitializeContainer();
    }
}
