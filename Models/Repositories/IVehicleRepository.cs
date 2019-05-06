using System.Threading.Tasks;

namespace vega.Models.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
         Task<Vehicle> GetVehicle(int id);
    }
}