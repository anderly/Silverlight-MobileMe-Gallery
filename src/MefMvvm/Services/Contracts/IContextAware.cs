using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MefMvvm.Services.Contracts
{
    /// <summary>
    /// Interface used for services that want to have the context injected
    /// </summary>
    public interface IContextAware
    {
        void InjectContext(object context);
    }
}
