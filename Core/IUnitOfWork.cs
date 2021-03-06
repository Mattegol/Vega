using System.Threading.Tasks;
using vega.Core.Repositories;

namespace vega.Core
{
    public interface IUnitOfWork
    {
        IVehicleRepository Vehicles { get; }

        IPhotoRepository Photos { get; }

        Task CompleteAsync();
    }
}