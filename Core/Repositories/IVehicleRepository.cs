using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
         Task<Vehicle> GetVehicle(int id);
    }
}