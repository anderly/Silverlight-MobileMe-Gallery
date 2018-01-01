using MefMvvm.ViewModelLocator;
using MefMvvm.Services.Contracts;
using System.ComponentModel.Composition;

namespace MefMvvm.Services.CommonServices
{
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    [ExportService(ServiceType.Both, typeof(IMediator))]
    public class Mediator : MediatorBase, IMediator
    {
    }
}
