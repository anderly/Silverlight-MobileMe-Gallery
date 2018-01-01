using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MefMvvm.ViewModelLocator
{
    /// <summary>
    /// Interface to be implemented by ViewModels that want to do something when rendering in design time
    /// </summary>
    public interface IDesignTimeAware
    {
        void DesignTimeInitialization();
    }
}
